using AccountManagement.Domain.Account;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFRepositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _dbContext;

    public AccountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsUsernameAlreadyExist(string username, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Account>().AnyAsync(a => a.UserName == username, cancellationToken);
    }

    public async Task Add(Account account, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(account, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ImmutableHashSet<string>> GetExistingUserNames(CancellationToken cancellationToken)
    {
        return (await _dbContext.Set<Account>().Select(s => s.UserName.Value).ToListAsync(cancellationToken)).ToImmutableHashSet();
    }
}