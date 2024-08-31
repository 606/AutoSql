using System;
using System.IO;

namespace AutoSql.Helpers
{
    public class ExportFileHelper
    {
        public string GetFilePath(string outputPath)
        {
            string currentUserName = Environment.UserName;
            string fileNameFormat = "updateFile.{1}.{0:yyyy.MM.dd.HH.mm.ss}.sql";
            string fileNameFormatted = string.Format(fileNameFormat, DateTime.Now, currentUserName);
            var scriptFilePath = Path.Combine(outputPath, fileNameFormatted);

            return scriptFilePath;
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
    }
}