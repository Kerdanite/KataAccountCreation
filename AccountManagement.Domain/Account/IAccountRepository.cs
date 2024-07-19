namespace AccountManagement.Domain.Account;

public interface IAccountRepository
{
    Task<bool> IsUsernameAlreadyExist(string username, CancellationToken cancellationToken);
    Task Add(Account account, CancellationToken cancellationToken);
}