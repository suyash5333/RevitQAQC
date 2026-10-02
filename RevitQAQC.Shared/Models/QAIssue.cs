using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitQAQC.Shared.Models
{
    public class QAIssue
    {
        public long ElementId { get; set; }

        public string Category { get; set; } = "";

        public string CurrentValue { get; set; } = "";

        public string Problem { get; set; } = "";

        public string SuggestedValue { get; set; } = "";

        public string Severity { get; set; } = "Warning";
    }
}

