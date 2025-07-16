using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EY.CE.Copilot.Data.Models
{
    [Table("AssistantGeneratorHistory")]
    public class GeneratorHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string InstanceId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Type { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string AdditionalInfo { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string UserEmail { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? Status { get; set; }
        public int? StatusCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string CreatedBy { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string UpdatedBy { get; set; }
    }
}
