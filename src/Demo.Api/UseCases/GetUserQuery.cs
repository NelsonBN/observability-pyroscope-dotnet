using System;
using System.Threading;
using Demo.Api.Domain;
using Demo.Api.DTOs;

namespace Demo.Api.UseCases;

public sealed class GetUserQuery(IUsersRepository repository)
{
    private readonly IUsersRepository _repository = repository;

    public UserResponse HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = _repository.Get(id, cancellationToken);
        if(user is null)
        {
            throw new UserNotFoundException(id);
        }

        return user;
    }
}
