﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTools.Data;
public class PDfBookmark
{
    public string Name { get; set; }
    public int PageNumber { get; set; }
    public string Action { get; set; }
    public string PageCommand { get; set; }
}