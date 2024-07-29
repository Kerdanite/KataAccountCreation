using AccountManagement.Application.Abstractions.Messaging;
using AccountManagement.Domain.Abstractions;
using System.Text.RegularExpressions;
using AccountManagement.Domain.Account;
using System.Collections.Immutable;

namespace AccountManagement.Application.CreateAccount;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, CreateAccountResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly AccountService _accountService;

    public CreateAccountCommandHandler(IAccountRepository accountRepository, AccountService accountService)
    {
        _accountRepository = accountRepository;
        _accountService = accountService;
    }

    public async Task<Result<CreateAccountResponse>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var isAccountNameValid = ValidateAccountName(command.UserName);
        if (!isAccountNameValid.IsSuccess)
        {
            return Result.Failure<CreateAccountResponse>(isAccountNameValid.Error);
        }


        var userName = await HandleUserNameUnicity(cancellationToken, isAccountNameValid.Value);

        var accountCreate = Account.Create(userName);

        await _accountRepository.Add(accountCreate, cancellationToken);

        return Result.Success(new CreateAccountResponse(accountCreate.UserName));
    }

    private Result<UserName> ValidateAccountName(string userName)
    {
        return UserName.Create(userName);
    }

    private async Task<UserName> HandleUserNameUnicity(CancellationToken cancellationToken, UserName userName)
    {
        var alreadyExist = await _accountRepository.IsUsernameAlreadyExist(userName, cancellationToken);
        if (alreadyExist)
        {
            return GenerateUniqueUserName(_accountService, await _accountRepository.GetExistingUserNames(cancellationToken));
        }

        return userName;
    }

    private UserName GenerateUniqueUserName(AccountService accountService, ImmutableHashSet<string> existingUserLogins)
    {
        var newUserLogin = UserName.Create(accountService.GenerateUniqueUserLogin(existingUserLogins));
        if (newUserLogin.IsFailure)
        {
            throw new ApplicationException("Auto generated UserLogin does not match UserLogin rules");
        }
        return newUserLogin.Value;
    }
}