using System.Collections.Generic;

namespace AutoSql.Interfaces
{
    public interface IGitService
    {
        bool IsGitRepository(string repoPath);
        string GetCurrentCommit(string repoPath);
        IEnumerable<string> GetChangedFiles(string repoPath);
        string GetCommitType(string commitHash, string repoPath);
    }
}