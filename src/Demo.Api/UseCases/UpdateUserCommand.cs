using System;
using System.Threading;
using Demo.Api.Domain;
using Demo.Api.DTOs;

namespace Demo.Api.UseCases;

public sealed class UpdateUserCommand(IUsersRepository repository)
{
    private readonly IUsersRepository _repository = repository;

    public void HandleAsync(Guid id, UserRequest request, CancellationToken cancellationToken)
    {
        var user = _repository.Get(id, cancellationToken);
        if(user is null)
        {
            throw new UserNotFoundException(id);
        }

        user.Update(
            request.Name,
            request.Email,
            request.Phone);

        _repository.Update(user, cancellationToken);
    }
}
