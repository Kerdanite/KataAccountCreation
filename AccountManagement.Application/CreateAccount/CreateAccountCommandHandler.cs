using AccountManagement.Application.Abstractions.Messaging;
using AccountManagement.Domain.Abstractions;

namespace AccountManagement.Application.CreateAccount;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Result>
{
    public async Task<Result<Result>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        await Task.Yield();

        return Result.Success();
    }
}