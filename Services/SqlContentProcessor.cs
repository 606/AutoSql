using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;
using AutoSql.Helpers;
using AutoSql.Consts;
using System.Windows.Forms;

namespace AutoSql.Services
{
    public class SqlContentProcessor
    {
        private readonly SqlBlockSplitter _sqlBlockSplitter;
        private readonly SqlContentExtractor _sqlContentExtractor;
        private readonly SqlExistenceHelper _sqlExistenceHelper;
        private readonly GitService _gitService;
        private readonly ExportFileHelper _exportFileHelper;

        public SqlContentProcessor(SqlBlockSplitter sqlBlockSplitter, SqlContentExtractor sqlContentExtractor, SqlExistenceHelper existenceCheckAdder)
        {
            _sqlBlockSplitter = sqlBlockSplitter;
            _sqlContentExtractor = new SqlContentExtractor();
            _sqlExistenceHelper = existenceCheckAdder;
            _gitService = new GitService();
            _exportFileHelper = new ExportFileHelper();

        }

        public void ProcessFile(string file, string repoPath, StreamWriter streamWriter)
        {
            if (file.EndsWith(".sql"))
            {
                var filePath = Path.Combine(repoPath, file);
                var sqlContent = File.ReadAllText(filePath);

                ProcessSqlContent(sqlContent, streamWriter, file);
                EnsureGoAtEnd(sqlContent, streamWriter);
            }
            else if (file.EndsWith(".cs"))
            {
                streamWriter.WriteLine($"-- CLR Object changed: {file}");
                streamWriter.WriteLine("-- Example script for CLR object change");
            }
            else
            {
                streamWriter.WriteLine($"-- Unrecognized file type: {file}");
            }
        }

        private void ProcessSqlContent(string sqlContent, StreamWriter streamWriter, string fileName)
        {
            var blocks = _sqlBlockSplitter.SplitSqlContent(sqlContent);

            foreach (var block in blocks)
            {
                var objectTypeString = SqlObjectTypeHelper.GetSqlObjectType(block);

                if (Enum.TryParse(objectTypeString, true, out SqlObjectTypes objectType)
                    && SqlAllowedObjectTypes.AllowedTypes.Contains(objectType))
                {
                    switch (objectType)
                    {
                        case SqlObjectTypes.StoredProcedure:
                        case SqlObjectTypes.SqlStoredProcedure:
                        case SqlObjectTypes.ReplicationFilterProcedure:
                        case SqlObjectTypes.ExtendedStoredProcedure:
                        case SqlObjectTypes.ClrStoredProcedure:
                        ProcessStoredProcedure(block, streamWriter);
                        break;

                        // Інші випадки для обробки різних типів SQL об'єктів
                        /*
                        case SqlObjectTypes.SqlScalarFunction:
                        case SqlObjectTypes.SqlTableValuedFunction:
                        case SqlObjectTypes.SqlInlineTableValuedFunction:
                        case SqlObjectTypes.ClrScalarFunction:
                        case SqlObjectTypes.ClrTableValuedFunction:
                            ProcessFunction(block, streamWriter);
                            break;

                        case SqlObjectTypes.Trigger:
                        case SqlObjectTypes.SqlTrigger:
                        case SqlObjectTypes.ClrTrigger:
                            ProcessTrigger(block, streamWriter);
                            break;

                        case SqlObjectTypes.View:
                            ProcessView(block, streamWriter);
                            break;

                        case SqlObjectTypes.UserTable:
                        case SqlObjectTypes.InternalTable:
                        case SqlObjectTypes.SystemTable:
                            ProcessTable(block, repoPath, streamWriter);
                            break;

                        case SqlObjectTypes.CheckConstraint:
                        case SqlObjectTypes.DefaultConstraint:
                        case SqlObjectTypes.EdgeConstraint:
                        case SqlObjectTypes.ForeignKeyConstraint:
                        case SqlObjectTypes.PrimaryKeyConstraint:
                        case SqlObjectTypes.UniqueConstraint:
                            ProcessConstraint(block, streamWriter);
                            break;

                        case SqlObjectTypes.AggregateFunction:
                            ProcessAggregateFunction(block, streamWriter);
                            break;

                        case SqlObjectTypes.PlanGuide:
                            ProcessPlanGuide(block, streamWriter);
                            break;

                        case SqlObjectTypes.Rule:
                            ProcessRule(block, streamWriter);
                            break;

                        case SqlObjectTypes.SequenceObject:
                            ProcessSequence(block, streamWriter);
                            break;

                        case SqlObjectTypes.ServiceQueue:
                            ProcessServiceQueue(block, streamWriter);
                            break;

                        case SqlObjectTypes.Synonym:
                            ProcessSynonym(block, streamWriter);
                            break;

                        case SqlObjectTypes.TypeTable:
                            ProcessTypeTable(block, streamWriter);
                            break;
                        */

                        default:
                        streamWriter.WriteLine(block);
                        break;
                    }
                }
                else
                {
                    // Якщо тип об'єкту не розпізнаний або не дозволений, додаємо файл в ігнор
                    _exportFileHelper.AddIgnoreFile(fileName);
                    streamWriter.WriteLine(block);
                }
            }
        }


        private void ProcessStoredProcedure(string block, StreamWriter streamWriter)
        {
            var procedureDetails = _sqlContentExtractor.ExtractProcedureDetails(block);
            if (procedureDetails != null)
            {
                _sqlExistenceHelper.ProcedureExistenceValidator(procedureDetails.Value.Schema, procedureDetails.Value.ProcedureName, streamWriter);
                var alteredProcedureContent = Regex.Replace(block, @"\bCREATE\s+(PROCEDURE|PROC)\b", "ALTER PROCEDURE", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredProcedureContent);
            }
        }

        /*****
        private void ProcessFunction(string block, StreamWriter streamWriter)
        {
            var functionDetails = ExtractFunctionDetails(block);
            if (functionDetails != null)
            {
                _sqlExistenceHelper.FunctionExistenceValidator(functionDetails.Value.Schema, functionDetails.Value.FunctionName, streamWriter);
                var alteredFunctionContent = Regex.Replace(block, @"\bCREATE\s+FUNCTION\b", "ALTER FUNCTION", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredFunctionContent);
            }
        }

        private void ProcessTrigger(string block, StreamWriter streamWriter)
        {
            var triggerDetails = ExtractTriggerDetails(block);
            if (triggerDetails != null)
            {
                _sqlExistenceHelper.TriggerExistenceValidator(triggerDetails.Value.Schema, triggerDetails.Value.TriggerName, streamWriter);
                var alteredTriggerContent = Regex.Replace(block, @"\bCREATE\s+TRIGGER\b", "ALTER TRIGGER", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredTriggerContent);
            }
        }

        private void ProcessView(string block, StreamWriter streamWriter)
        {
            var viewDetails = ExtractViewDetails(block);
            if (viewDetails != null)
            {
                _sqlExistenceHelper.ViewExistenceValidator(viewDetails.Value.Schema, viewDetails.Value.ViewName, streamWriter);
                var alteredViewContent = Regex.Replace(block, @"\bCREATE\s+VIEW\b", "ALTER VIEW", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredViewContent);
            }
        }

        */


        private void ProcessTable(string block, string repoPath, StreamWriter streamWriter)
        {
            var tableDetails = _sqlContentExtractor.ExtractTableDetails(block);
            if (tableDetails != null)
            {
                var columns = _sqlContentExtractor.ExtractTableColumns(block);
                var changedFiles = _gitService.GetDatabaseObjectChanges(repoPath);
                var createTableFile = changedFiles.FirstOrDefault(file => file.IndexOf(tableDetails.Value.TableName, StringComparison.OrdinalIgnoreCase) >= 0);

                if (createTableFile != null)
                {
                    var createTableBlock = ExtractCreateTableBlockFromFile(createTableFile);

                    _sqlExistenceHelper.TableExistenceValidator(
                        tableDetails.Value.Schema,
                        tableDetails.Value.TableName,
                        createTableBlock,
                        columns,
                        streamWriter
                    );
                }
            }
        }


        private string ExtractCreateTableBlockFromFile(string filePath)
        {
            // Читаємо вміст файлу
            var fileContent = File.ReadAllText(filePath);

            // Логіка для витягування блоку створення таблиці з вмісту файлу
            // Наприклад, використовуючи регулярні вирази або інший метод парсингу
            // Повертаємо блок як є для прикладу
            return fileContent;
        }


        private string ExtractCreateTableBlock(string block)
        {
            // Реалізуйте логіку для витягування блоку створення таблиці з комміту
            // Наприклад, використовуючи регулярні вирази або інший метод парсингу
            return block; // Повертаємо блок як є для прикладу
        }




        /*
        private void ProcessConstraint(string block, StreamWriter streamWriter)
        {
            var constraintDetails = ExtractConstraintDetails(block);
            if (constraintDetails != null)
            {
                _sqlExistenceHelper.ConstraintExistenceValidator(constraintDetails.Value.Schema, constraintDetails.Value.ConstraintName, streamWriter);
                var alteredConstraintContent = Regex.Replace(block, @"\bCREATE\s+CONSTRAINT\b", "ALTER CONSTRAINT", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredConstraintContent);
            }
        }

        private void ProcessAggregateFunction(string block, StreamWriter streamWriter)
        {
            var aggregateFunctionDetails = ExtractAggregateFunctionDetails(block);
            if (aggregateFunctionDetails != null)
            {
                _sqlExistenceHelper.AggregateFunctionExistenceValidator(aggregateFunctionDetails.Value.Schema, aggregateFunctionDetails.Value.FunctionName, streamWriter);
                var alteredAggregateFunctionContent = Regex.Replace(block, @"\bCREATE\s+AGGREGATE\s+FUNCTION\b", "ALTER AGGREGATE FUNCTION", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredAggregateFunctionContent);
            }
        }

        private void ProcessPlanGuide(string block, StreamWriter streamWriter)
        {
            var planGuideDetails = ExtractPlanGuideDetails(block);
            if (planGuideDetails != null)
            {
                _sqlExistenceHelper.PlanGuideExistenceValidator(planGuideDetails.Value.Schema, planGuideDetails.Value.PlanGuideName, streamWriter);
                var alteredPlanGuideContent = Regex.Replace(block, @"\bCREATE\s+PLAN\s+GUIDE\b", "ALTER PLAN GUIDE", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredPlanGuideContent);
            }
        }

        private void ProcessRule(string block, StreamWriter streamWriter)
        {
            var ruleDetails = ExtractRuleDetails(block);
            if (ruleDetails != null)
            {
                _sqlExistenceHelper.RuleExistenceValidator(ruleDetails.Value.Schema, ruleDetails.Value.RuleName, streamWriter);
                var alteredRuleContent = Regex.Replace(block, @"\bCREATE\s+RULE\b", "ALTER RULE", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredRuleContent);
            }
        }

        private void ProcessSequence(string block, StreamWriter streamWriter)
        {
            var sequenceDetails = ExtractSequenceDetails(block);
            if (sequenceDetails != null)
            {
                _sqlExistenceHelper.SequenceExistenceValidator(sequenceDetails.Value.Schema, sequenceDetails.Value.SequenceName, streamWriter);
                var alteredSequenceContent = Regex.Replace(block, @"\bCREATE\s+SEQUENCE\b", "ALTER SEQUENCE", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredSequenceContent);
            }
        }

        private void ProcessServiceQueue(string block, StreamWriter streamWriter)
        {
            var serviceQueueDetails = ExtractServiceQueueDetails(block);
            if (serviceQueueDetails != null)
            {
                _sqlExistenceHelper.ServiceQueueExistenceValidator(serviceQueueDetails.Value.Schema, serviceQueueDetails.Value.QueueName, streamWriter);
                var alteredServiceQueueContent = Regex.Replace(block, @"\bCREATE\s+SERVICE\s+QUEUE\b", "ALTER SERVICE QUEUE", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredServiceQueueContent);
            }
        }

        private void ProcessSynonym(string block, StreamWriter streamWriter)
        {
            var synonymDetails = ExtractSynonymDetails(block);
            if (synonymDetails != null)
            {
                _sqlExistenceHelper.SynonymExistenceValidator(synonymDetails.Value.Schema, synonymDetails.Value.SynonymName, streamWriter);
                var alteredSynonymContent = Regex.Replace(block, @"\bCREATE\s+SYNONYM\b", "ALTER SYNONYM", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredSynonymContent);
            }
        }
   
        private void ProcessTypeTable(string block, StreamWriter streamWriter)
        {
            var extractor = new UserTableExtractor();
            var typeTableDetails = extractor.Extract(block);
            if (typeTableDetails != null)
            {
                _sqlExistenceHelper.TypeTableExistenceValidator(typeTableDetails.Value.Schema, typeTableDetails.Value.TypeTableName, streamWriter);
                var alteredTypeTableContent = Regex.Replace(block, @"\bCREATE\s+TYPE\s+TABLE\b", "ALTER TYPE TABLE", RegexOptions.IgnoreCase);
                streamWriter.WriteLine(alteredTypeTableContent);
            }
        }

     
 
        // Add methods to extract details for each type of object
        private (string Schema, string TableName)? ExtractTableDetails(string block)
        {
            // Implement logic to extract table details
            return null;
        }

        private (string Schema, string FunctionName)? ExtractFunctionDetails(string block)
        {
            // Implement logic to extract function details
            return null;
        }

        private (string Schema, string TriggerName)? ExtractTriggerDetails(string block)
        {
            // Implement logic to extract trigger details
            return null;
        }

        private (string Schema, string ViewName)? ExtractViewDetails(string block)
        {
            // Implement logic to extract view details
            return null;
        }

        private (string Schema, string ConstraintName)? ExtractConstraintDetails(string block)
        {
            // Implement logic to extract constraint details
            return null;
        }

        private (string Schema, string FunctionName)? ExtractAggregateFunctionDetails(string block)
        {
            // Implement logic to extract aggregate function details
            return null;
        }

        private (string Schema, string PlanGuideName)? ExtractPlanGuideDetails(string block)
        {
            // Implement logic to extract plan guide details
            return null;
        }

        private (string Schema, string RuleName)? ExtractRuleDetails(string block)
        {
            // Implement logic to extract rule details
            return null;
        }

        private (string Schema, string SequenceName)? ExtractSequenceDetails(string block)
        {
            // Implement logic to extract sequence details
            return null;
        }

        private (string Schema, string QueueName)? ExtractServiceQueueDetails(string block)
        {
            // Implement logic to extract service queue details
            return null;
        }

        private (string Schema, string SynonymName)? ExtractSynonymDetails(string block)
        {
            // Implement logic to extract synonym details
            return null;
        }

        private (string Schema, string TypeTableName)? ExtractTypeTableDetails(string block)
        {
            // Implement logic to extract type table details
            return null;
        }
        
          */

        private void EnsureGoAtEnd(string sqlContent, StreamWriter streamWriter)
        {
            var lines = sqlContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var lastLines = lines.Skip(Math.Max(0, lines.Length - 15)).ToArray();

            bool goFound = lastLines.Any(line => Regex.IsMatch(line.Trim(), @"^GO\s*$", RegexOptions.IgnoreCase));

            if (!goFound)
            {
                streamWriter.WriteLine("GO");
            }
        }
    }
}