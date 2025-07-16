using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Mapper;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using EY.CE.Copilot.Data.Contexts;
using EY.CE.Copilot.Data.Models;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EY.CE.Copilot.API.Clients
{
    public class GlossaryClient : BaseClass, IGlossaryClient
    {
        private readonly CopilotContext _context;
        private readonly object? userId;
        public GlossaryClient(CopilotContext context, IHttpContextAccessor httpContextAccessor, IAppLoggerService logger) : base(logger, nameof(GlossaryClient))
        {
            _context = context;
            httpContextAccessor.HttpContext?.Items.TryGetValue(Constants.UserMail, out userId);
        }

        public async Task<List<Glossary>> GetAll()
        {
            try
            {
                Log(AppLogLevel.Trace, "Getting all Glossaries", nameof(GetAll));
                List<Glossary> dbGlossary;
                dbGlossary = _context.Glossaries.ToList();
                return dbGlossary;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method GetAll - {e.Message}", nameof(GetAll), exception: e);
                throw;
            }
        }

        public async Task Add(List<GlossaryInsert> input)
        {
            try
            {
                foreach (var item in input)
                {
                    var glossary = GlossaryMapper.CreateInsertModelForUser(item, userId.ToString());
                    _context.Glossaries.Add(glossary);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Glossaries | Unable to Add Glossaries | {e.Message}", nameof(Add), exception: e);
                throw;
            }
        }

        public async Task Update(List<GlossaryUpdate> input)
        {
            try
            {
                List<int?> glossaryIds = input.Select(s => s.ID).ToList();
                var dbGlossaries = _context.Glossaries.Where(s => glossaryIds.Contains(s.ID)).ToList();
                GlossaryMapper.CreateUpdateModelForUser(input, dbGlossaries, userId.ToString());
                _context.Glossaries.UpdateRange(dbGlossaries);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Glossaries | Unable to Update Glossaries", nameof(Update), exception: e);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var glossary = _context.Glossaries.Find(id);
                if (glossary != null)
                {
                    _context.Glossaries.Remove(glossary);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Glossaries | Unable to Delete {id} | {e.Message}", nameof(Delete), exception: e);
                throw;
            }
        }
    }
}
