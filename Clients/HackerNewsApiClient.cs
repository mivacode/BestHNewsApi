using BestHNewsApi.DTOs;
using Microsoft.Extensions.Options;

namespace BestHNewsApi.Clients
{

    public class HackerNewsApiClient : IHackerNewsApiClient
    {
        private readonly HackerNewsApiClientOptions _clientOptions;
        private HttpClient _client;

        public HackerNewsApiClient(IOptions<HackerNewsApiClientOptions> clientOptions)
        {
            _client = new HttpClient();
            _clientOptions = clientOptions.Value;
        }

        public async Task<IEnumerable<long>> GetBestStories()
        {
            var bestStories = await _client.GetFromJsonAsync<long[]>($"{_clientOptions.BaseUrl}/v0/beststories.json");
            return bestStories ?? Enumerable.Empty<long>();
        }

        public async Task<HackerNewsStory> GetStoryDetailsById(long id)
        {
            var story = await _client.GetFromJsonAsync<HackerNewsStory>($"{_clientOptions.BaseUrl}/v0/item/{id}.json");
            return story;
        }
    }
}
