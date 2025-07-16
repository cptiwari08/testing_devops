using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EY.CE.Copilot.API.Controllers
{
    [Authorize(Policy = AuthenticationPolicy.CE_USER_OR_APISECRET)]
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        ISearchClient _searchClient;
        public SearchController(ISearchClient searchClient)
        {
            _searchClient = searchClient;
        }
        [HttpPost("SimilaritySearch")]
        public async Task<IActionResult> GetSimilarValues(QuerySearchInput input)
        {
            var searchResult = await _searchClient.PerformSimilaritySearch(input);
            return Ok(searchResult);
        }
        
    }
}
