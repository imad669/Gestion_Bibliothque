using System.Data.SqlClient;

namespace BibliothequeTP2.DAL
{
    public static class Database
    {
        // CETTE CHAÎNE MARCHE QUAND SSMS AFFICHE "localhost"
        private static string connectionString =
            @"Server=localhost;Database=BibliothequeTP2;Trusted_Connection=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}