namespace PastPaperRepository.Application.Models;

public class GetAllPastPapersOptions
{
    public required string? Title { get; set; }
    public required int? Year { get; set; }
    public  string? UserID { get; set; }
    
    public required string? SortField { get; set; }
    
    public SortOrder? SortOrder { get; set; }
}

public enum SortOrder
{
    UnSorted,
    Ascending,
    Descending
}