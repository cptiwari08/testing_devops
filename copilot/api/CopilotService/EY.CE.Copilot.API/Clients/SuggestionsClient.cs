using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Extensions;
using EY.CE.Copilot.API.Mapper;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using EY.CE.Copilot.Data.Contexts;
using EY.CE.Copilot.Data.Models;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using EY.SAT.CE.CoreServices.DependencyInjection;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace EY.CE.Copilot.API.Clients
{
    public class SuggestionsClient : BaseClass, ISuggestionsClient
    {
        private readonly CopilotContext _context;
        private readonly IPortalClient _portalClient;
        private readonly IProgramOfficeClient _programOfficeClient;
        private readonly object? userId;
        private readonly string authPolicy;
        public SuggestionsClient(CopilotContext context, IPortalClient client, IHttpContextAccessor httpContextAccessor, IAppLoggerService logger, IProgramOfficeClient programOfficeClient) : base(logger, nameof(SuggestionsClient))
        {
            _context = context;
            _portalClient = client;
            httpContextAccessor.HttpContext?.Items.TryGetValue(Constants.UserMail, out userId);
            authPolicy = httpContextAccessor.HttpContext?.User?.Identity?.AuthenticationType;
            _programOfficeClient = programOfficeClient;
        }

        public async Task<List<Suggestion>> GetAll(bool? filterVisibleToAssistant)
        {
            try
            {
                Log(AppLogLevel.Trace, "Getting all suggestions", nameof(GetAll));
                List<Suggestion> dbSuggestions;
                //all suggestions will be returned if filterVisibleToAssistant is null
                if (filterVisibleToAssistant != null)
                {
                    dbSuggestions = _context.Suggestions.Where(s => s.VisibleToAssistant == filterVisibleToAssistant).ToList();
                }
                else {
                    dbSuggestions = _context.Suggestions.ToList();
                }
                if(authPolicy != CEAuthExtension.SecretAuthenticationScheme)
                    dbSuggestions.ReplaceAndFilterTags(_portalClient, userId.ToString());
                return dbSuggestions;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method GetAll - {e.Message}", nameof(GetAll), exception: e);
                throw;
            }
        }

        public async Task<Suggestion> Get(int id)
        {
            try
            {
                var dbSuggestion = await _context.Suggestions.FindAsync(id);
                if(dbSuggestion != null)
                {
                    var suggestions = new List<Suggestion> { dbSuggestion };
                    if (authPolicy != CEAuthExtension.SecretAuthenticationScheme)
                        suggestions.ReplaceAndFilterTags(_portalClient, userId.ToString());
                    return suggestions[0];
                }
                return null;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Suggestions | Get {id} | {e.Message}", nameof(Get), exception: e);
                throw;
            }
        }

        public async Task<List<QueryValidationResult>> Add(List<SuggestionInsert> apiSuggestions)
        {
            try
            {
                List<QueryValidationResult> results = new List<QueryValidationResult>();
                bool newRecordAdded = false;
                foreach (var apiSuggestion in apiSuggestions)
                {
                    var sources = apiSuggestion.Source.Split(',', StringSplitOptions.TrimEntries).ToList();
                    var suggestion = SuggestionMapper.CreateInsertModelForUser(apiSuggestion, userId.ToString());
                    if (sources.Contains(Constants.Chats.ProjectDataSource))
                    {
                        var validationResult = await ValidateQuerySyntax(apiSuggestion.AnswerSQL ?? string.Empty);
                        results.Add(validationResult);
                        if (validationResult.IsValid)
                        {
                            _context.Suggestions.Add(suggestion);
                            newRecordAdded = true;
                        }
                    }
                    else
                    {
                        if (apiSuggestion.AnswerSQL != null)
                        {
                            results.Add(new QueryValidationResult { ErrorString = "SQL Query cannot be there for selected source.", IsValid = false });
                        }
                        else
                        {
                            results.Add(new QueryValidationResult { ErrorString = string.Empty, IsValid = true });
                            newRecordAdded = true;
                            _context.Suggestions.Add(suggestion);
                        }
                    }
                }
                if(newRecordAdded) 
                    await _context.SaveChangesAsync();
                return results;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Suggestions | Unable to Add Suggestions | {e.Message}", nameof(Add), exception: e);
                throw;
            }
        }

        public async Task<List<QueryValidationResult>> Update(List<SuggestionUpdate> apiSuggestions)
        {
            try
            {
                List<QueryValidationResult> results = new List<QueryValidationResult>();
                List<int> validIds = new List<int>();
                foreach (var apiSuggestion in apiSuggestions)
                {
                    if (apiSuggestion.AnswerSQL != null)
                    {
                        QueryValidationResult result = await ValidateQuerySyntax(apiSuggestion.AnswerSQL);
                        results.Add(result);
                        if (result.IsValid)
                            validIds.Add(apiSuggestion.ID);
                    }
                    else
                    {
                        results.Add(new QueryValidationResult { ErrorString = string.Empty, IsValid = true });
                        validIds.Add(apiSuggestion.ID);
                    }
                }
                if (validIds.Count > 0)
                {
                    var dbSuggestions = _context.Suggestions.Where(s => validIds.Contains(s.ID)).ToList();
                    if (dbSuggestions.Count > 0)
                    {
                        SuggestionMapper.CreateUpdateModelForUser(apiSuggestions, dbSuggestions, userId.ToString());
                        _context.Suggestions.UpdateRange(dbSuggestions);
                        await _context.SaveChangesAsync();
                    }
                }
                return results;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Suggestions | Unable to Update suggestions", nameof(Update), exception: e);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var suggestion = _context.Suggestions.Find(id);
                if (suggestion != null)
                {
                    _context.Suggestions.Remove(suggestion);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Suggestions | Unable to Delete {id} | {e.Message}", nameof(Delete), exception: e);
                throw;
            }
        }

        public async Task<List<Suggestion>> GetSuggestionsBySources(List<string> sources)
        {
            try
            {
                var dbSuggestions = _context.Suggestions.Where(s => sources.Contains(s.Source)).ToList();
                if (authPolicy != CEAuthExtension.SecretAuthenticationScheme)
                    dbSuggestions.ReplaceAndFilterTags(_portalClient, userId.ToString());
                return dbSuggestions;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Suggestions | GetSuggestions | {e.Message}", nameof(GetSuggestionsBySources), exception: e);
                throw;
            }
        }

        public async Task<QueryValidationResult> ValidateQuerySyntax(string query)
        {
            TSql150Parser parser = new TSql150Parser(false);
            IList<ParseError> errors;
            bool isValid = false;
            string errorString = "Invalid SQL query. Please ensure that only a SELECT statement is used.";

            using (StringReader reader = new StringReader(query))
            {
                TSqlFragment fragment = parser.Parse(reader, out errors);
                if (errors.Count == 0)
                {
                    if (fragment is TSqlScript script)
                    {
                        foreach (var batch in script.Batches)
                        {
                            foreach (var statement in batch.Statements)
                            {
                                if (statement is SelectStatement)
                                {
                                    isValid = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    errorString = string.Join(", ", errors.Select(e => e.Message));
                }
            }
            if (!isValid) { 
                return new QueryValidationResult { IsValid = isValid, ErrorString = errorString };
            }
            else
            {
                var response = await _programOfficeClient.ExecuteQuery(query);
                var result = new QueryValidationResult
                {
                    IsValid = response.IsSuccessStatusCode,
                    ErrorString = response.IsSuccessStatusCode ? "" : await response.Content.ReadAsStringAsync()
                };
                return result;
            }
        }
    }

    public class QueryValidationResult
    {
        public string ErrorString { get; set; }
        public bool IsValid { get; set; }
    }
}