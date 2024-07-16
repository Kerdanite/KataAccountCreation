using AccountManagement.Domain.Abstractions;
using MediatR;

namespace AccountManagement.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}