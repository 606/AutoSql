using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AutoSql.Consts;
using AutoSql.Helpers;
using AutoSql.Services;

namespace AutoSql.Forms
{
    public partial class MainForm : Form
    {
        private readonly GitService _gitService;
        private readonly SqlScriptGenerationService _sqlScriptGenerationService;

        public MainForm()
        {
            InitializeComponent();
            var exportFileHelper = new ExportFileHelper();
            var sqlBlockSplitter = new SqlBlockSplitter();
            var sqlContentExtractor = new SqlContentExtractor();
            var dbObjectExistenceValidator = new SqlExistenceHelper();
            var sqlContentProcessor = new SqlContentProcessor(sqlBlockSplitter, sqlContentExtractor, dbObjectExistenceValidator);

            _gitService = new GitService();
            _sqlScriptGenerationService = new SqlScriptGenerationService(_gitService, sqlContentProcessor, exportFileHelper);
        }

        private void btnGenerateScript_Click(object sender, EventArgs e)
        {
            string repoPath = txtRepoPath.Text;
            string outputPath = txtOutputPath.Text;

            if (string.IsNullOrWhiteSpace(repoPath) || string.IsNullOrWhiteSpace(outputPath))
            {
                MessageBox.Show(ErrorMessages.FillInAllFields, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_gitService.IsGitRepository(repoPath))
            {
                MessageBox.Show(ErrorMessages.InvalidFolderPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                _sqlScriptGenerationService.GenerateScript(repoPath, outputPath);
                MessageBox.Show(AppMessages.SqlScriptGeneratedSuccess, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(ErrorMessages.ErrorWritingToFile, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            txtRepoPath.Text = "C:\\Gitlab\\db-ldbbase2";
            txtOutputPath.Text = "C:\\Gitlab\\db-ldbbase-savedFiles";
        }
    }
}