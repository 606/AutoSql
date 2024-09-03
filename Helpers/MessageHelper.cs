using System.Windows.Forms;
using AutoSql.Consts;

namespace AutoSql.Helpers
{
    static class MessageHelper
    {
        public static void ShowSuccess(string message) => MessageBox.Show(message, AppMessages.LoginSuccess, MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void ShowError(string message) => MessageBox.Show(message, AppMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static void ShowWarning(string message) => MessageBox.Show(message, AppMessages.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}