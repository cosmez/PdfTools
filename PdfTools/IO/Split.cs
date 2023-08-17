using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfTools.Data;
using iTextSharp.text;

namespace PdfTools.IO;
public class Split
{
    public static void SplitPdf(string filename, string outputFolderPath, Dictionary<int, string> names, 
        int startingNumber = 1,
        IProgress<SplitProgress> progress = null)
    {
        int currentBiggestOrder = startingNumber;
        //first we get the current last number in the directory
        foreach (var file in Directory.EnumerateFiles(outputFolderPath))
        {
            //this file was jus generated
            string name = Path.GetFileName(file);
            if (name.Contains("^"))
            {

                var parts = name.Split('^');
                var number = parts[0];
                if (int.TryParse(number, out int order))
                {
                    if (order > currentBiggestOrder) currentBiggestOrder = order;
                }
            }
        }

        string escapedFilename = IO.Paths.RemoveInvalidChars(Path.GetFileNameWithoutExtension(filename));

        using PdfReader reader = new PdfReader(filename);
        int pageCount = reader.NumberOfPages;

        

        for (int page = 1; page <= pageCount; page++)
        {
            if (names.ContainsKey(page))
                escapedFilename = IO.Paths.RemoveInvalidChars($"{page:D3}^{names[page]}");

            Document document = new Document(reader.GetPageSizeWithRotation(page));
            string outputPath = Path.Combine(outputFolderPath, $"{escapedFilename}.pdf");
            while (File.Exists(outputPath))
            {
                outputPath = Path.Combine(outputFolderPath, $"{escapedFilename}_{Paths.GenerateRandomName(3)}.pdf");
            }
            PdfCopy writer = new PdfCopy(document, new FileStream(outputPath, FileMode.Create));

            document.Open();
            PdfImportedPage importedPage = writer.GetImportedPage(reader, page);
            writer.AddPage(importedPage);
            document.Close();
            writer.Close();

            if (progress is not null)
            {
                progress.Report(new SplitProgress()
                {
                    Filename = filename,
                    Page = page,
                    NewFilename = outputPath,
                    Total = reader.NumberOfPages
                });
            }
            
        }
    }
}
