using System;
using System.Collections.Generic;
using System.Threading;

namespace Demo.Api.Domain;

public interface IUsersRepository
{
    IEnumerable<User> List(CancellationToken cancellationToken = default);
    User? Get(Guid id, CancellationToken cancellationToken = default);
    void Add(User user, CancellationToken cancellationToken = default);
    void Update(User user, CancellationToken cancellationToken = default);
    void Delete(Guid id, CancellationToken cancellationToken = default);
    bool Any(Guid id, CancellationToken cancellationToken = default);
}
