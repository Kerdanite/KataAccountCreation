using AccountManagement.Application.Abstractions.Messaging;
using AccountManagement.Domain.Abstractions;
using System.Text.RegularExpressions;
using AccountManagement.Domain.Account;

namespace AccountManagement.Application.CreateAccount;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand>
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        await Task.Yield();


        var accountCreate = Account.Create(command.UserName);

        if (!accountCreate.IsSuccess)
        {
            return accountCreate;
        }

        var account = accountCreate.Value;

        var alreadyExist = await _accountRepository.IsUsernameAlreadyExist(account.UserName, cancellationToken);
        if (!alreadyExist)
        {
            return Result.Failure(null);
        }

        return Result.Success();
    }

    

    
}