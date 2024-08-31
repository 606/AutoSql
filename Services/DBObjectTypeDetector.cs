using System;
using System.Text.RegularExpressions;
using AutoSql.Consts;

namespace AutoSql.Services
{
    public static class DBObjectTypeDetector
    {
        public static DBObjectType DetermineObjectType(string sqlContent)
        {
            if (string.IsNullOrWhiteSpace(sqlContent))
                return DBObjectType.Unknown;

            // Define regular expressions for different object types
            var procedureRegex = new Regex(@"\bCREATE\s+(PROCEDURE|PROC)\b", RegexOptions.IgnoreCase);
            var tableRegex = new Regex(@"\bCREATE\s+TABLE\b", RegexOptions.IgnoreCase);
            var functionRegex = new Regex(@"\bCREATE\s+(FUNCTION|FN)\b", RegexOptions.IgnoreCase);
            var viewRegex = new Regex(@"\bCREATE\s+VIEW\b", RegexOptions.IgnoreCase);

            if (procedureRegex.IsMatch(sqlContent))
                return DBObjectType.StoredProcedures;
            /*
            if (tableRegex.IsMatch(sqlContent))
                return DBObjectType.Tables;

            if (functionRegex.IsMatch(sqlContent))
                return DBObjectType.Functions;

            if (viewRegex.IsMatch(sqlContent))
                return DBObjectType.Views;

            // Add checks for other object types here
            */
            return DBObjectType.Unknown;
        }
    }
}