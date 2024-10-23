using ApiContracts;
using Entity;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<User>> AddUser([FromBody] CreateUserDto request)
    {
        try
        {
            User user = new(request.UserName, request.Password);
            User created = await userRepo.AddAsync(user);
            UserDto dto = new() { Id = created.Id, UserName = created.Username };
            return Created($"/Users/{dto.Id}", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }

    }
    
    //GET: /Users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetSingleUser(int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            UserDto userDto = new() { Id = user.Id, UserName = user.Username };
            return Ok(userDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(5000, e.Message);
        }
    }
    
    //GET: /Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetManyUsers([FromQuery] string? username)
    {
        try
        {
            var users = userRepo.GetMany();
            if (!string.IsNullOrEmpty(username))
            {
                users = users.Where(user => user.Username.Contains(username, StringComparison.OrdinalIgnoreCase));
            }

            var userDtos = users.Select(user => new UserDto() { Id = user.Id, UserName = user.Username });
            return Ok(userDtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    // PUT: /Users/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        try
        {   
            User user = await userRepo.GetSingleAsync(id);
            if (user == null)
            {
                return NotFound(); // 404 Not Found
            }
            
            user.Username = request.UserName;
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.Password = request.Password; 
            }

            await userRepo.UpdateAsync(user);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    //Delete: /Users/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> DeleteUser(int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            if (user == null)
            {
                return NotFound(); // 404 Not Found
            }

            await userRepo.DeleteAsync(id);
            return NoContent(); // 204 No Content
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}