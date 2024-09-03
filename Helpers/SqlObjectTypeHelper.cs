using AutoSql.Consts;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public static class SqlObjectTypeHelper
{
    private static readonly Dictionary<SqlObjectTypes, string> SqlObjectTypePatterns = new Dictionary<SqlObjectTypes, string>
    {
        { SqlObjectTypes.StoredProcedure, @"(?i)\bCREATE\s+(PROCEDURE|PROC)\b" },
        { SqlObjectTypes.UserTable, @"(?i)\bCREATE\s+TABLE\b" },
        { SqlObjectTypes.View, @"(?i)\bCREATE\s+VIEW\b" },
        { SqlObjectTypes.Trigger, @"(?i)\bCREATE\s+TRIGGER\b" },
        { SqlObjectTypes.AggregateFunction, @"(?i)\bCREATE\s+AGGREGATE\s+FUNCTION\b" },
        { SqlObjectTypes.CheckConstraint, @"(?i)\bCREATE\s+CHECK\s+CONSTRAINT\b" },
        { SqlObjectTypes.ClrScalarFunction, @"(?i)\bCREATE\s+CLR\s+SCALAR\s+FUNCTION\b" },
        { SqlObjectTypes.ClrStoredProcedure, @"(?i)\bCREATE\s+CLR\s+STORED\s+PROCEDURE\b" },
        { SqlObjectTypes.ClrTableValuedFunction, @"(?i)\bCREATE\s+CLR\s+TABLE\s+VALUED\s+FUNCTION\b" },
        { SqlObjectTypes.ClrTrigger, @"(?i)\bCREATE\s+CLR\s+TRIGGER\b" },
        { SqlObjectTypes.DefaultConstraint, @"(?i)\bCREATE\s+DEFAULT\s+CONSTRAINT\b" },
        { SqlObjectTypes.EdgeConstraint, @"(?i)\bCREATE\s+EDGE\s+CONSTRAINT\b" },
        { SqlObjectTypes.ExtendedStoredProcedure, @"(?i)\bCREATE\s+EXTENDED\s+STORED\s+PROCEDURE\b" },
        { SqlObjectTypes.ForeignKeyConstraint, @"(?i)\bCREATE\s+FOREIGN\s+KEY\s+CONSTRAINT\b" },
        { SqlObjectTypes.InternalTable, @"(?i)\bCREATE\s+INTERNAL\s+TABLE\b" },
        { SqlObjectTypes.PlanGuide, @"(?i)\bCREATE\s+PLAN\s+GUIDE\b" },
        { SqlObjectTypes.PrimaryKeyConstraint, @"(?i)\bCREATE\s+PRIMARY\s+KEY\s+CONSTRAINT\b" },
        { SqlObjectTypes.ReplicationFilterProcedure, @"(?i)\bCREATE\s+REPLICATION\s+FILTER\s+PROCEDURE\b" },
        { SqlObjectTypes.Rule, @"(?i)\bCREATE\s+RULE\b" },
        { SqlObjectTypes.SequenceObject, @"(?i)\bCREATE\s+SEQUENCE\s+OBJECT\b" },
        { SqlObjectTypes.ServiceQueue, @"(?i)\bCREATE\s+SпERVICE\s+QUEUE\b" },
        { SqlObjectTypes.SqlInlineTableValuedFunction, @"(?i)\bCREATE\s+SQL\s+INLINE\s+TABLE\s+VALUED\s+FUNCTION\b" },
        { SqlObjectTypes.SqlScalarFunction, @"(?i)\bCREATE\s+SQL\s+SCALAR\s+FUNCTION\b" },
        { SqlObjectTypes.SqlStoredProcedure, @"(?i)\bCREATE\s+SQL\s+STORED\s+PROCEDURE\b" },
        { SqlObjectTypes.SqlTableValuedFunction, @"(?i)\bCREATE\s+SQL\s+TABLE\s+VALUED\s+FUNCTION\b" },
        { SqlObjectTypes.SqlTrigger, @"(?i)\bCREATE\s+SQL\s+TRIGGER\b" },
        { SqlObjectTypes.Synonym, @"(?i)\bCREATE\s+SYNONYM\b" },
        { SqlObjectTypes.SystemTable, @"(?i)\bCREATE\s+SYSTEM\s+TABLE\b" },
        { SqlObjectTypes.TypeTable, @"(?i)\bCREATE\s+TYPE\s+TABLE\b" },
        { SqlObjectTypes.UniqueConstraint, @"(?i)\bCREATE\s+UNIQUE\s+CONSTRAINT\b" }
    };

    public static string GetSqlObjectType(string sqlContent)
    {
        if (sqlContent == null)
        {
            throw new ArgumentNullException(nameof(sqlContent));
        }

        foreach (var objectType in SqlObjectTypePatterns)
        {
            if (Regex.IsMatch(sqlContent, objectType.Value, RegexOptions.IgnoreCase))
            {
                return objectType.Key.ToString();
            }
        }
        return null;
    }
}
