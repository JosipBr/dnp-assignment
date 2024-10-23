namespace ApiContracts;

public class UpdateUserDto
{
    public required string UserName { get; set; }
    public string? Password { get; set; } // Password is optional during update
}