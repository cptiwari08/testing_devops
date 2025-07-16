using EY.CE.Copilot.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EY.CE.Copilot.Data.Configurations
{
    internal class CopilotConfigurationData : IEntityTypeConfiguration<CopilotConfiguration>
    {
        public void Configure(EntityTypeBuilder<CopilotConfiguration> modelBuilder)
        {
            modelBuilder.ToTable("AssistantConfigurations");
            List<CopilotConfiguration> copilotConfigurations = new List<CopilotConfiguration>
            {
                new CopilotConfiguration
                {
                    ID = 1,
                    Title = "Project Context",
                    Key = "PROJECT_CONTEXT",
                    Value = "",
                    IsEnabled = false
                },
                new CopilotConfiguration
                {
                    ID = 2,
                    Title = "Source Configuration",
                    Key = "SOURCE_CONFIGS",
                    Value = @"
                    [
                        {
                            ""key"": ""project-docs"",
                            ""displayName"": ""Project Docs"",
                            ""originalDisplayName"": ""Project Docs"",
                            ""isQuestionTextBoxEnabled"": true,
                            ""description"": ""Reference information from the project documents that you have access to through SharePoint. Pick and choose which documents you want to focus on via the Document Hub."",
                            ""isDefault"": false,
                            ""ordinal"": 1,
                            ""isActive"": true
                        },
                        {
                            ""key"": ""project-data"",
                            ""displayName"": ""Project Data"",
                            ""originalDisplayName"": ""Project Data"",
                            ""isQuestionTextBoxEnabled"": true,
                            ""description"": ""Ask questions about how this project is progressing. Examples of key insights to ask about include identifying existing risks, issues, actions, decisions, or initiatives that have fallen of track."",
                            ""isDefault"": false,
                            ""ordinal"": 2,
                            ""isActive"": true
                        },
                        {
                            ""key"": ""ey-guidance"",
                            ""displayName"": ""EY Guidance"",
                            ""originalDisplayName"": ""EY Guidance"",
                            ""isQuestionTextBoxEnabled"": true,
                            ""description"": ""Ask questions against SaT best practices, Capital Edge help, and EY intellectual property such as workplan blueprints, cost saving initiatives, and normative operating models."",
                            ""isDefault"": true,
                            ""ordinal"": 3,
                            ""isActive"": true
                        },
                        {
                            ""key"": ""internet"",
                            ""displayName"": ""Internet"",
                            ""originalDisplayName"": ""Internet"",
                            ""isQuestionTextBoxEnabled"": true,
                            ""description"": ""Open your search to the entire internet, protected by the wrapper of EY.ai."",
                            ""isDefault"": false,
                            ""ordinal"": 4,
                            ""isActive"": true
                        }
                    ]",
                    IsEnabled = true
                }
            };
            modelBuilder.HasData(copilotConfigurations);
        }
    }
}
