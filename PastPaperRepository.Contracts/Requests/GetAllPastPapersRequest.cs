namespace PastPaperRepository.Contracts.Requests;

public class GetAllPastPapersRequest
{
    public required string? Title { get; init; }
    public required int? Year { get; init; }
}