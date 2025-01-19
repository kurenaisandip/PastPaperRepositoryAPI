using FluentValidation;
using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Repositories;
using PastPaperRepository.Application.Validators;

namespace PastPaperRepository.Application.Services;

public class PastPaperService : IPastPaperService
{
    private readonly GetAllPastPapersOptionValidator _getAllPastPapersOptionValidator;
    private readonly IPastPaperRepository _pastPaperRepository;
    private readonly IValidator<PastPapers> _pastPaperValidators;

    public PastPaperService(IPastPaperRepository pastPaperRepository, IValidator<PastPapers> pastPaperValidators,
        GetAllPastPapersOptionValidator getAllPastPapersOptionValidator)
    {
        _pastPaperRepository = pastPaperRepository;
        _pastPaperValidators = pastPaperValidators;
        _getAllPastPapersOptionValidator = getAllPastPapersOptionValidator;
    }

    public async Task<bool> CreatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default)
    {
        await _pastPaperValidators.ValidateAndThrowAsync(pastPapers, token);
        return await _pastPaperRepository.CreatePastPaperAsync(pastPapers, token);
    }

    public async Task<IEnumerable<QuestionAnswer>> GetPastPaperByIdAsync(string pastPaperId, string? userId = default,
        CancellationToken token = default)
    {
        return await _pastPaperRepository.GetPastPaperByIdAsync(pastPaperId, token);
    }

    public Task<PastPapers?> GetPastPaperBySlugAsync(string slug, CancellationToken token = default)
    {
        return _pastPaperRepository.GetPastPaperBySlugAsync(slug, token);
    }

    public async Task<IEnumerable<PastPapers>> GetAllPastPapersAsync(GetAllPastPapersOptions options,
        CancellationToken token = default)
    {
        await _getAllPastPapersOptionValidator.ValidateAndThrowAsync(options, token);

        return await _pastPaperRepository.GetAllPastPapersAsync(options, token);
    }

    public async Task<PastPapers?> UpdatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default)
    {
        await _pastPaperValidators.ValidateAndThrowAsync(pastPapers, token);
        var pastPaperExists = await _pastPaperRepository.ExistsById(pastPapers.PastPaperId);
        if (!pastPaperExists) return null;

        await _pastPaperRepository.UpdatePastPaperAsync(pastPapers, token);
        return pastPapers;
    }

    public async Task<bool> DeletePastPaperAsync(string pastPaperId, CancellationToken token = default)
    {
        return await _pastPaperRepository.DeletePastPaperAsync(pastPaperId, token);
    }

    public Task<int> GetCountAsync(string? title, int? year, CancellationToken token = default)
    {
        return _pastPaperRepository.GetCountAsync(title, year, token);
    }

    public Task<IEnumerable<DynamicPastPaperModal>> GetDynamicPastPapersAsync(int id, CancellationToken token = default)
    {
        return _pastPaperRepository.GetDynamicPastPapersAsync(id, token);
    }

    public Task<IEnumerable<SearchPastPaper>> SearchPastPapersAsync(string title, CancellationToken token = default)
    {
        return _pastPaperRepository.SearchPastPapersAsync(title, token);
    }
}