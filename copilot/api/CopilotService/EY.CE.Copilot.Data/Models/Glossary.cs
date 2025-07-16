using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EY.CE.Copilot.Data.Models
{
    [Table("AssistantGlossary")]
    public class Glossary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Context { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string TableName { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Tag { get; set; }

        [Required]
        [Column(TypeName = "bit")]
        public bool IsSchema { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedAt { get; set; } = null;

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedAt { get; set; } = null;

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string CreatedBy { get; set; } = "System";

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string UpdatedBy { get; set; } = "System";
    }
}
