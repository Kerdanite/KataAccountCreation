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


    public static Account Create(UserName userName)
    {
        return new Account(userName); 
    }


    
}