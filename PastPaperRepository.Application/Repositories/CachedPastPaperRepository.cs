using Microsoft.Extensions.Caching.Memory;
using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public class CachedPastPaperRepository : IPastPaperRepository
{
    private readonly PastPaperRepository _decorated;
    private readonly IMemoryCache _memoryCache;

    public CachedPastPaperRepository(PastPaperRepository decorated, IMemoryCache memoryCache)
    {
        _decorated = decorated;
        _memoryCache = memoryCache;
    }

    public Task<bool> CreatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<PastPapers?> GetPastPaperByIdAsync(string pastPaperId, CancellationToken token = default)
    {
        string cacheKey = $"PastPaper_{pastPaperId}";
        // return _decorated.GetPastPaperByIdAsync(pastPaperId, token);
        return await _memoryCache.GetOrCreateAsync(cacheKey, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            return _decorated.GetPastPaperByIdAsync(pastPaperId, token);
        });
    }

    public Task<PastPapers?> GetPastPaperBySlugAsync(string slug, CancellationToken token = default)
    {
        return _decorated.GetPastPaperBySlugAsync(slug, token);
    }

    public Task<IEnumerable<PastPapers>> GetAllPastPapersAsync(GetAllPastPapersOptions options, CancellationToken token = default)
    {
        return _decorated.GetAllPastPapersAsync(options, token);
    }

    public Task<bool> UpdatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeletePastPaperAsync(string pastPaperId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsById(string pastPaperId, CancellationToken token = default)
    {
        return _decorated.ExistsById(pastPaperId, token);
    }

    public Task<int> GetCountAsync(string? title, int? year, CancellationToken token = default)
    {
       return _decorated.GetCountAsync(title, year, token);
    }
}