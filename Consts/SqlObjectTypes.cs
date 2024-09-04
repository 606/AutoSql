﻿using System.Collections.Generic;

namespace AutoSql.Consts
{
    public enum SqlObjectTypes
    {
        Unknown,
        StoredProcedure,
        UserTable,
        View,
        Trigger,
        AggregateFunction,
        CheckConstraint,
        ClrScalarFunction,
        ClrStoredProcedure,
        ClrTableValuedFunction,
        ClrTrigger,
        DefaultConstraint,
        EdgeConstraint,
        ExtendedStoredProcedure,
        ForeignKeyConstraint,
        InternalTable,
        PlanGuide,
        PrimaryKeyConstraint,
        ReplicationFilterProcedure,
        Rule,
        SequenceObject,
        ServiceQueue,
        SqlInlineTableValuedFunction,
        SqlScalarFunction,
        SqlStoredProcedure,
        SqlTableValuedFunction,
        SqlTrigger,
        Synonym,
        SystemTable,
        TypeTable,
        UniqueConstraint,
        UserDefinedFunction
    }

    public static class SqlAllowedObjectTypes
    {
        public static readonly HashSet<SqlObjectTypes> AllowedTypes = new HashSet<SqlObjectTypes>
        {
            SqlObjectTypes.SqlStoredProcedure
        };
    }
}