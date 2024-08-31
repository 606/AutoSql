namespace AutoSql.Interfaces
{
    public interface IValidator
    {
        bool Validate(string input, string repoPath, out string errorMessage);
    }
}