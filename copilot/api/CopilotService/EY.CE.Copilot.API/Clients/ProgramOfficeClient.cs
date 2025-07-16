using Azure.Storage.Blobs;
using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Extensions;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text.Json;
using FileInfo = EY.CE.Copilot.API.Models.FileInfo;

namespace EY.CE.Copilot.API.Clients
{
    /// <summary>
    /// This class is responsible for making calls to the Program Office (CE4) API.
    /// </summary>
    public class ProgramOfficeClient : BaseClass, IProgramOfficeClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _ceAuthToken;
        private readonly string _workspaceName;
        private readonly object POBaseURL;
        public ProgramOfficeClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IAppLoggerService logger) : base(logger, nameof(ProgramOfficeClient))
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _workspaceName = configuration[ConfigMap.WORKSPACE_NAME];
            httpContextAccessor.HttpContext?.Items.TryGetValue(Constants.CustomClaimTypes.POApiURL, out POBaseURL);
            if (httpContextAccessor.HttpContext.Items.TryGetValue(HeaderNames.Authorization, out var authValue) && authValue != null)
                _ceAuthToken = authValue.ToString().Split(' ')[1];
        }

        /// <summary>
        /// Gets the file stream data by document id.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="projectfriendlyId"></param>
        /// <returns></returns>
        public async Task<FileStreamData?> GetFileByDocumentId(string documentId, string projectfriendlyId = "")
        {
            try
            {
                Log(AppLogLevel.Trace, $"Getting File", nameof(GetFileByDocumentId));
                if (string.IsNullOrWhiteSpace(projectfriendlyId)) projectfriendlyId = _workspaceName;
                var endPoint = string.Format(ProgramOfficeEndPoints.GetFileInfoByDocumentId, projectfriendlyId, documentId);
                
                using var httpClient = _httpClientFactory.CreateClient(Constants.ProgramOfficeHttpClient);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, _ceAuthToken);
                var response = await httpClient.GetAsync(_configuration[SharedKeyVault.SSP_BASE_URL] + endPoint);
                response.EnsureSuccessStatusCode();

                var fileInfo = response.ToResponseModel<FileInfo>();
                return await GetFileStreamDataAsync(fileInfo.BlobUrl, projectfriendlyId, fileInfo.FileName);

            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method {nameof(GetFileByDocumentId)} - {e.Message}", nameof(GetFileByDocumentId), exception: e);
                throw;
            }
        }

        private async Task<FileStreamData?> GetFileStreamDataAsync(string url, string projectfriendlyId, string fileName = "")
        {
            if (!string.IsNullOrWhiteSpace(url) && Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                var container = GetContainer(projectfriendlyId);
                var blob = GetBlobFromUrl(container, uri);
                if (blob == null) return null;
                var stream = await blob.OpenReadAsync();
                FileStreamData fileStreamData = new(blob.GetProperties().Value.ContentType, stream, string.IsNullOrWhiteSpace(fileName) ? blob.Name : fileName);
                return fileStreamData;
            }
            else
            {
                return null;
            }

            BlobContainerClient GetContainer(string projectFriendlyId)
            {
                BlobServiceClient blobClient = new(_configuration[SharedKeyVault.AZURE_BLOB_STORAGE_CONNECTION_STRING]);
                BlobContainerClient container = blobClient.GetBlobContainerClient($"{Constants.ProgramOfficeBlobContainerPrefix}{projectFriendlyId}");
                return container;
            }

            static BlobClient? GetBlobFromUrl(BlobContainerClient container, Uri uri)
            {
                string name = new BlobClient(uri).Name;
                BlobClient blob = container.GetBlobClient(name);
                if (!blob.Exists()) return null;
                return blob;
            }
        }

        public async Task<HttpResponseMessage> ExecuteQuery(string query)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient(Constants.ProgramOfficeHttpClient);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, _ceAuthToken);
                if(POBaseURL == null)
                {
                    Log(AppLogLevel.Error, "Program office url not present in claim");
                }
                string endPoint = POBaseURL.ToString()+ProgramOfficeEndPoints.ExecuteQuery;

                var requestBody = new
                {
                    SqlQuery = query
                };

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsJsonAsync(endPoint, requestBody);
                return response;
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, $"Error in method {nameof(ExecuteQuery)} - {ex.Message}", nameof(ExecuteQuery), exception: ex);
                throw;
            }
        }
    }
}