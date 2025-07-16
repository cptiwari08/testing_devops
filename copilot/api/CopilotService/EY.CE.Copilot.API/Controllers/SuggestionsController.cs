using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EY.CE.Copilot.API.Controllers
{
    [Authorize(Policy = AuthenticationPolicy.CE_USER_POLICY)]
    [Route("[controller]")]
    [ApiController]
    public class SuggestionsController : ControllerBase
    {
        private readonly ISuggestionsClient _client;
        private readonly IProgramOfficeClient _programOfficeClient;
        public SuggestionsController(ISuggestionsClient client)
        {
            _client = client;
        }

        [Authorize(Policy = AuthenticationPolicy.CE_USER_OR_APISECRET)]
        [HttpGet]
        public async Task<IActionResult> Get(bool? filterVisibleToAssistant=null)
        {
            try
            {
                var suggestions = await _client.GetAll(filterVisibleToAssistant);
                return Ok(suggestions);
            } catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var suggestion = await _client.Get(id);
                return Ok(suggestion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<SuggestionInsert> suggestions)
        {
            try
            {
                var result = await _client.Add(suggestions);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] List<SuggestionUpdate> suggestion)
        {
            try
            {
                var result = await _client.Update(suggestion);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _client.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetSuggestionsBySources")]
        public async Task<IActionResult> GetSuggestions([FromBody] List<string> sources)
        {
            try
            {
                var suggestions = await _client.GetSuggestionsBySources(sources);
                return Ok(suggestions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //GET: validatesql query
        [HttpPost("ValidateSqlQuery")]
        public async Task<IActionResult> ValidateSqlQuery([FromBody] SuggestionInsert suggestion)
        {
            try
            {
                var result = await _client.ValidateQuerySyntax(suggestion.AnswerSQL);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        

    }

}
