using System.Collections.Generic;
using System.IO;

namespace AutoSql.Helpers
{
    /// <summary>
    /// Validates the existence of a database object (in this case, a stored procedure) before executing the main content.
    /// If the object does not exist, it creates the object before proceeding.
    /// </summary>
    public class SqlExistenceHelper
    {
        /// <summary>
        /// Checks if the specified stored procedure exists in the specified schema. If it does not exist, it creates the procedure.
        /// </summary>
        /// <param name="schemaName">The name of the schema where the procedure is located.</param>
        /// <param name="procedureName">The name of the stored procedure.</param>
        /// <param name="streamWriter">The stream writer used to write SQL commands.</param>
        public void ProcedureExistenceValidator(string schemaName, string procedureName, StreamWriter streamWriter)
        {
            // Check if the procedure exists
            streamWriter.WriteLine($"if object_Id('{schemaName}.{procedureName}', 'P') is null");
            streamWriter.WriteLine("begin");
            // Create the procedure if it does not exist
            streamWriter.WriteLine($"    exec('create procedure {schemaName}.{procedureName} as begin return 0 end')");
            streamWriter.WriteLine("end");
            streamWriter.WriteLine("go");
        }

        public void FunctionExistenceValidator(string schema, string functionName, StreamWriter streamWriter)
        {
            // Приклад перевірки існування функції
            streamWriter.WriteLine($"IF OBJECT_ID('{schema}.{functionName}', 'FN') IS NOT NULL");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Function {schema}.{functionName} exists.';");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("ELSE");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Function {schema}.{functionName} does not exist.';");
            streamWriter.WriteLine("END");
        }

        public void TriggerExistenceValidator(string schema, string triggerName, StreamWriter streamWriter)
        {
            // Приклад перевірки існування тригера
            streamWriter.WriteLine($"IF OBJECT_ID('{schema}.{triggerName}', 'TR') IS NOT NULL");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Trigger {schema}.{triggerName} exists.';");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("ELSE");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Trigger {schema}.{triggerName} does not exist.';");
            streamWriter.WriteLine("END");
        }

        public void ViewExistenceValidator(string schema, string viewName, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"IF OBJECT_ID('{schema}.{viewName}', 'V') IS NOT NULL");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'View {schema}.{viewName} exists.';");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("ELSE");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'View {schema}.{viewName} does not exist.';");
            streamWriter.WriteLine("END");
        }

        public void TableExistenceValidator(string schema, string tableName, string createTableBlock,
            Dictionary<string, (string Type, bool IsNullable)> columns, StreamWriter streamWriter)
        {
            // Перевірка існування таблиці
            streamWriter.WriteLine(
                $"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}' AND schema_id = SCHEMA_ID('{schema}'))");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Table {schema}.{tableName} does not exist.';");
            streamWriter.WriteLine(createTableBlock);
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("GO");

            // Додавання колонок, якщо таблиця існує
            foreach (var column in columns)
            {
                string nullability = column.Value.IsNullable ? "NULL" : "NOT NULL";
                streamWriter.WriteLine(
                    $"IF EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}' AND schema_id = SCHEMA_ID('{schema}'))");
                streamWriter.WriteLine(
                    $"AND NOT EXISTS (SELECT TOP 1 1 FROM sys.columns c JOIN sys.tables t ON t.object_id = c.object_id WHERE t.name = '{tableName}' AND c.name = '{column.Key}')");
                streamWriter.WriteLine("BEGIN");
                streamWriter.WriteLine(
                    $"    ALTER TABLE [{schema}].[{tableName}] ADD [{column.Key}] {column.Value.Type} {nullability};");
                streamWriter.WriteLine("END");
                streamWriter.WriteLine("GO");
            }
        }

        public void ConstraintExistenceValidator(string schema, string constraintName, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"IF OBJECT_ID('{schema}.{constraintName}', 'C') IS NOT NULL");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Constraint {schema}.{constraintName} exists.';");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("ELSE");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Constraint {schema}.{constraintName} does not exist.';");
            streamWriter.WriteLine("END");
        }

        public void AggregateFunctionExistenceValidator(string schema, string functionName, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"IF OBJECT_ID('{schema}.{functionName}', 'AF') IS NOT NULL");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Aggregate Function {schema}.{functionName} exists.';");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("ELSE");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Aggregate Function {schema}.{functionName} does not exist.';");
            streamWriter.WriteLine("END");
        }

        public void PlanGuideExistenceValidator(string schema, string planGuideName, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"IF OBJECT_ID('{schema}.{planGuideName}', 'PG') IS NOT NULL");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Plan Guide {schema}.{planGuideName} exists.';");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("ELSE");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Plan Guide {schema}.{planGuideName} does not exist.';");
            streamWriter.WriteLine("END");
        }

        public void RuleExistenceValidator(string schema, string ruleName, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"IF OBJECT_ID('{schema}.{ruleName}', 'R') IS NOT NULL");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Rule {schema}.{ruleName} exists.';");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("ELSE");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Rule {schema}.{ruleName} does not exist.';");
            streamWriter.WriteLine("END");
        }

        public void SequenceExistenceValidator(string schema, string sequenceName, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"IF OBJECT_ID('{schema}.{sequenceName}', 'SQ') IS NOT NULL");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Sequence {schema}.{sequenceName} exists.';");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("ELSE");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Sequence {schema}.{sequenceName} does not exist.';");
            streamWriter.WriteLine("END");
        }

        public void ServiceQueueExistenceValidator(string schema, string queueName, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"IF OBJECT_ID('{schema}.{queueName}', 'SQ') IS NOT NULL");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Service Queue {schema}.{queueName} exists.';");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("ELSE");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Service Queue {schema}.{queueName} does not exist.';");
            streamWriter.WriteLine("END");
        }

        public void SynonymExistenceValidator(string schema, string synonymName, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"IF OBJECT_ID('{schema}.{synonymName}', 'SN') IS NOT NULL");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Synonym {schema}.{synonymName} exists.';");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("ELSE");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Synonym {schema}.{synonymName} does not exist.';");
            streamWriter.WriteLine("END");
        }

        public void TypeTableExistenceValidator(string schema, string typeTableName, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"IF OBJECT_ID('{schema}.{typeTableName}', 'TT') IS NOT NULL");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Type Table {schema}.{typeTableName} exists.';");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("ELSE");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine($"    PRINT 'Type Table {schema}.{typeTableName} does not exist.';");
            streamWriter.WriteLine("END");
        }
    }
}