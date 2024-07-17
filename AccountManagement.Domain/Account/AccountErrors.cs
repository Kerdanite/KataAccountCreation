using AccountManagement.Domain.Abstractions;

namespace AccountManagement.Domain.Account;

public static class AccountErrors
{
    public static Error NotExactlyThreeCapitalLetters = new("Account.UserNameFormat", "Username should have exactly 3 capitals letters");
}