using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Mapper;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using EY.CE.Copilot.Data.Contexts;
using EY.CE.Copilot.Data.Models;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;

namespace EY.CE.Copilot.API.Clients
{
    public class PromptClient : BaseClass, IPromptClient
    {
        private readonly CopilotContext _context;
        private readonly object? userId;
        public PromptClient(CopilotContext context, IHttpContextAccessor httpContextAccessor, IAppLoggerService logger) : base(logger, nameof(PromptClient))
        {
            _context = context;
            httpContextAccessor.HttpContext?.Items.TryGetValue(Constants.UserMail, out userId);
        }

        public async Task<List<AssistantPrompt>> GetAll()
        {
            try
            {
                Log(AppLogLevel.Trace, "Getting all Assistant prompts", nameof(GetAll));
                List<AssistantPrompt> dbPrompts;
                dbPrompts = _context.AssistantPrompts.ToList();
                return dbPrompts;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method GetAll - {e.Message}", nameof(GetAll), exception: e);
                throw;
            }
        }

        public async Task<List<AssistantPrompt>> GetByQuery(string key, string agent)
        {
            try
            {
                Log(AppLogLevel.Trace, $"Getting all Assistant prompts based on input {key} and {agent}", nameof(GetByQuery));
                List<AssistantPrompt> dbPrompts;
                if(!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(agent)) 
                    dbPrompts = _context.AssistantPrompts.Where(prompt => prompt.Key == key && prompt.Agent == agent).ToList();
                else if (string.IsNullOrEmpty(key)) 
                    dbPrompts = _context.AssistantPrompts.Where(prompt => prompt.Agent == agent).ToList();
                else if(string.IsNullOrEmpty(agent))
                    dbPrompts = _context.AssistantPrompts.Where(prompt => prompt.Key == key).ToList();
                else 
                    dbPrompts = new List<AssistantPrompt>() { };
                return dbPrompts;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method GetByQuery - {e.Message}", nameof(GetByQuery), exception: e);
                throw;
            }
        }

        public async Task Add(List<PromptInsert> input)
        {
            try
            {
                foreach (var item in input)
                {
                    var prompt = PromptMapper.CreateInsertModelForUser(item, userId.ToString());
                    _context.AssistantPrompts.Add(prompt);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Assistant Prompts | Unable to Add Assistant Prompts | {e.Message}", nameof(Add), exception: e);
                throw;
            }
        }

        public async Task Update(List<PromptUpdate> input)
        {
            try
            {
                List<int?> promptIds = input.Select(s => s.ID).ToList();
                var dbPrompts = _context.AssistantPrompts.Where(s => promptIds.Contains(s.ID)).ToList();
                PromptMapper.CreateUpdateModelForUser(input, dbPrompts, userId.ToString());
                _context.AssistantPrompts.UpdateRange(dbPrompts);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Assistant Prompts | Unable to Update Assistant Prompts", nameof(Update), exception: e);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var prompt = _context.AssistantPrompts.Find(id);
                if (prompt != null)
                {
                    _context.AssistantPrompts.Remove(prompt);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Assistant Prompts | Unable to Delete {id} | {e.Message}", nameof(Delete), exception: e);
                throw;
            }
        }
    }
}
