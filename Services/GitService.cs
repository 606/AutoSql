using AutoSql.Consts;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AutoSql.Services
{
    public class GitService
    {
        public bool IsGitRepository(string repoPath)
        {
            return Directory.Exists(Path.Combine(repoPath, ".git"));
        }

        public string GetCurrentCommit(string repoPath)
        {
            // Метод для отримання поточного комміту
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = $"-C \"{repoPath}\" rev-parse HEAD",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string commitHash = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();
            return commitHash;
        }

        public string[] GetChangedFiles(string repoPath)
        {
            // Метод для отримання списку змінених файлів
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = $"-C \"{repoPath}\" diff --name-only HEAD~1 HEAD",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }

        // Реалізація GetCommitType
        public string GetCommitType(string commitHash, string repoPath)
        {
            // Отримання списку гілок, до яких належить комміт
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = $"-C \"{repoPath}\" branch --contains {commitHash}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            var branches = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(branch => branch.Trim());

            // Аналізуємо гілки для визначення типу
            if (branches.Any(branch => branch.Contains(CommitTypes.Feature)))
                return CommitTypes.Feature;
            if (branches.Any(branch => branch.Contains(CommitTypes.Bugfix)))
                return CommitTypes.Bugfix;
            if (branches.Any(branch => branch.Contains(CommitTypes.Documentation)))
                return CommitTypes.Documentation;

            // Якщо не знайшли відповідний тип, повертаємо стандартний тип
            return CommitTypes.Commit;
        }
    }
}