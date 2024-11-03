using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Services;

public interface IPastPaperService
{
    Task<bool> CreatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default);
    Task<PastPapers?> GetPastPaperByIdAsync(string pastPaperId, string? userId = default, CancellationToken token = default);
    Task<PastPapers?> GetPastPaperBySlugAsync(string slug, CancellationToken token = default);
    
    Task<IEnumerable<PastPapers>> GetAllPastPapersAsync(string? userId = default,CancellationToken token = default);
    
    Task<PastPapers?> UpdatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default);
    Task<bool> DeletePastPaperAsync(string pastPaperId, CancellationToken token = default);
}