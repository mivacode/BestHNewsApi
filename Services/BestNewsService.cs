using BestHNewsApi.Clients;
using BestHNewsApi.DTOs;
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
            var bestStories = await GetBestStoriesDetails();

            //take max maxStories after sorting by score
            return bestStories
                .OrderByDescending(_ => _.Score)
                .Take(maxStories)
                .Select(storyDetails => new BestNewsStory()
                {
                    Title = storyDetails.Title,
                    Score = storyDetails.Score,
                    CommentCount = storyDetails.Descendants,
                    PostedBy = storyDetails.By,
                    Uri = storyDetails.Url,
                    Time = storyDetails.Time.ToDateTime()
                });
        }

        private async Task<IEnumerable<HackerNewsStory>> GetBestStoriesDetails()
        {
            var bestStoriesId = (await _hackerNewsApiClient.GetBestStories()) ?? Enumerable.Empty<long>();

            //The Tasks which are created here to make GetStoryDetailsById requests are throttled by Polly's bulkhead policy on HttpClient level, see Program.cs for details
            var bestStories = bestStoriesId.Select(async storyId =>
            {
                //note: by using HackerNewsApiClientCached, responses from Hacker News API are cached locally to limit number of requests to the external API
                return await _hackerNewsApiClient.GetStoryDetailsById(storyId);
            });

            await Task.WhenAll(bestStories);

            return bestStories
                .Select(_ => _.Result)
                .Where(_ => _ != null)
                .Cast<HackerNewsStory>();
        }
    }
}
