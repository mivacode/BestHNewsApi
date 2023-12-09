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

            //The Tasks which are created here to make GetStoryDetailsById requests are throttled by Polly's bulkhead policy on HttpClient level, see Program.cs for details
            var bestStories = bestStoriesId.Select(async storyId =>
            {
                //note: by using HackerNewsApiClientCached, responses from Hacker News API are cached locally to limit number of requests to the external API
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

            //take max maxStories after sorting by score
            return bestStories
                .Select(_ => _.Result)
                .OrderByDescending(_ => _.Score)
                .Take(maxStories);
        }
    }
}
