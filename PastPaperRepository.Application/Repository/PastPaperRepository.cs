using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repository;

public class PastPaperRepository : IPastPaperRepository
{
    private readonly List<PastPapers> _papers = new();
    public Task<bool> CreatePaspaperAsync(PastPapers pastPapers)
    {
        _papers.Add(pastPapers);
        return Task.FromResult(true);
    }

    public Task<PastPapers?> GetPastPaperByIdAsync(Guid pastPaperId)
    {
        var pastPaper = _papers.SingleOrDefault(x => x.PastPaperId == pastPaperId);
        return Task.FromResult(pastPaper);
    }

    public Task<IEnumerable<PastPapers>> GetAllPastPapersAsync()
    {
        return Task.FromResult(_papers.AsEnumerable());
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