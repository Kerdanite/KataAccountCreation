using AccountManagement.Application.Abstractions.Messaging;
using AccountManagement.Domain.Abstractions;

namespace AccountManagement.Application.CreateAccount;

public record CreateAccountCommand(string UserName) : ICommand<Result>;