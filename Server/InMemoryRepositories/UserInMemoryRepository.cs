using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository: IUserRepository
{
    public List<User> Users;

    public Task<User> AddAsync(User user)
    {
        user.Id = Users.Any()
            ? Users.Max(p => p.Id) + 1
            : 1;
        Users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? User = Users.SingleOrDefault(p=>p.Id == user.Id);
        if (User == null)
        {
            throw new InvalidOperationException($"User {user.Id} was not found.");
        }

        Users.Remove(User);
        Users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int Id)
    {
        User? userToRemove = Users.SingleOrDefault(p => p.Id == Id);
        if (userToRemove == null)
        {
            throw new InvalidOperationException($"User {Id} was not found.");
        }
        Users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int Id)
    {
        User? user = Users.SingleOrDefault(p => p.Id == Id);
        if (user == null)
        {
            throw new InvalidOperationException($"User {Id} was not found.");
        }
        return Task.FromResult(user);
    }

    public IQueryable<User> GetMany()
    {
        return Users.AsQueryable();
    }
}