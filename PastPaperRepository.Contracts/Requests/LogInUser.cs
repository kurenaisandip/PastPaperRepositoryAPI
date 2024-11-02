namespace PastPaperRepository.Contracts.Requests;

public class LogInUser
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}