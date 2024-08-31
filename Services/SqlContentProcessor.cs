using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;
using AutoSql.Validators;

namespace AutoSql.Services
{
    public class SqlContentProcessor
    {
        private readonly SqlBlockSplitter _sqlBlockSplitter;
        private readonly ProcedureDetailsExtractor _procedureDetailsExtractor;
        private readonly DBObjectExistenceValidator _procedureExistenceValidator;

        public SqlContentProcessor(SqlBlockSplitter sqlBlockSplitter, ProcedureDetailsExtractor procedureDetailsExtractor, DBObjectExistenceValidator existenceCheckAdder)
        {
            _sqlBlockSplitter = sqlBlockSplitter;
            _procedureDetailsExtractor = procedureDetailsExtractor;
            _procedureExistenceValidator = existenceCheckAdder;
        }

        public void ProcessFile(string file, string repoPath, StreamWriter streamWriter)
        {
            if (file.EndsWith(".sql"))
            {
                var filePath = Path.Combine(repoPath, file);
                var sqlContent = File.ReadAllText(filePath);

                ProcessSqlContent(sqlContent, streamWriter);
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

        private void ProcessSqlContent(string sqlContent, StreamWriter streamWriter)
        {
            var blocks = _sqlBlockSplitter.SplitSqlContent(sqlContent);

            foreach (var block in blocks)
            {
                var procedureDetails = _procedureDetailsExtractor.Extract(block);

                if (procedureDetails != null)
                {
                    _procedureExistenceValidator.ProcedureExistenceValidator(procedureDetails.Value.Schema, procedureDetails.Value.ProcedureName, streamWriter);
                    var alteredProcedureContent = Regex.Replace(block, @"\bCREATE\s+(PROCEDURE|PROC)\b", "ALTER PROCEDURE", RegexOptions.IgnoreCase);
                    streamWriter.WriteLine(alteredProcedureContent);
                }
                else
                {
                    streamWriter.WriteLine(block);
                }
            }
        }

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