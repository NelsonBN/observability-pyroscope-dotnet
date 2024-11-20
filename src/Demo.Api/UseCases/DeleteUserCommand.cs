using System;
using System.Threading;
using Demo.Api.Domain;

namespace Demo.Api.UseCases;

public sealed class DeleteUserCommand(IUsersRepository repository)
{
    private readonly IUsersRepository _repository = repository;

    public void HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        if(!_repository.Any(id, cancellationToken))
        {
            throw new UserNotFoundException(id);
        }

        _repository.Delete(id, cancellationToken);
    }
}
