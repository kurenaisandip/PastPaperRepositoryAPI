using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Services;

public interface IPastPaperService
{
    Task<bool> CreatePastPaperAsync(PastPapers pastPapers);
    Task<PastPapers?> GetPastPaperByIdAsync(string pastPaperId);
    Task<PastPapers?> GetPastPaperBySlugAsync(string slug);
    
    Task<IEnumerable<PastPapers>> GetAllPastPapersAsync();
    
    Task<PastPapers?> UpdatePastPaperAsync(PastPapers pastPapers);
    Task<bool> DeletePastPaperAsync(string pastPaperId);
}