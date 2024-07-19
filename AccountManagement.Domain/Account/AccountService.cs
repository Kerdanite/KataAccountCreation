using System.Collections.Immutable;

namespace AccountManagement.Domain.Account;

public class AccountService
{
    private static readonly Lazy<HashSet<string>> _allPossibleUserLogins = new Lazy<HashSet<string>>(GenerateAllPossibleUserLogins);
    private static HashSet<string> GenerateAllPossibleUserLogins()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var allLogins = new HashSet<string>(26 * 26 * 26);
        for (int i = 0; i < chars.Length; i++)
        {
            for (int j = 0; j < chars.Length; j++)
            {
                for (int k = 0; k < chars.Length; k++)
                {
                    allLogins.Add(new string(new[] { chars[i], chars[j], chars[k] }));
                }
            }
        }
        return allLogins;
    }

    public string GenerateUniqueUserLogin(ImmutableHashSet<string> existingUserLogins)
    {
        Random random = new Random();
        var availableUserLogins = _allPossibleUserLogins.Value.Except(existingUserLogins).ToList();
        if (availableUserLogins.Count == 0)
        {
            throw new Exception("No available UserLogins left");
        }
        int index = random.Next(availableUserLogins.Count);
        string newUserLogin = availableUserLogins[index];
        return newUserLogin;
    }
}