using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repository;

public class PastPaperRepository : IPastPaperRepository
{
    private readonly List<PastPapers> _papers = new();
    public Task<bool> CreatePaspaperAsync(PastPapers pastPapers)
    {
        throw new NotImplementedException();
    }

    public Task<PastPapers?> GetPastPaperByIdAsync(Guid pastPaperId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PastPapers>> GetAllPastPapersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdatePastPaperAsync(PastPapers pastPapers)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeletePastPaperAsync(Guid pastPaperId)
    {
        throw new NotImplementedException();
    }
}