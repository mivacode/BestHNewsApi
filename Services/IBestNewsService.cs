using BestHNewsApi.Models;

namespace BestHNewsApi.Services
{
    public interface IBestNewsService
    {
        Task<IEnumerable<BestNewsStory>> GetBestNewsAsync(int maxStories);
    }
}