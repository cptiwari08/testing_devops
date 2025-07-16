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
    public class ContentGeneratorController : ControllerBase
    {
        private readonly IContentGeneratorClient _client;

        public ContentGeneratorController(IContentGeneratorClient client)
        {
            _client = client;
        }

        [HttpGet("data")]
        public async Task<IActionResult> Get(string? app, string? generatorType)
        {
            try
            {
                var data = await _client.GetDataByFilter(app, generatorType);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("start-request")]
        public async Task<IActionResult> StartRequest(AsyncOperationRequest request)
        {
            var instanceId = Guid.NewGuid().ToString();
            await _client.StartRequest(request, instanceId);
            return Ok(new { instanceId });
        }

        [HttpPost("status/{instanceId}")]
        public async Task<IActionResult> GetDataFromRedis(string instanceId, ContentGenerator input)
        {
            try
            {
                var output = await _client.GetDataFromRedis(instanceId, input);
                return Ok(output);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("terminate/{appType}/{instanceid}")]
        public async Task<IActionResult> Terminate(AppType appType, string instanceId)
        {
            try
            {
                var output = await _client.TerminateContentGenerator(appType, instanceId);
                return Ok(output);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
