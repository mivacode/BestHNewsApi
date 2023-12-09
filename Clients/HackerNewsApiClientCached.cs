using BestHNewsApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

namespace BestHNewsApi.Clients
{
    public class HackerNewsApiClientCached : IHackerNewsApiClientCached
    {
        private readonly IHackerNewsApiClient _hackerNewsApiClient;
        private readonly IMemoryCache _memoryCache;

        const string CacheEntry_GetBestStories = "BestStories";
        const string CacheEntry_GetStoryDetailsById = "StoryDetailsById";

        public HackerNewsApiClientCached(IHackerNewsApiClient hackerNewsApiClient, IMemoryCache memoryCache)

        {
            _hackerNewsApiClient = hackerNewsApiClient;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<long>> GetBestStories()
        {
            return await _memoryCache.GetOrCreateAsync(CacheEntry_GetBestStories, async (cacheEntry) =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
                var bestStoriesResult = await _hackerNewsApiClient.GetBestStories();
                return bestStoriesResult;
            });
        }

        public async Task<HackerNewsStory> GetStoryDetailsById(long id)
        {
            return await _memoryCache.GetOrCreateAsync(CacheEntry_GetStoryDetailsById + id, async (cacheEntry) =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
                var storyDetails = await _hackerNewsApiClient.GetStoryDetailsById(id);
                return storyDetails;
            });
        }
    }
}
