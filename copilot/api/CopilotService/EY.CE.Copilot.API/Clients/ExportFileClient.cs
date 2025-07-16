using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Static;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using ComponentInfo = GemBox.Document.ComponentInfo;
using PdfSaveOptions = GemBox.Document.PdfSaveOptions;

namespace EY.CE.Copilot.API.Clients
{
    public class ExportFileClient : BaseClass, IExportFileClient
    {
        private readonly IConfiguration _configuration;
        public ExportFileClient(IConfiguration configuration, IAppLoggerService logger) : base(logger, nameof(ExportFileClient))
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a PDF file by replacing placeholders in an HTML template with the provided content.
        /// Converts the HTML content to a PDF stream and returns it as a FileContentResult.
        /// </summary>
        /// <param name="content">The content to be inserted into the HTML template.</param>
        /// <returns>A FileContentResult containing the generated PDF file.</returns>
        public async Task<FileContentResult> GetExportFile(string content, string fileType)
        {
            string? htmlContent = PrepareHtmlDocument(content);
            SetLicense();
            switch (fileType.ToLower())
            {
                case Constants.SupportedExportFileTypes.Pdf:
                    return GeneratePdf(htmlContent);
                case Constants.SupportedExportFileTypes.Word:
                    return GenerateWordFile(htmlContent);
                default:
                    return GeneratePdf(htmlContent);
            }
        }

        /// <summary>
        /// Prepares the HTML document by replacing placeholders with the provided content.
        /// </summary>
        /// <param name="content">The content to be inserted into the HTML template.</param>
        /// <returns>The prepared HTML document.</returns>
        private string? PrepareHtmlDocument(string content)
        {
            // Get the file paths for the HTML template, image directory, and font directory
            var htmlFilePath = Path.Combine(Environment.CurrentDirectory, Constants.Assets, Constants.Templates, Constants.AssistantTemplate);
            var imagePath = Path.Combine(Environment.CurrentDirectory, Constants.Assets, Constants.Images);
            var fontPath = Path.Combine(Environment.CurrentDirectory, Constants.Assets, Constants.Fonts);

            // Ensure that the image and font paths end with a directory separator character
            if (!imagePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                imagePath += Path.DirectorySeparatorChar;
            }

            if (!fontPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                fontPath += Path.DirectorySeparatorChar;
            }

            // Read the HTML template file and replace the placeholders with the provided content and file paths
            var htmlContent = File.ReadAllText(htmlFilePath);
            htmlContent = htmlContent
                                .Replace("{Messages}", content)
                                .Replace("{ImagePath}", imagePath)
                                .Replace("{FontPath}", fontPath);

            Log(AppLogLevel.Information, "Read the html content from template is completed", nameof(PrepareHtmlDocument));

            return htmlContent;
        }

        /// <summary>
        /// Generates a PDF file by converting the HTML content to a PDF stream.
        /// </summary>
        /// <param name="htmlContent">The HTML content to be converted.</param>
        /// <returns>A FileContentResult containing the generated PDF file.</returns>
        private FileContentResult GeneratePdf(string htmlContent)
        {
            // Convert the HTML content to a PDF stream
            MemoryStream pdfStream = ConvertHtmlToPdfStream(htmlContent);
            pdfStream.Position = 0;

            Log(AppLogLevel.Information, "ConvertHtmlToPdfStream is completed", nameof(GeneratePdf));

            // Create a FileContentResult with the PDF stream and set the file name for download
            var result = new FileContentResult(pdfStream.ToArray(), Constants.ContentType.ApplicationPdf);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
            string fileName = $"AssistantHistory_{timestamp}.pdf";
            result.FileDownloadName = fileName;
            pdfStream.Dispose();
            return result;
        }
        
        /// <summary>
        /// Generates a Word file by converting the HTML content to a Word document.
        /// </summary>
        /// <param name="htmlContent">The HTML content to be converted.</param>
        /// <returns>A FileContentResult containing the generated Word file.</returns>
        private FileContentResult GenerateWordFile(string htmlContent)
        {
            // Create a new document.
            var document = new DocumentModel();

            // Load the HTML content into the document.
            document.Content.LoadText(htmlContent, new HtmlLoadOptions());

            // Save the document to a byte array.
            byte[] fileContents;
            using (var stream = new MemoryStream())
            {
                document.Save(stream, SaveOptions.DocxDefault);
                fileContents = stream.ToArray();
            }

            var result = new FileContentResult(fileContents, Constants.ContentType.ApplicationDocx);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
            string fileName = $"AssistantHistory_{timestamp}.docx";
            result.FileDownloadName = fileName;
            return result;
        }

        /// <summary>
        /// Sets the GemBox.Document license.
        /// </summary>
        public void SetLicense()
        {
            try
            {
                string serialKey = _configuration[SharedKeyVault.GEMBOX_DOCUMENT_LICENSE_KEY];

                if (string.IsNullOrWhiteSpace(serialKey))
                {
                    throw new Exception("GemBox.Document license key is missing.");
                }

                ComponentInfo.SetLicense(serialKey);
                Log(AppLogLevel.Information, "SetLicense is done", nameof(GetExportFile));
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, $"Error in SetLicense Method - {ex.Message}", nameof(ExportFileClient), exception: ex);
                throw;
            }
        }

        /// <summary>
        /// Converts the HTML content to a PDF stream using GemBox.Document library.
        /// </summary>
        /// <param name="html">The HTML content to be converted.</param>
        /// <returns>A MemoryStream containing the converted PDF stream.</returns>
        public MemoryStream ConvertHtmlToPdfStream(string html)
        {
            try
            {
                DocumentModel document = DocumentModel.Load(new MemoryStream(Encoding.UTF8.GetBytes(html)), LoadOptions.HtmlDefault);

                foreach (var section in document.Sections)
                {
                    section.PageSetup.PaperType = PaperType.Letter;
                    section.PageSetup.Orientation = Orientation.Portrait;
                }

                var pdfStream = new MemoryStream();
                document.Save(pdfStream, new PdfSaveOptions());
                pdfStream.Position = 0;
                return pdfStream;
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, $"Error in ConvertHtmlToPdfStream - {ex.Message}", nameof(ExportFileClient), exception: ex);
                throw;
            }
        }
    }
}
