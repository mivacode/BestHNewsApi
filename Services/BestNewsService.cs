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

        public IAsyncEnumerable<BestNewsStory> GetBestNewsAsync(int maxStories)
        {
            //take max maxStories after sorting by score
            var maxBestStories = GetBestStoriesDetails()
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
            return maxBestStories;
        }

        private async IAsyncEnumerable<HackerNewsStory> GetBestStoriesDetails()
        {
            var bestStoriesId = await _hackerNewsApiClient.GetBestStories().ToListAsync(); //get all the IDs in one go as we want to request details for each of them in parallel

            //The Tasks which are created here to make GetStoryDetailsById requests are throttled by Polly's bulkhead policy on HttpClient level, see Program.cs for details
            var bestStories = bestStoriesId.Select(async storyId =>
            {
                //note: by using HackerNewsApiClientCached implementation, responses from Hacker News API are cached locally to limit number of requests to the external API
                return await _hackerNewsApiClient.GetStoryDetailsById(storyId);
            });

            //as tasks run in parallel, wait for all tasks to complete
            await Task.WhenAll(bestStories);

            foreach (var requestTask in bestStories)
            {
                var requestResult = await requestTask; //since tasks are already completed this just gets task's result
                if (requestResult != null)
                {
                    yield return requestResult;
                }
            }
        }
    }
}
