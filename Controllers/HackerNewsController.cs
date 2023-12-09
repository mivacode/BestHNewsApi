using BestHNewsApi.Models;
using BestHNewsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BestHNewsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BestNewsController : ControllerBase
    {
        private readonly ILogger<BestNewsController> _logger;
        private readonly IBestNewsService _bestNewsService;

        public BestNewsController(ILogger<BestNewsController> logger, IBestNewsService bestNewsService)
        {
            _logger = logger;
            _bestNewsService = bestNewsService;
        }

        [HttpGet(Name = "GetBestStories")]
        public async Task<IEnumerable<BestNewsStory>> GetAsync(int maxStories)
        {
            var stopWatch = new Stopwatch();
            try
            {
                return await _bestNewsService.GetBestNewsAsync(maxStories);
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogDebug($"GetBestStories took {stopWatch.Elapsed}");
            }
        }
    }
}
