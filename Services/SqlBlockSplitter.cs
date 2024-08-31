using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoSql.Services
{
    public class SqlBlockSplitter
    {
        public IEnumerable<string> SplitSqlContent(string sqlContent)
        {
            return sqlContent.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(b => b.Trim())
                             .Where(b => !string.IsNullOrEmpty(b));
        }
    }
}