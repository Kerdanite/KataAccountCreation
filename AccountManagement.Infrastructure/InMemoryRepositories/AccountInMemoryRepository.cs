using AccountManagement.Domain.Account;

namespace AccountManagement.Infrastructure.Repositories;

public class AccountInMemoryRepository : IAccountRepository
{
    private IDictionary<string, Account> _accounts = new Dictionary<string, Account>();
    public Task<bool> IsUsernameAlreadyExist(string username, CancellationToken cancellationToken)
    {
        return Task.FromResult(_accounts.ContainsKey(username));
    }
}