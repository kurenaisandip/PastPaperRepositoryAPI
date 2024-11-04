namespace PastPaperRepository.Contracts.Requests;

public class GetAllPastPapersRequest : PageRequest
{
    public required string? Title { get; init; }
    public required int? Year { get; init; }
    
    public required string? SortBy { get; init; }
}