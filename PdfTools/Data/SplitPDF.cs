using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTools.Data
{
    internal class SplitPDF
    {
        public string Name { get; set; }
        public string Order { get; set; }
        public int Page { get; set; }
        public int Level { get; set; }
    }
}
