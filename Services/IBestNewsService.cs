using BestHNewsApi.Models;

namespace BestHNewsApi.Services
{
    public interface IBestNewsService
    {
        IAsyncEnumerable<BestNewsStory> GetBestNewsAsync(int maxStories);
    }
}