using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repository;

public interface IPastPaperRepository
{
    Task<bool> CreatePaspaperAsync(PastPapers pastPapers);
    Task<PastPapers?> GetPastPaperByIdAsync(Guid pastPaperId);
    
    Task<IEnumerable<PastPapers>> GetAllPastPapersAsync();
    
    Task<bool> UpdatePastPaperAsync(PastPapers pastPapers);
    Task<bool> DeletePastPaperAsync(Guid pastPaperId);
}