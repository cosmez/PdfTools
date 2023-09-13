using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace PdfTools.IO
{
    internal class Utils
    {
        public static async Task<List<string>> GetProjectDocuments(string opsplannum)
        {
            using var http = new HttpClient();
            List<string> documents = new();
            using (var response = await http.GetAsync($"http://onlineplanservice.com/ost_dynamic_xml/{opsplannum}.ops"))
            {
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return documents;
                    }

                    using var reader = XmlReader.Create(await response.Content.ReadAsStreamAsync(), new XmlReaderSettings()
                    {
                        Async = true
                    });
                    if (reader.ReadToFollowing("ID"))
                        await reader.ReadAsync();//this moves reader to next node which is text 

                    if (reader.ReadToFollowing("Name"))
                        await reader.ReadAsync();//this moves reader to next node which is text 

                    if (reader.ReadToFollowing("Folder"))
                    {
                        await GetProjectDocumentsRecursive(reader, documents);
                    }
                }
            }


            return documents;
        }

        public static async Task GetProjectDocumentsRecursive(XmlReader reader, List<string> documents)
        {
            while (await reader.ReadAsync())
            {
                if (reader is { Name: "Folder", NodeType: XmlNodeType.EndElement })
                {
                    return;
                }

                if (reader is { Name: "Folder", NodeType: XmlNodeType.Element })
                    await GetProjectDocumentsRecursive(reader, documents);

                if (reader is { Name: "File", NodeType: XmlNodeType.Element })
                {
                    //string name = reader.GetAttribute("Name");
                    //string strWidth = reader.GetAttribute("width");
                    //string strHeight = reader.GetAttribute("height");
                    //string strPages = reader.GetAttribute("Pages");
                    //string strSize = reader.GetAttribute("Size");
                    //string strSqft = reader.GetAttribute("sqft");
                    await reader.ReadAsync();
                    //string url = reader.Value.Replace("http://", "https://");
                    string url = reader.Value;
                    documents.Add(url);

                    //string widthHeightStr = string.Empty;
                    //string documentType = url.Substring(url.LastIndexOf(".") + 1, (url.Length - (url.LastIndexOf(".") + 1))).ToUpper();
                    ////string documentIcon = url.ToUpper().EndsWith(".PDF") ? "pdf.gif" : "tiff.gif";
                    //string documentIcon = $"{url.Substring(url.LastIndexOf(".") + 1, (url.Length - (url.LastIndexOf(".") + 1))).ToLower()}.gif";
                    //if (url.ToUpper().EndsWith(".TIF") || url.ToUpper().EndsWith(".TIFF") ||
                    //        url.ToUpper().EndsWith(".JPG") || url.ToUpper().EndsWith(".JPEG"))
                    //{
                    //    widthHeightStr = String.Format(" ({0}X{1})", strWidth, strHeight);
                    //}

                    //if (float.TryParse(strSize, out float size))
                    //    fileNode.Data.Size = size;
                    //
                    //if (float.TryParse(strSqft, out float sqft))
                    //    fileNode.Data.Sqft = sqft;
                    //
                    //if (int.TryParse(strPages, out int pages))
                    //    fileNode.Data.Pages = pages;
                    //
                    //if (float.TryParse(strWidth, out float width))
                    //    fileNode.Data.Width = width;
                    //
                    //if (float.TryParse(strHeight, out float height))
                    //    fileNode.Data.Height = height;
                    //
                    //
                    //fileNode.Data.LastUpdated = fileNode.Data.DateCreated;
                    //fileNode.Data.Url = url;
                    //
                }
            }
        }


        public static string FindProgramFolderPath(string programName)
        {
            string[] commonFolders = {
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86)
        };

            foreach (string folder in commonFolders)
            {
                string programFolderPath = Path.Combine(folder, programName);

                if (Directory.Exists(programFolderPath))
                {
                    return programFolderPath;
                }
            }

            return null;
        }


        static readonly string Invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()) + ":(){}%";
        public static string EscapeFilename(string name)
        {
            foreach (char c in Invalid)
            {
                name = name.Replace(c.ToString(), "");
            }

            //remove web codes
            name = name.Replace("&amp;", "&");
            name = Regex.Replace(name, @"&.+;", "");




            return name;
        }
    }
}
