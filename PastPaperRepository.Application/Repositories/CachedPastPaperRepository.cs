using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public class CachedPastPaperRepository : IPastPaperRepository
{
    private readonly IPastPaperRepository _decorated;
    private readonly IDistributedCache _distributedCache;
       private readonly IMemoryCache _memoryCache;

    public CachedPastPaperRepository(IPastPaperRepository decorated, IDistributedCache distributedCache, IMemoryCache memoryCache)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
        _memoryCache = memoryCache;
    }

    public Task<bool> CreatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default)
    {
        return _decorated.CreatePastPaperAsync(pastPapers, token);
    }

    public async Task<IEnumerable<QuestionAnswer>?> GetPastPaperByIdAsync(string pastPaperId, CancellationToken token = default)
    {
        var cacheKey = $"PastPaper_{pastPaperId}";
        
        return await _memoryCache.GetOrCreateAsync(cacheKey, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            return _decorated.GetPastPaperByIdAsync(pastPaperId, token);
        });

        // var cachedPastPaper = await _distributedCache.GetStringAsync(cacheKey, token);
        //
        // IEnumerable<QuestionAnswer>? pastPaper;
        // if (string.IsNullOrEmpty(cachedPastPaper))
        // {
        //     pastPaper = await _decorated.GetPastPaperByIdAsync(pastPaperId, token);
        //
        //     if (pastPaper is null) return pastPaper;
        //
        //     await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(pastPaper), token);
        //     return pastPaper;
        // }
        //
        // pastPaper = JsonSerializer.Deserialize<IEnumerable<QuestionAnswer>>(cachedPastPaper);
        //
        // return pastPaper;
    }

    public Task<PastPapers?> GetPastPaperBySlugAsync(string slug, CancellationToken token = default)
    {
        return _decorated.GetPastPaperBySlugAsync(slug, token);
    }

    public Task<IEnumerable<PastPapers>> GetAllPastPapersAsync(GetAllPastPapersOptions options,
        CancellationToken token = default)
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

    public Task<IEnumerable<DynamicPastPaperModal>> GetDynamicPastPapersAsync(int id, CancellationToken token = default)
    {
        return _decorated.GetDynamicPastPapersAsync(id, token);
    }
}