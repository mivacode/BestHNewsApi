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

        public IAsyncEnumerable<long> GetBestStories()
        {
            return HttpClient.GetFromJsonAsAsyncEnumerable<long>("v0/beststories.json");
        }

        public async Task<HackerNewsStory?> GetStoryDetailsById(long id)
        {
            return await HttpClient.GetFromJsonAsync<HackerNewsStory>($"v0/item/{id}.json");
        }
    }
}
