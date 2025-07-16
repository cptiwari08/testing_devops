using EY.CE.Copilot.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace EY.CE.Copilot.Data.Models
{
    [Table("AssistantContentGeneratorQueries")]
    public class ContentGeneratorQuery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string APP { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string GeneratorType { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string Key { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string SQLQuery { get; set; }
        [Required]
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

    public static class ContentGeneratorQueryExtension
    {
        public static void ApplyConfiguration_ContentGeneratorQuery(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContentGeneratorQuery>()
               .Property(b => b.IsActive)
               .HasDefaultValue(true);
            modelBuilder.Entity<ContentGeneratorQuery>()
                .HasIndex(b => b.Key)
                .IsUnique();
            modelBuilder.ApplyConfiguration(new ContentGenerationData());
        }
    }
}
