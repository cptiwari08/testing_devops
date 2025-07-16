using EY.CE.Copilot.Data.Configurations;
using EY.CE.Copilot.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EY.CE.Copilot.Data.Contexts
{
    public class CopilotContext : DbContext
    {
        public CopilotContext(DbContextOptions<CopilotContext> options)
            : base(options)
        {
        }

        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<CopilotConfiguration> CopilotConfigurations { get; set; }
        public DbSet<ChatHistory> ChatHistorys { get; set; }
        public DbSet<CopilotFeedback> CopilotFeedbacks { get; set; }
        public DbSet<MessageFeedback> MessageFeedbacks { get; set; }
        public DbSet<Glossary> Glossaries { get; set; }
        public DbSet<ContentGeneratorQuery> ContentGeneratorQueries { get;set; }
        public DbSet<AssistantPrompt> AssistantPrompts { get; set; }
        public DbSet<GeneratorHistory> GeneratorHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SuggestionConfiguration());
            modelBuilder.ApplyConfiguration(new CopilotConfigurationData());
            modelBuilder.ApplyConfiguration(new GlossaryData());
            modelBuilder.ApplyConfiguration_ContentGeneratorQuery();
            modelBuilder.ApplyConfiguration_AssistantPrompt();
        }
    }
}