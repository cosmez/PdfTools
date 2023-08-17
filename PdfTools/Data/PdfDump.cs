using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;

namespace PdfTools.Data;
public class PdfDump
{
    public string Filename { get; set; }
    public int NumberOfPages { get; set; }
    public List<PDfBookmark> Bookmarks { get; set; }
    public List<string> Text { get; set; }
    public List<List<string>> Annotations { get; set; }
    public Rectangle Size { get; set; }
    public int Rotation { get; set; }
}

