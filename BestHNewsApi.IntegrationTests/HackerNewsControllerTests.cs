using BestHNewsApi.DTOs;
using FluentAssertions;
using Microsoft.Playwright;
using System.Text.Json;

namespace BestHNewsApi.IntegrationTests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class HackerNewsControllerTests : PlaywrightTest
    {

        private IAPIRequestContext? Request = null;

        [SetUp]
        public async Task SetUpAPITesting()
        {
            await CreateAPIRequestContext();
        }

        private async Task CreateAPIRequestContext()
        {
            var headers = new Dictionary<string, string>
            {
                { "Accept", "application/json" }
            };

            Request = await Playwright.APIRequest.NewContextAsync(new()
            {
                // All requests we send go to this API endpoint.
                BaseURL = "http://localhost:5120",
                ExtraHTTPHeaders = headers,
            });
        }

        [TearDown]
        public async Task TearDownAPITesting()
        {
            await Request.DisposeAsync();
        }

        [Test]
        public async Task ShouldRetrieveTopNBestStoriesOrderedByScoreDescending()
        {
            var getParams = new Dictionary<string, object>() { { "maxStories", 10 } };
            var bestNewsRequest = await Request.GetAsync("/BestNews", new() { Params = getParams });

            bestNewsRequest.Ok.Should().BeTrue();

            var response = JsonSerializer.Deserialize<IEnumerable<BestNewsStory>>(await bestNewsRequest.TextAsync());

            response.Should().HaveCount(10)
                .And
                .BeInDescendingOrder((a, b) => a.Score.CompareTo(b.Score));
        }
    }
}
