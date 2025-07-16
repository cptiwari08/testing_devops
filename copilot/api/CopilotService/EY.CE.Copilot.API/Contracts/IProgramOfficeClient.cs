using EY.CE.Copilot.API.Models;

namespace EY.CE.Copilot.API.Contracts
{
    public interface IProgramOfficeClient
    {
        Task<FileStreamData?> GetFileByDocumentId(string documentId, string projectfriendlyId = "");
        Task<HttpResponseMessage> ExecuteQuery(string query);
    }
}
