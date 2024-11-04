namespace PastPaperRepository.Contracts.Responses;

public class PagedResponse<TResponse>
{
    public IEnumerable<TResponse> PastPapers { get; init; } = Enumerable.Empty<TResponse>();
    
    public required int PageSize { get; init; }
    public required int Page { get; init; }
    public required int TotalCount { get; init; }
    public bool HasNextPage => TotalCount > (Page * PageSize);

}