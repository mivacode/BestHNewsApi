
using BestHNewsApi.DTOs;

namespace BestHNewsApi.Clients
{
    public interface IHackerNewsApiClient
    {
        IAsyncEnumerable<long> GetBestStories();

        Task<HackerNewsStory?> GetStoryDetailsById(long id);
    }
}