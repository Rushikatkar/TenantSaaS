using Microsoft.Data.SqlClient;

namespace BAL.Helpers
{
    public static class DatabaseHelper
    {
        public static void CreateDatabaseIfNotExists(string databaseName)
        {
            using (var connection = new SqlConnection("Server=DESKTOP-PAJ1QPJ\\SQLEXPRESS;TrustServerCertificate=True;Integrated Security=True;"))
            {
                connection.Open();
                var command = new SqlCommand(
                    $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{databaseName}') CREATE DATABASE {databaseName};",
                    connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
