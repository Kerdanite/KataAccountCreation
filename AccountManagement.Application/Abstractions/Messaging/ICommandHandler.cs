using AccountManagement.Domain.Abstractions;
using MediatR;

namespace AccountManagement.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}