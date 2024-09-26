using System.Text.Json;
using Entity;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository: IUserRepository
{
    private readonly string _filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(usersAsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        int maxId = users.Count > 0 ? users.Max(x => x.Id)+1 : 1;
        user.Id = maxId;
        users.Add(user);
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, usersAsJson);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson);
        User? User = users.SingleOrDefault(c=>c.Id == user.Id);
        if (User == null)
        {
            throw new InvalidOperationException($"User {user.Id} was not found.");
        }
        users.Remove(User);
        users.Add(user);
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, usersAsJson);
    }

    public async Task DeleteAsync(int Id)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson);
        User? User = users.SingleOrDefault(c=>c.Id == Id);
        if (User == null)
        {
            throw new InvalidOperationException($"User {Id} was not found.");
        }
        users.Remove(User);
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, usersAsJson);
    }

    public async Task<User> GetSingleAsync(int Id)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson);
        User? user = users.SingleOrDefault(p => p.Id == Id);
        if (user == null)
        {
            throw new InvalidOperationException($"User {Id} was not found.");
        }
        return user;
    }

    public  IQueryable<User> GetMany()
    {
        string usersAsJson =  File.ReadAllTextAsync(_filePath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson);
        return users.AsQueryable();
    }
}