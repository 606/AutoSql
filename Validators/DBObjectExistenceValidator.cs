using System.IO;

/// <summary>
/// Validates the existence of a database object (in this case, a stored procedure) before executing the main content.
/// If the object does not exist, it creates the object before proceeding.
/// </summary>
public class DBObjectExistenceValidator
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
}