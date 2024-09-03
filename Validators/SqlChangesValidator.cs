using System.Linq;
using AutoSql.Consts;
using AutoSql.Interfaces;
using AutoSql.Services;

namespace AutoSql.Validators
{
    public class SqlChangesValidator : IValidator
    {
        private readonly GitService _gitService;

        public SqlChangesValidator(GitService gitService)
        {
            _gitService = gitService;
        }
         
        public bool Validate(string input, string repoPath, out string errorMessage)
        {
            errorMessage = string.Empty;

            var changedFiles = _gitService.GetChangedFiles(repoPath);

            if (changedFiles.Any(file => file.EndsWith(".sql") || file.EndsWith(".cs")))
            {
                return true;
            }

            errorMessage = ErrorMessages.NoValidFilesWasDecteced;
            return false;
        }
    }
}