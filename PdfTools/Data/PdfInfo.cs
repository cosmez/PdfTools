using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTools.Data;

[DefaultProperty("Pages")]
class PdfInfo
{
    [ReadOnly(true)]
    [Description("Number of Pages")]
    public int Pages { get; set; }
    [ReadOnly(true)]
    public string Author { get; set; }
    [ReadOnly(true)]
    public string CreationDate { get; set; }
    [ReadOnly(true)]
    public string Creator { get; set; }
    [ReadOnly(true)]
    public string Keywords { get; set; }
    [ReadOnly(true)]
    public string Producer { get; set; }
    [ReadOnly(true)]
    public string ModifiedDate { get; set; }
    [ReadOnly(true)]
    public string Subject { get; set; }
    [ReadOnly(true)]
    public string Title { get; set; }
    [ReadOnly(true)]
    public decimal Version { get; set; }
    [ReadOnly(true)]
    public bool IsEncrypted { get; set; }
}


[DefaultProperty("Number")]
internal class PdfPageInfo
{
    [ReadOnly(true)]
    public int Number { get; set; }
    [Browsable(false)]
    public string Text { get; set; }
    [ReadOnly(true)]
    public double Height { get; set; }
    [ReadOnly(true)]
    public double Width { get; set; }
    [ReadOnly(true)]
    public int Rotation { get; set; }
    [ReadOnly(true)]
    public int NumberOfImages { get; set; }
    [ReadOnly(true)]
    public string Size { get; set; }
}