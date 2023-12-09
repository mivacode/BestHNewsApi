using BestHNewsApi.Clients;
using BestHNewsApi.Models;

namespace BestHNewsApi.Services
{
    public class BestNewsService : IBestNewsService
    {
        private readonly IHackerNewsApiClient _hackerNewsApiClient;

        public BestNewsService(IHackerNewsApiClientCached hackerNewsApiClient)
        {
            _hackerNewsApiClient = hackerNewsApiClient;
        }

        public async Task<IEnumerable<BestNewsStory>> GetBestNewsAsync(int maxStories)
        {
            var bestStoriesId = await _hackerNewsApiClient.GetBestStories();

            var bestStories = bestStoriesId.Select(async storyId =>
            {
                var storyDetails = await _hackerNewsApiClient.GetStoryDetailsById(storyId);
                return new BestNewsStory()
                {
                    Title = storyDetails.Title,
                    Score = storyDetails.Score,
                    CommentCount = storyDetails.Descendants,
                    PostedBy = storyDetails.By,
                    Uri = storyDetails.Url,
                    Time = storyDetails.Time.ToDateTime()
                };
            });

            await Task.WhenAll(bestStories);

            return bestStories
                .Select(_ => _.Result)
                .OrderByDescending(_ => _.Score)
                .Take(maxStories);
        }
    }
}
