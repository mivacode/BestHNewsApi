
using BestHNewsApi.DTOs;

namespace BestHNewsApi.Clients
{
    public interface IHackerNewsApiClient
    {
        Task<IEnumerable<long>?> GetBestStories();

        Task<HackerNewsStory?> GetStoryDetailsById(long id);
    }
}