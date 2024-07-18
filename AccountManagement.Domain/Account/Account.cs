using AccountManagement.Domain.Abstractions;
using System.Text.RegularExpressions;

namespace AccountManagement.Domain.Account;

public sealed class Account : Entity
{
    private Account(UserName userName)
    {
        UserName = userName;
    }


    public UserName UserName { get; private set; }


    public static Result<Account> Create(string userName)
    {
        var userNameResult = UserName.Create(userName);
        if (userNameResult.IsFailure)
        {
            return Result.Failure<Account>(userNameResult.Error);
        }
    
        return new Account(userNameResult.Value); 
    }


}