using AutoSql.Helpers;
using System.IO;
using System.Text;

namespace AutoSql.Services
{
    public class SqlScriptGenerationService
    {
        private readonly GitService _gitService;
        private readonly SqlContentProcessor _sqlContentProcessor;
        private readonly ExportFileHelper _fileHelper;

        public SqlScriptGenerationService(GitService gitService, SqlContentProcessor sqlContentProcessor, ExportFileHelper fileHelper)
        {
            _gitService = gitService;
            _sqlContentProcessor = sqlContentProcessor;
            _fileHelper = fileHelper;
        }

        public void GenerateScript(string repoPath, string outputPath)
        {
            if (!_fileHelper.DirectoryExists(outputPath))
            {
                _fileHelper.CreateDirectory(outputPath);
            }

            var commitHash = _gitService.GetCurrentCommit(repoPath);
            var changedFiles = _gitService.GetChangedFiles(repoPath);
            string scriptFilePath = _fileHelper.GetFilePath(outputPath);

            using (StreamWriter streamWriter = new StreamWriter(scriptFilePath, false, Encoding.UTF8))
            {
                string header = _fileHelper.GenerateFileHeader();
                streamWriter.WriteLine(header);

                foreach (var file in changedFiles)
                {
                    _sqlContentProcessor.ProcessFile(file, repoPath, streamWriter);
                }
            }
        }
    }
}