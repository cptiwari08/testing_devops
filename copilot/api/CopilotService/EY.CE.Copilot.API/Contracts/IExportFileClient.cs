using Microsoft.AspNetCore.Mvc;

namespace EY.CE.Copilot.API.Contracts
{
    public interface IExportFileClient
    {
        Task<FileContentResult> GetExportFile(string content, string fileType);
    }
}