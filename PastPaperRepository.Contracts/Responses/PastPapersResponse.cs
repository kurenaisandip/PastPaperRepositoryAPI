namespace PastPaperRepository.Contracts.Responses;

public class PastPapersResponse
{
    public IEnumerable<PastPaperResponse> PastPapers { get; init; } = Enumerable.Empty<PastPaperResponse>();
}