using AccountManagement.Domain.Abstractions;
using System.Text.RegularExpressions;

namespace AccountManagement.Domain.Account;

public record UserName
{
    private UserName()
    {
    }

    public string Value { get; init; }

    public static Result<UserName> Create(string value)
    {
        var isUserNameValid = ValidateUserName(value);
        if (isUserNameValid.IsFailure)
        {
            return Result.Failure<UserName>(isUserNameValid.Error);
        }

        return new UserName() { Value = value };
    }

    private static Result ValidateUserName(string userName)
    {
        if (userName is null)
        {
            return Result.Failure(Error.NullValue);
        }

        if (!UserNameContainsExactlyThreeCapitalLetters(userName))
        {
            return Result.Failure(AccountErrors.NotExactlyThreeCapitalLetters);
        }

        return Result.Success();
    }

    private static bool UserNameContainsExactlyThreeCapitalLetters(string commandUserName)
    {
        var pattern = @"^[A-Z]{3}$";
        var regex = new Regex(pattern);

        return regex.IsMatch(commandUserName);
    }


    public static implicit operator string(UserName userName)
    {
        return userName.Value; 
    }

}