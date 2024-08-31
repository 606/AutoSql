using AutoSql.Consts;
using AutoSql.Interfaces;
using AutoSql.Services;

namespace AutoSql.Validators
{
    public class RepositoryValidator : IValidator
    {
        private readonly GitService _gitService;

        public RepositoryValidator(GitService gitService)
        {
            _gitService = gitService;
        }

        public bool Validate(string input, string repoPath, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(repoPath) || !_gitService.IsGitRepository(repoPath))
            {
                errorMessage = ErrorMessages.InvalidFolderPath;
                return false;
            }
            errorMessage = null;
            return true;
        }
    }
}