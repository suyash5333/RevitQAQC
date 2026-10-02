using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitQAQC.Shared.Models
{
    public class NamingStandard
    {
        public string Category { get; set; } = "";

        public string Prefix { get; set; } = "";

        public string LevelPrefix { get; set; } = "L";

        public int LevelDigits { get; set; } = 2;

        public int NumberDigits { get; set; } = 3;

        public string Separator { get; set; } = "-";
    }
}
