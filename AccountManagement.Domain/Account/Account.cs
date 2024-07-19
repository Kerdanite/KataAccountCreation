using AccountManagement.Domain.Abstractions;
using System.Collections.Immutable;
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


    public void GenerateUniqueUserName(AccountService accountService, ImmutableHashSet<string> existingUserLogins)
    {
        var newUserLogin = UserName.Create(accountService.GenerateUniqueUserLogin(existingUserLogins));
        if (newUserLogin.IsFailure)
        {
            throw new ApplicationException("Auto generated UserLogin does not match UserLogin rules");
        }
        UserName = newUserLogin.Value;
    }
}