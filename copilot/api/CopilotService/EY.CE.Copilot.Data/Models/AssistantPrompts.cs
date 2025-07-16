using EY.CE.Copilot.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EY.CE.Copilot.Data.Models
{
    [Table("AssistantPrompts")]
    public class AssistantPrompt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID {  get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Type { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Key { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string? Prompt { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string OriginalPrompt { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string? Description { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Agent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string CreatedBy { get; set; } = "System";
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string UpdatedBy { get; set; } = "System";
    }

    public static class AssistantPromptExtension
    {
        public static void ApplyConfiguration_AssistantPrompt(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssistantPrompt>()
               .Property(b => b.IsActive)
               .HasDefaultValue(true);
            modelBuilder.Entity<AssistantPrompt>()
                .HasIndex(b => b.Key)
                .IsUnique();
            modelBuilder.ApplyConfiguration(new AssistantPromptConfiguration());
        }
    }
}
