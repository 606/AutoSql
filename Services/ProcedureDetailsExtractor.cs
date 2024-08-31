using System.Text.RegularExpressions;

namespace AutoSql.Services
{
    public class ProcedureDetailsExtractor
    {
        public (string Schema, string ProcedureName)? Extract(string procedureContent)
        {
            var match = Regex.Match(procedureContent, @"\bCREATE\s+(PROCEDURE|PROC)\s+(?:\[?(?<Schema>[^\[\].]+)\]?\.)?\[?(?<ProcedureName>[^\[\]]+)\]?", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                var schema = match.Groups["Schema"].Success ? $"[{match.Groups["Schema"].Value}]" : "dbo";
                var procedureName = $"[{match.Groups["ProcedureName"].Value}]";

                return (schema, procedureName);
            }

            return null;
        }
    }
}