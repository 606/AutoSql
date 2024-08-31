using System;
using System.IO;
using System.Windows.Forms;
using AutoSql.Forms;
using AutoSql.Helpers;
using AutoSql.Services;
using AutoSql.Validators;

namespace AutoSql
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "/silent")
            {
                // CLI режим для post events
                var gitService = new GitService();
                var sqlContentProcessor = new SqlContentProcessor(new SqlBlockSplitter(), new ProcedureDetailsExtractor(), new DBObjectExistenceValidator());
                var fileHelper = new ExportFileHelper();
                var scriptGenerator = new SqlScriptGenerationService(gitService, sqlContentProcessor, fileHelper);


                // Обробка аргументів для /silent режиму
                string repoPath = Environment.CurrentDirectory; // Значення за замовчуванням
                string outputPath = Path.Combine(Environment.CurrentDirectory, "update.sql"); // Значення за замовчуванням

                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i].StartsWith("/repoPath=", StringComparison.OrdinalIgnoreCase))
                    {
                        repoPath = args[i].Substring("/repoPath=".Length);
                    }
                    else if (args[i].StartsWith("/outputPath=", StringComparison.OrdinalIgnoreCase))
                    {
                        outputPath = args[i].Substring("/outputPath=".Length);
                    }
                }

                // Перевірка існування репозиторію
                if (!Directory.Exists(repoPath) || !File.Exists(Path.Combine(repoPath, ".git")))
                {
                    Console.WriteLine($"Error: The path '{repoPath}' is not a valid Git repository.");
                    return;
                }

                // Генерація скрипта на основі змін у поточному коміті
                scriptGenerator.GenerateScript(repoPath, outputPath);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new LoginForm());
            }
        }
    }
}