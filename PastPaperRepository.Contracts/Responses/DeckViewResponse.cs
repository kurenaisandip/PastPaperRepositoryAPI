namespace PastPaperRepository.Contracts.Responses;

public class DeckViewResponse
{
    public string PastPaperId { get; init; } = default!;
    public string Title { get; init; } = default!;
    public long Totalquestions { get; init; } = default!;
}