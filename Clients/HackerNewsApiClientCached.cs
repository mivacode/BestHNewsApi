using BestHNewsApi.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BestHNewsApi.Clients
{
    public class HackerNewsApiClientCached : IHackerNewsApiClientCached
    {
        private readonly IHackerNewsApiClient _hackerNewsApiClient;
        private readonly IMemoryCache _memoryCache;
        private readonly HackerNewsApiClientOptions _options;

        const string CacheEntry_GetBestStories = "BestStories";
        const string CacheEntry_GetStoryDetailsById = "StoryDetailsById";

        public HackerNewsApiClientCached(IHackerNewsApiClient hackerNewsApiClient, IMemoryCache memoryCache, IOptions<HackerNewsApiClientOptions> options)
        {
            _hackerNewsApiClient = hackerNewsApiClient;
            _memoryCache = memoryCache;
            _options = options.Value;
        }

        public async IAsyncEnumerable<long> GetBestStories()
        {
            var cachedList = await _memoryCache.GetOrCreateAsync(CacheEntry_GetBestStories, async (cacheEntry) =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_options.CacheAbsoluteExpiration ?? 60);
                var bestStoriesResult = await _hackerNewsApiClient.GetBestStories().ToListAsync();
                return bestStoriesResult;
            }) ?? Enumerable.Empty<long>();

            //make this method and async iterator
            foreach (var item in cachedList)
            {
                yield return item;
            }
        }

        public async Task<HackerNewsStory?> GetStoryDetailsById(long id)
        {
            return await _memoryCache.GetOrCreateAsync(CacheEntry_GetStoryDetailsById + id, async (cacheEntry) =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_options.CacheAbsoluteExpiration ?? 60);
                var storyDetails = await _hackerNewsApiClient.GetStoryDetailsById(id);
                return storyDetails;
            });
        }
    }
}
