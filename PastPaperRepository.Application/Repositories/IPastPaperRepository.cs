using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public interface IPastPaperRepository
{
    Task<bool> CreatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default);
    Task<IEnumerable<QuestionAnswer>?> GetPastPaperByIdAsync(string pastPaperId, CancellationToken token = default);
    Task<PastPapers?> GetPastPaperBySlugAsync(string slug, CancellationToken token = default);

    Task<IEnumerable<PastPapers>> GetAllPastPapersAsync(GetAllPastPapersOptions options,
        CancellationToken token = default);

    Task<bool> UpdatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default);
    Task<bool> DeletePastPaperAsync(string pastPaperId, CancellationToken token = default);
    Task<bool> ExistsById(string pastPaperId, CancellationToken token = default);

    Task<int> GetCountAsync(string? title, int? year, CancellationToken token = default);
    
    Task<IEnumerable<DynamicPastPaperModal>> GetDynamicPastPapersAsync(int id, CancellationToken token = default);
    
    Task<IEnumerable<SearchPastPaper>> SearchPastPapersAsync(string title, CancellationToken token = default);
}