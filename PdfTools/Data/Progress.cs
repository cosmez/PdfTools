using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTools.Data;
public class SplitProgress
{
    public string Filename { get; set; }
    public int Page { get; set; }
    public string NewFilename { get; set; }
    public int Total { get; set; }
}


public class PdfDumpProgress
{
    public string Filename { get; set; }
    public int Page { get; set; }
    public int Total { get; set; }
}
