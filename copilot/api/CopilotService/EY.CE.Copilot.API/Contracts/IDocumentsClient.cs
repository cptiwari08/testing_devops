using EY.CE.Copilot.API.Models;
namespace EY.CE.Copilot.API.Contracts
{
    public interface IDocumentsClient
    {
        Task<Stream> GetDocumentFromHelpAssistant(string documentId, string token);
        Task<IEnumerable<FileItem>> GetProjectDocs(DocsRequest docsRequest, bool onbehalfOfUser);
        Task<IEnumerable<FileItem>> GetAssistantDocs(DocsRequest docsRequest, bool onbehalf = false);
       }
}
