using FluentValidation;
using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Repositories;
using PastPaperRepository.Application.Validators;

namespace PastPaperRepository.Application.Services;

public class PastPaperService: IPastPaperService
{
    private readonly IPastPaperRepository _pastPaperRepository;
    private readonly IValidator<PastPapers> _pastPaperValidators;

    public PastPaperService(IPastPaperRepository pastPaperRepository, IValidator<PastPapers> pastPaperValidators)
    {
        _pastPaperRepository = pastPaperRepository;
        _pastPaperValidators = pastPaperValidators;
    }

    public async Task<bool> CreatePastPaperAsync(PastPapers pastPapers)
    {
        await _pastPaperValidators.ValidateAndThrowAsync(pastPapers);
        return await _pastPaperRepository.CreatePastPaperAsync(pastPapers);
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
        await _pastPaperValidators.ValidateAndThrowAsync(pastPapers);
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