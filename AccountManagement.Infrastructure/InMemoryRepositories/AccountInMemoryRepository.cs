using AccountManagement.Domain.Account;

namespace AccountManagement.Infrastructure.InMemoryRepositories;

public class AccountInMemoryRepository : IAccountRepository
{
    private IDictionary<string, Account> _accounts = new Dictionary<string, Account>();
    public Task<bool> IsUsernameAlreadyExist(string username, CancellationToken cancellationToken)
    {
        return Task.FromResult(_accounts.ContainsKey(username));
    }

    public Task Add(Account account, CancellationToken cancellationToken)
    {
        _accounts.Add(account.UserName, account);
        return Task.CompletedTask;
    }
}