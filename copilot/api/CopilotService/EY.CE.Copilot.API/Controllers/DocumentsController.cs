using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EY.CE.Copilot.API.Controllers
{
    [Authorize(Policy = AuthenticationPolicy.CE_USER_POLICY)]
    [Route("[controller]")]
    [ApiController]
    public class DocumentsController(IDocumentsClient documentsClient, IProgramOfficeClient programOfficeClient) : Controller
    {
        private readonly IDocumentsClient _documentsClient = documentsClient;
        private readonly IProgramOfficeClient _programOfficeClient = programOfficeClient;

        [HttpGet("{documentId}")]
        public async Task<IActionResult> GetDocument(string documentId, [FromQuery] string source)
        {
            try
            {
                if (source.Equals(Constants.EYGuidanceSource.HelpAssistant))
                {
                    var authorizationHeader = Request.Headers[Constants.Authorization];
                    string token = authorizationHeader.ToString().Replace(Constants.Bearer + " ", "");
                    var response = await _documentsClient.GetDocumentFromHelpAssistant(documentId, token);
                    return Ok(response);
                }
                else if (source.Equals(Constants.EYGuidanceSource.IP))
                {
                    var fileStreamData = await _programOfficeClient.GetFileByDocumentId(documentId, Constants.IpAssetManagerFriendlyId);
                    return fileStreamData == null ? StatusCode(StatusCodes.Status400BadRequest)
                            : File(fileStreamData.Stream, fileStreamData.ContentType, fileStreamData.Name);
                }
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
