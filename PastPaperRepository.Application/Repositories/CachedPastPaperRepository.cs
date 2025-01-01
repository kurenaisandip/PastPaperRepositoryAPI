using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public class CachedPastPaperRepository : IPastPaperRepository
{
    private readonly IPastPaperRepository _decorated;
    private readonly IDistributedCache _distributedCache;

    public CachedPastPaperRepository(IPastPaperRepository decorated, IDistributedCache distributedCache)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
    }

    public Task<bool> CreatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<PastPapers?> GetPastPaperByIdAsync(string pastPaperId, CancellationToken token = default)
    {
        string cacheKey = $"PastPaper_{pastPaperId}";
      
        string? cachedPastPaper = await _distributedCache.GetStringAsync(cacheKey, token);

        PastPapers? pastPaper;
        if (string.IsNullOrEmpty(cachedPastPaper))
        {
             pastPaper = await _decorated.GetPastPaperByIdAsync(pastPaperId, token);

            if (pastPaper is null)
            {
                return pastPaper;
            }

            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(pastPaper), token);
            return pastPaper;
        }
        
        pastPaper = JsonSerializer.Deserialize<PastPapers>(cachedPastPaper);

        return pastPaper;
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