namespace PastPaperRepository.Application.Models;

public class GetAllPastPapersOptions
{
    public required string? Title { get; set; }
    public required int? Year { get; set; }
    public  string? UserID { get; set; }
}