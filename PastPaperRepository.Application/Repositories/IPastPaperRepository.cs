using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public interface IPastPaperRepository
{
    Task<bool> CreatePastPaperAsync(PastPapers pastPapers);
    Task<PastPapers?> GetPastPaperByIdAsync(string pastPaperId);
    Task<PastPapers?> GetPastPaperBySlugAsync(string slug);
    
    Task<IEnumerable<PastPapers>> GetAllPastPapersAsync();
    
    Task<bool> UpdatePastPaperAsync(PastPapers pastPapers);
    Task<bool> DeletePastPaperAsync(string pastPaperId);
    Task<bool> ExistsById(string pastPaperId);
}