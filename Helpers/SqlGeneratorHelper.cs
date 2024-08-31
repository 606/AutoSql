public static class SqlGeneratorHelper
{
    public static string GenerateProcedureScript(string procedureName, string procedureBody)
    {
        // Видаляємо створення процедури з початку тіла
        var cleanedProcedureBody = procedureBody.Substring(procedureBody.IndexOf('('));

        return $@"
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = '{procedureName}')
CREATE PROCEDURE dbo.{procedureName}
AS
BEGIN
    RETURN 0;
END
GO

ALTER PROCEDURE dbo.{procedureName}
{cleanedProcedureBody}
GO
";
    }

    // Інші методи, якщо потрібно додати для функцій, тригерів тощо
}
