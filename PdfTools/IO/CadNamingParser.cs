using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PdfTools.IO;

public class ConstructionFileInfo
{
    public string Discipline { get; set; }
    public string SheetNumber { get; set; }
}

public class CadNamingParser
{
    public static ConstructionFileInfo? Parse(string filename)
    {
        // Define the regular expression pattern to match the construction filename
        //string pattern = @"^(?<Discipline>[^_]+)_(?<SheetNumber>[^.]+)_(?<RevisionDate>\d{8})$";
        string pattern = @"(?<Discipline>[A-Za-z]{1,2})(?<SheetNumber>[0-9]{3}-?\d{0,3})";
        // Create a regular expression object
        Regex regex = new Regex(pattern);

        // Match the filename against the pattern
        Match match = regex.Match(filename);

        if (match.Success)
        {
            // Extract the elements from the matched groups
            string discipline = match.Groups["Discipline"].Value;
            string sheetNumber = match.Groups["SheetNumber"].Value;

            // Create a ConstructionFileInfo object to store the parsed information
            ConstructionFileInfo fileInfo = new ConstructionFileInfo
            {
                Discipline = discipline,
                SheetNumber = sheetNumber
            };

            return fileInfo;
        }

        return null;
    }
}
