using AutoSql.Consts;
using AutoSql.Interfaces;
using AutoSql.Services;

namespace AutoSql.Validators
{
    public class CommitValidator : IValidator
    {
        private readonly GitService _gitService;

        public CommitValidator(GitService gitService) => _gitService = gitService;

        public bool Validate(string commitHash, string repoPath, out string errorMessage)
        {
            errorMessage = null;
            string commitType = _gitService.GetCommitType(commitHash, repoPath);

            if (commitType != "commit" && commitType != "feature")
            { 
                errorMessage = ErrorMessages.InvalidCommitType;
                return false;
            }

            return true;
        }
    }
}