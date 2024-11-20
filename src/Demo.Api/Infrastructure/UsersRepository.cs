using System;
using System.Collections.Generic;
using System.Threading;
using Demo.Api.Domain;

namespace Demo.Api.Infrastructure;

public sealed class UsersRepository : IUsersRepository
{
    private readonly Dictionary<Guid, User> _store = [];


    public IEnumerable<User> List(CancellationToken cancellationToken = default)
        => _store.Values;

    public User? Get(Guid id, CancellationToken cancellationToken = default)
    {
        if(_store.TryGetValue(id, out var user))
        {
            return user;
        }
        return null;
    }

    public void Add(User user, CancellationToken cancellationToken = default)
    {
        if(_store.ContainsKey(user.Id))
        {
            throw new InvalidOperationException($"User with id {user.Id} already exists.");
        }
        _store.Add(user.Id, user);
    }

    public void Update(User user, CancellationToken cancellationToken = default)
    {
        if(!_store.ContainsKey(user.Id))
        {
            throw new InvalidOperationException($"User with id {user.Id} does not exist.");
        }

        _store[user.Id] = user;
    }

    public void Delete(Guid id, CancellationToken cancellationToken = default)
    {
        if(!_store.ContainsKey(id))
        {
            throw new InvalidOperationException($"User with id {id} does not exist.");
        }

        _store.Remove(id);
    }

    public bool Any(Guid id, CancellationToken cancellationToken = default)
        => _store.ContainsKey(id);
}
