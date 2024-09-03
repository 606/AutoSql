using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AutoSql.Services
{
    public class SqlContentExtractor
    {
        public (string Schema, string TableName)? ExtractTableDetails(string block)
        {
            var match = Regex.Match(block, @"\bCREATE\s+TABLE\s+(?:\[?(?<Schema>[^\[\].]+)\]?\.)?\[?(?<TableName>[^\[\]]+)\]?", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                var schema = match.Groups["Schema"].Value;
                var tableName = match.Groups["TableName"].Value;
                return (schema, tableName);
            }
            return null;
        }

        public Dictionary<string, (string Type, bool IsNullable)> ExtractTableColumns(string block)
        {
            // Реалізуйте логіку для витягування колонок з блоку SQL
            // Наприклад, використовуючи регулярні вирази або інший метод парсингу
            var columns = new Dictionary<string, (string Type, bool IsNullable)>();

            // Додайте логіку для заповнення словника columns

            return columns;
        }


        public (string Schema, string ProcedureName)? ExtractProcedureDetails(string procedureContent)
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