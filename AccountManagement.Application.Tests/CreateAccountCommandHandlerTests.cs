using System.Collections.Immutable;
using AccountManagement.Application.CreateAccount;
using AccountManagement.Domain.Account;

namespace AccountManagement.Application.Tests
{
    public class CreateAccountCommandHandlerTests
    {
        private IAccountRepository _fakeAccountRepository = new FakeAccountRepository();


        [Fact]
        public async Task CreateAccount_WithNullUserLogin_ShouldReturnResultFailure()
        {
            var command = new CreateAccountCommand(null);

            var sut = new CreateAccountCommandHandler(_fakeAccountRepository, new AccountService());

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData("AA")]
        [InlineData("")]
        [InlineData("AaA")]
        [InlineData("A1A")]
        [InlineData("ABCD")]
        public async Task CreateAccount_WithLoginThatNotMatchThreeCapitalLetters_ShouldReturnResultFailure(string userLogin)
        {
            var command = new CreateAccountCommand(userLogin);

            var sut = new CreateAccountCommandHandler(_fakeAccountRepository, new AccountService());

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(AccountErrors.NotExactlyThreeCapitalLetters, result.Error);
        }

        [Fact]
        public async Task CreateAccount_ValidUserNameAndNotAlreadyExists_ShouldReturnResultSuccessAndSameUserLoginCreated()
        {
            var userLogin = "AEZ";
            var command = new CreateAccountCommand(userLogin);

            var sut = new CreateAccountCommandHandler(_fakeAccountRepository, new AccountService());

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(userLogin, result.Value.UserNameCreated);
        }
        
        
        [Fact]
        public async Task CreateAccount_ValidUserNameAndNotAlreadyExists_ShouldAddAccountToRepository()
        {
            var userLogin = "AEZ";
            var command = new CreateAccountCommand(userLogin);
            var repository = new FakeAccountRepository(false);

            var sut = new CreateAccountCommandHandler(repository, new AccountService());

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.Equal(1, repository.Accounts.Count);
            Assert.Equal(userLogin, repository.Accounts.First().UserName);
        }


        [Fact]
        public async Task CreateAccount_ValidUserNameAndAlreadyExists_ShouldReturnResultSuccessWithANewUserName()
        {
            var userLogin = "AEZ";
            var command = new CreateAccountCommand(userLogin);
            var repository = new FakeAccountRepository(true);
            await repository.Add(Account.Create(UserName.Create(userLogin).Value), CancellationToken.None);

            var sut = new CreateAccountCommandHandler(repository, new AccountService());

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotEqual(userLogin,result.Value.UserNameCreated);
        }
    }

    public class FakeAccountRepository : IAccountRepository
    {
        private readonly bool _returnIsExist;

        public HashSet<Account> Accounts { get; private set; } = new();

        public FakeAccountRepository(bool returnIsExist = false)
        {
            _returnIsExist = returnIsExist;
        }

        public Task<bool> IsUsernameAlreadyExist(string username, CancellationToken cancellationToken)
        {
            return Task.FromResult(_returnIsExist);
        }

        public Task Add(Account account, CancellationToken cancellationToken)
        {
            Accounts.Add(account);
            return Task.CompletedTask;
        }

        public async Task<ImmutableHashSet<string>> GetExistingUserNames(CancellationToken cancellationToken)
        {
            await Task.Yield();
            return Accounts.Select(s => s.UserName.Value).ToImmutableHashSet();
        }
    }
}