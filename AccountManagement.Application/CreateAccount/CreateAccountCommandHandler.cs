using AccountManagement.Application.Abstractions.Messaging;
using AccountManagement.Domain.Abstractions;
using System.Text.RegularExpressions;
using AccountManagement.Domain.Account;

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
        var accountCreate = Account.Create(command.UserName);

        if (!accountCreate.IsSuccess)
        {
            return Result.Failure<CreateAccountResponse>(accountCreate.Error);
        }

        var account = accountCreate.Value;

        var alreadyExist = await _accountRepository.IsUsernameAlreadyExist(account.UserName, cancellationToken);
        if (alreadyExist)
        {
            account.GenerateUniqueUserName(_accountService, await _accountRepository.GetExistingUserNames(cancellationToken));
        }



        await _accountRepository.Add(account, cancellationToken);

        return Result.Success(new CreateAccountResponse(account.UserName));
    }
}