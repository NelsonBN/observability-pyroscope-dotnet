using System;
using System.Threading;
using Demo.Api.Domain;
using Demo.Api.DTOs;

namespace Demo.Api.UseCases;

public sealed class CreateUserCommand(IUsersRepository repository)
{
    private readonly IUsersRepository _repository = repository;

    public Guid HandleAsync(UserRequest request, CancellationToken cancellationToken)
    {
        var user = User.Create(
            request.Name,
            request.Email,
            request.Phone);

        _repository.Add(user, cancellationToken);

        return user.Id;
    }
}
