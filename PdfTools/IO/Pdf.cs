using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfTools.Data;
using iTextSharp.text.pdf.parser;

namespace PdfTools.IO;
public class Pdf
{

    public static PdfDump GetPdfDump(string filename, bool readContents = false, IProgress<PdfDumpProgress> progress = null)
    {
        var dump = new PdfDump();
        using PdfReader reader = new PdfReader(filename);
        dump.Filename = filename;
        dump.NumberOfPages = reader.NumberOfPages;
        dump.Text = new ();
        dump.Annotations = new();
        dump.Bookmarks = new();
        GetBookmarks(reader, 0,dump.Bookmarks);
        for (int page = 1; page <= reader.NumberOfPages; page++)
        {
            if (readContents)
            {
                dump.Text.Add(GetText(reader, page));
                dump.Annotations.Add(GetAnnotations(reader, page));
            }
            else
            {
                dump.Text.Add("");
                dump.Annotations.Add(new List<string>());
            }

            dump.Size = reader.GetPageSize(page);
            dump.Rotation = reader.GetPageRotation(page);

            if (progress != null)
                progress.Report(new PdfDumpProgress()
                {
                    Page = page,
                    Filename = filename,
                    Total = reader.NumberOfPages
                });
        }


        return dump;
    }

    public static List<PDfBookmark> GetBookmarks(string filename)
    {
        var bookmarks = new List<PDfBookmark>();
        using PdfReader reader = new PdfReader(filename);
        GetBookmarks(reader,0, bookmarks);
        return bookmarks;
    }

    public static void GetBookmarks(PdfReader reader, int level, List<PDfBookmark> bookmarks)
    {
        PdfDictionary catalog = reader.Catalog;
        PdfDictionary outlines = catalog.GetAsDict(PdfName.OUTLINES);
        if (outlines != null)
        {
            PdfArray firstLevel = outlines.GetAsArray(PdfName.FIRST);
            if (firstLevel != null)
            {
                for (int i = 0; i < firstLevel.Size; i += 2)
                {
                    PdfDictionary current = firstLevel.GetAsDict(i);
                    string title = current.GetAsString(PdfName.TITLE).ToString();
                    int page = current.GetAsNumber(PdfName.P).IntValue;

                    var bookmark = new PDfBookmark()
                    {
                        Level = level,
                        Name = title,
                        PageNumber = page
                    };
                    bookmarks.Add(bookmark);
                    Debug.WriteLine($"[{level}] {title} - Page {page}");

                    PdfDictionary next = current.GetAsDict(PdfName.FIRST);
                    if (next != null)
                    {
                        GetBookmarks(reader, level++, bookmarks);
                    }
                }
            }
        }
    }

    public static string GetText(string filename, int pageNumber)
    {
        using PdfReader reader = new PdfReader(filename);
        return GetText(reader, pageNumber);
    }

    public static string GetText(PdfReader reader, int pageNumber)
    {
        if (pageNumber <= reader.NumberOfPages)
        {
            string pageText = PdfTextExtractor.GetTextFromPage(reader, pageNumber);
            return pageText;
        }

        return null;
    }

    public static List<string> GetAnnotations(string filename, int pageNumber)
    {
        using PdfReader reader = new PdfReader(filename);
        return GetAnnotations(reader, pageNumber);
    }

    public static List<string> GetAnnotations(PdfReader reader, int pageNumber)
    {
        var annotations = new List<string>();
        if (pageNumber <= reader.NumberOfPages)
        {
            PdfDictionary pageDictionary = reader.GetPageN(pageNumber);
            PdfArray annotationsArray = pageDictionary.GetAsArray(PdfName.ANNOTS);

            if (annotationsArray != null)
            {
                foreach (PdfObject annotationObj in annotationsArray.ArrayList)
                {
                    if (annotationObj is PdfDictionary annotationDict)
                    {
                        PdfString annotationText = annotationDict.GetAsString(PdfName.CONTENTS);
                        if (annotationText != null)
                        {
                            annotations.Add(annotationText.ToString());
                        }
                    }
                }
            }
        }

        return annotations;
    }

   
}
