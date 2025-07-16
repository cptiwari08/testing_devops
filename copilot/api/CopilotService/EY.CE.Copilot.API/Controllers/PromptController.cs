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
    public class PromptController : ControllerBase
    {
        private readonly IPromptClient _client;

        public PromptController(IPromptClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var prompts = await _client.GetAll();
                return Ok(prompts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("GetByQuery")]
        public async Task<IActionResult> GetByQuery(PromptQuery input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.Key) && string.IsNullOrEmpty(input.Agent) ){
                    return BadRequest("Either key or agent must be supplied in input payload");
                }
                var prompts = await _client.GetByQuery(input.Key, input.Agent);
                return Ok(prompts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<PromptInsert> prompt)
        {
            try
            {
                await _client.Add(prompt);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] List<PromptUpdate> prompts)
        {
            try
            {
                if (prompts == null || prompts.Count == 0)
                {
                    return BadRequest("Request payload cannot be empty");
                }
                else if (prompts.Any(prompt => prompt.ID == null))
                {
                    return BadRequest("ID must be specified in every item in the list");
                }
                await _client.Update(prompts);
                return StatusCode(204);
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
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
