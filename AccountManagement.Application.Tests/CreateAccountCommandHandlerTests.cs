using AccountManagement.Application.CreateAccount;
using AccountManagement.Domain.Account;

namespace AccountManagement.Application.Tests
{
    public class CreateAccountCommandHandlerTests
    {
        [Fact]
        public async Task CreateAccount_WithNullUserLogin_ShouldReturnResultFailure()
        {
            var command = new CreateAccountCommand(null);

            var sut = new CreateAccountCommandHandler();

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

            var sut = new CreateAccountCommandHandler();

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(AccountErrors.NotExactlyThreeCapitalLetters, result.Error);
        }

        [Theory]
        [InlineData("AAA")]
        [InlineData("ABC")]
        [InlineData("AXP")]
        public async Task CreateAccount_WithLoginThatMatchThreeCapitalLetters_ShouldReturnResultSuccess(string userLogin)
        {
            var command = new CreateAccountCommand(userLogin);

            var sut = new CreateAccountCommandHandler();

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }
    }
}