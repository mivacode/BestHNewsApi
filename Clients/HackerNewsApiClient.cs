using BestHNewsApi.DTOs;

namespace BestHNewsApi.Clients
{

    public class HackerNewsApiClient : IHackerNewsApiClient
    {
        public const string HttpClientFactory = "HackerNewsAPI";
        private readonly IHttpClientFactory _httpClientFactory;

        public HackerNewsApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected HttpClient HttpClient => _httpClientFactory.CreateClient(HttpClientFactory);

        public async Task<IEnumerable<long>?> GetBestStories()
        {
            var bestStories = await HttpClient.GetFromJsonAsync<long[]>("v0/beststories.json");
            return bestStories;
        }

        public async Task<HackerNewsStory?> GetStoryDetailsById(long id)
        {
            var story = await HttpClient.GetFromJsonAsync<HackerNewsStory>($"v0/item/{id}.json");
            return story;
        }
    }
}
