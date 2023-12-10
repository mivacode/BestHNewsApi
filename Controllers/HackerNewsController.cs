using BestHNewsApi.Models;
using BestHNewsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        /// <summary>
        /// Retrieves best stories' details from live Hacker News API
        /// </summary>
        /// <param name="maxStories">Limits the result to maxStories</param>
        /// <returns>Top maxStories, after sorting by score, descending</returns>
        [HttpGet(Name = "GetBestStories")]
        public IAsyncEnumerable<BestNewsStory> GetAsync([Required] int maxStories)
        {
            _logger.LogDebug($"Requesting GetBestStories(maxStories={maxStories})");
            return _bestNewsService.GetBestNewsAsync(maxStories);            
        }
    }
}
