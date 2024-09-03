using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoSql.Services
{
    class UserTableExtractor
    {
        public (string Schema, string TypeTableName)? Extract(string block)
        {
            var match = Regex.Match(block, @"(?i)\bCREATE\s+TYPE\s+TABLE\s+(?<Schema>\w+)\.(?<TypeTableName>\w+)\b");
            if (match.Success)
            {
                var schema = match.Groups["Schema"].Value;
                var typeTableName = match.Groups["TypeTableName"].Value;
                return (schema, typeTableName);
            }
            return null;
        }
    }
}
