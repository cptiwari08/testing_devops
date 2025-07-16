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
    public class GlossaryController : ControllerBase
    {
        private readonly IGlossaryClient _client;

        public GlossaryController(IGlossaryClient client)
        {
            _client = client;
        }

        [Authorize(Policy = AuthenticationPolicy.CE_USER_OR_APISECRET)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var glossaries = await _client.GetAll();
                return Ok(glossaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<GlossaryInsert> glossary)
        {
            try
            {
                await _client.Add(glossary);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] List<GlossaryUpdate> glossaries)
        {
            try
            {
                if (glossaries == null || glossaries.Count==0) {
                    return BadRequest("Request payload cannot be empty");
                }else if(glossaries.Any(glossary => glossary.ID == null))
                {
                    return BadRequest("ID must be specified in every item in the list");
                }
                await _client.Update(glossaries);
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
