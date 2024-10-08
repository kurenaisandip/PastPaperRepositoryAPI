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
       var pastPaperIndex = _papers.FindIndex(x => x.PastPaperId == pastPapers.PastPaperId);

       if (pastPaperIndex == -1)
       {
           return Task.FromResult(false);
       }
       
       _papers[pastPaperIndex] = pastPapers;
       return Task.FromResult(true);
    }

    public Task<bool> DeletePastPaperAsync(Guid pastPaperId)
    {
        var pastPaper = _papers.SingleOrDefault(x => x.PastPaperId == pastPaperId);
        if (pastPaper == null)
        {
            return Task.FromResult(false);
        }

        _papers.Remove(pastPaper);
        return Task.FromResult(true);
    }
}