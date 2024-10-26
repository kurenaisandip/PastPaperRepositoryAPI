using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Repositories;

namespace PastPaperRepository.Application.Services;

public class PastPaperService: IPastPaperService
{
    private readonly IPastPaperRepository _pastPaperRepository;

    public PastPaperService(IPastPaperRepository pastPaperRepository)
    {
        _pastPaperRepository = pastPaperRepository;
    }

    public Task<bool> CreatePastPaperAsync(PastPapers pastPapers)
    {
        return _pastPaperRepository.CreatePastPaperAsync(pastPapers);
    }

    public Task<PastPapers?> GetPastPaperByIdAsync(string pastPaperId)
    {
        return _pastPaperRepository.GetPastPaperByIdAsync(pastPaperId);
    }

    public Task<PastPapers?> GetPastPaperBySlugAsync(string slug)
    {
        return _pastPaperRepository.GetPastPaperBySlugAsync(slug);
    }

    public Task<IEnumerable<PastPapers>> GetAllPastPapersAsync()
    {
        return _pastPaperRepository.GetAllPastPapersAsync();
    }

    public async Task<PastPapers?> UpdatePastPaperAsync(PastPapers pastPapers)
    {
      var pastPaperExists = await _pastPaperRepository.ExistsById(pastPapers.PastPaperId);
      if (!pastPaperExists)
      {
          return null;
      }
      
      await _pastPaperRepository.UpdatePastPaperAsync(pastPapers);
      return pastPapers;
    }

    public Task<bool> DeletePastPaperAsync(string pastPaperId)
    {
        return _pastPaperRepository.DeletePastPaperAsync(pastPaperId);
    }
}