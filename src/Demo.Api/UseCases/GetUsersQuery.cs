using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Demo.Api.Domain;
using Demo.Api.DTOs;

namespace Demo.Api.UseCases;

public sealed class GetUsersQuery(IUsersRepository repository)
{
    private readonly IUsersRepository _repository = repository;

    public IEnumerable<UserResponse> HandleAsync(CancellationToken cancellationToken)
    {
        var users = _repository.List(cancellationToken);

        var result = users.Select(n => (UserResponse)n);

        return result;
    }
}
