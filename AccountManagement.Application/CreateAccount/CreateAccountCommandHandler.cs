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

        var validationResult = ValidateAccount(command);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }


        var alreadyExist = await _accountRepository.IsUsernameAlreadyExist(command.UserName, cancellationToken);
        if (!alreadyExist)
        {
            return Result.Failure(null);
        }

        return Result.Success();
    }

    private Result ValidateAccount(CreateAccountCommand command)
    {
        if (command.UserName is null)
        {
            return Result.Failure(Error.NullValue);
        }

        if (!UserNameContainsExactlyThreeCapitalLetters(command.UserName))
        {
            return Result.Failure(AccountErrors.NotExactlyThreeCapitalLetters);
        }

        return Result.Success();
    }

    private bool UserNameContainsExactlyThreeCapitalLetters(string commandUserName)
    {
        var pattern = @"^(?:[^A-Z]*[A-Z]){3}[^A-Z]*$";
        var regex = new Regex(pattern);

        return regex.IsMatch(commandUserName);
    }
}