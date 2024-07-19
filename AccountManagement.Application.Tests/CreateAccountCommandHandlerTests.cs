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
        public async Task CreateAccount_ValidUserNameAndNotAlreadyExists_ShouldReturnResultSuccessAndSameUserLoginCreated()
        {
            var userLogin = "AEZ";
            var command = new CreateAccountCommand(userLogin);

            var sut = new CreateAccountCommandHandler(_fakeAccountRepository);

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(userLogin, result.Value.UserNameCreated);
        }
        
        
        [Fact]
        public async Task CreateAccount_ValidUserNameAndAlreadyExists_ShouldReturnResultFailure()
        {
            var userLogin = "AEZ";
            var command = new CreateAccountCommand(userLogin);

            var sut = new CreateAccountCommandHandler(new FakeAccountRepository(true));

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }

    public class FakeAccountRepository : IAccountRepository
    {
        private readonly bool _returnIsExist;

        public FakeAccountRepository(bool returnIsExist = false)
        {
            _returnIsExist = returnIsExist;
        }

        public Task<bool> IsUsernameAlreadyExist(string username, CancellationToken cancellationToken)
        {
            return Task.FromResult(_returnIsExist);
        }
    }
}