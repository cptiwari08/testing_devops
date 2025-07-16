using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    public class ChatHistory
    {
        public int MessageId { get; set; }
        public required string Role { get; set; }
        public required string Content { get; set; }
    }
}
