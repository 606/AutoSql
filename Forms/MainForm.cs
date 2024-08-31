using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using AutoSql.Consts;
using AutoSql.Helpers;
using AutoSql.Services;
using AutoSql.Validators;

namespace AutoSql.Forms
{
    public partial class MainForm : Form
    {
        private readonly SqlScriptGenerationService _generator;
        private readonly ValidationService _validationService;
        private readonly GitService _gitService;

        public MainForm()
        {
            InitializeComponent();
            var sqlBlockSplitter = new SqlBlockSplitter();
            var procedureDetailsExtractor = new ProcedureDetailsExtractor();
            var dbObjectExistenceValidator = new DBObjectExistenceValidator();
            _generator = new SqlScriptGenerationService(new GitService(), new SqlContentProcessor(sqlBlockSplitter, procedureDetailsExtractor, dbObjectExistenceValidator), new ExportFileHelper());
            _validationService = new ValidationService();
            _gitService = new GitService();

            // Додаємо валідатори
            _validationService.AddValidator(new RepositoryValidator(_gitService));
            _validationService.AddValidator(new CommitValidator(_gitService));
            _validationService.AddValidator(new SqlChangesValidator(_gitService));
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            var repoPath = txtRepoPath.Text;
            var outputPath = txtOutputPath.Text;
            var commitHash = _gitService.GetCurrentCommit(repoPath); // Отримання поточного комміту

            if (!Directory.Exists(outputPath))
            {
                try
                {
                    Directory.CreateDirectory(outputPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to create output directory: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }


            if (_validationService.ValidateAll(commitHash, repoPath, out List<string> errorMessages))
            {
                _generator.GenerateScript(repoPath, outputPath);
                MessageHelper.ShowSuccess(AppMessages.SqlScriptGeneratedSuccess);
                Application.Exit();
            }
            else
            {
                foreach (var message in errorMessages)
                {
                    MessageHelper.ShowError(message);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            txtRepoPath.Text = "C:\\Gitlab\\db-ldbbase2";
            txtOutputPath.Text = "C:\\Gitlab\\db-ldbbase-savedFiles";
        }
    }
}