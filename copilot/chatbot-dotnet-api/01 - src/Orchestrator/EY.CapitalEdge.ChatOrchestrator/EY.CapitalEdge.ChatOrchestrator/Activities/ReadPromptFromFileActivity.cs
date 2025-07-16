using EY.CapitalEdge.ChatOrchestrator.Models;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EY.CapitalEdge.ChatOrchestrator.Activities
{
    public class ReadPromptFromFileActivity
    {
        private readonly Services.ILoggerProvider _loggerProvider;

        private const string ProjectFriendlyId = "projectFriendlyId";

        public ReadPromptFromFileActivity(Services.ILoggerProvider loggerProvider)
        {
            _loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Read prompt from file.
        /// </summary>
        /// <param name="fileName">The name of the file to read.</param>
        /// <param name="executionContext">The execution context of the function.</param>
        /// <returns>The content of the file.</returns>
        [Function(nameof(ReadPromptFromFileActivity))]
        public async Task<string> Run([ActivityTrigger] ReadPromptFromFileActivityInput input, FunctionContext executionContext)
        {
            ILogger log = _loggerProvider.GetLogger(executionContext, "ReadPromptFromFileActivity");
            ILogger logger = new CustomLogger(log, $"[instanceId:{input.InstanceId}] [chatId:{input.ChatId}] [{ProjectFriendlyId}:{input.ProjectFriendlyId}]");
            logger.LogInformation("Reading prompt from file.");

            string content = string.Empty;
            try
            {
                string promptDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Summaries");
                string filePath = Path.Combine(promptDirectory, input.FileName);
                return await File.ReadAllTextAsync(filePath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error reading prompt from file: {FileName}.", input.FileName);
                return content;
            }
        }
    }
}