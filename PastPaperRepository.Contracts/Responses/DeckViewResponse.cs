namespace PastPaperRepository.Contracts.Responses;

public class DeckViewResponse
{
    public string PastPaperId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public long Totalquestions { get; set; } = default!;
}