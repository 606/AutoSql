namespace AutoSql.Consts
{
    public static class ErrorMessages
    {
        public const string CommitNotFound = "Commit not found.";
        public const string SqlChangesMissing = "No SQL changes detected in the commit.";
        public const string InvalidOutputPath = "Invalid output path.";
        public const string InvalidCommitType = "Invalid commit type.";
        public const string InvalidFolderPath = "The specified path is not a valid Git repository.";
        public const string NoValidFilesWasDecteced = "No valid files was detected to generate update-file.";
        public const string FillInAllFields = "Please fill in all fields.";
        public const string ErrorRetrievingChanges = "Error retrieving changes: {0}";
        public const string NoChangesFound = "No changes found in the repository.";
        public const string ErrorWritingToFile = "Error writing to file: {0}";
    }
}