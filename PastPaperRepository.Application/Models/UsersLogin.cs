namespace PastPaperRepository.Application.Models;

public class UsersLogin
{
    public string? Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}