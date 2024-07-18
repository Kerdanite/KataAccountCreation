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

            var sut = new CreateAccountCommandHandler(_fakeAccountRepository);

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

            var sut = new CreateAccountCommandHandler(_fakeAccountRepository);

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(AccountErrors.NotExactlyThreeCapitalLetters, result.Error);
        }

        [Fact]
        public async Task CreateAccount_ValidUserNameAndNotAlreadyExists_ShouldReturnResultSuccess()
        {
            var userLogin = "AEZ";
            var command = new CreateAccountCommand(userLogin);

            var sut = new CreateAccountCommandHandler(_fakeAccountRepository);

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }
        
        
        [Fact]
        public async Task CreateAccount_ValidUserNameAndAlreadyExists_ShouldReturnResultFailure()
        {
            var userLogin = "AEZ";
            var command = new CreateAccountCommand(userLogin);

            var sut = new CreateAccountCommandHandler(new FakeAccountRepository(false));

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }

    public class FakeAccountRepository : IAccountRepository
    {
        private readonly bool _returnIsexist;

        public FakeAccountRepository(bool returnIsexist = true)
        {
            _returnIsexist = returnIsexist;
        }

        public Task<bool> IsUsernameAlreadyExist(string username, CancellationToken cancellationToken)
        {
            return Task.FromResult(_returnIsexist);
        }
    }
}