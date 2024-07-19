using AccountManagement.Application.Abstractions.Messaging;
using AccountManagement.Domain.Abstractions;
using System.Text.RegularExpressions;
using AccountManagement.Domain.Account;

namespace AccountManagement.Application.CreateAccount;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, CreateAccountResponse>
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<CreateAccountResponse>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        await Task.Yield();


        var accountCreate = Account.Create(command.UserName);

        if (!accountCreate.IsSuccess)
        {
            return Result.Failure<CreateAccountResponse>(accountCreate.Error);
        }

        var account = accountCreate.Value;

        var alreadyExist = await _accountRepository.IsUsernameAlreadyExist(account.UserName, cancellationToken);
        if (!alreadyExist)
        {
            return Result.Failure<CreateAccountResponse>(null);
        }

        return Result.Success(new CreateAccountResponse(account.UserName));
    }

    

    
}