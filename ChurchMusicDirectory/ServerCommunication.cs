using Microsoft.Data.SqlClient;
using System.Data;

namespace ChurchMusicDirectory
{
    internal class ServerCommunication
    {
        const string serverIpAddress = "localhost";
        const int serverPort = 1433;
        const string catalog = "ProvidenceSongs";

        public static DataTable QuerySqlServer(string sqlQuery, string userName, string password)
        {
            DataTable resultTable = new DataTable();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = serverIpAddress + "," + serverPort;
                builder.UserID = userName;
                builder.Password = password;
                builder.InitialCatalog = catalog;
                builder.TrustServerCertificate = true;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                resultTable.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new Exception("Error connecting to SQL Server: " + e.Message);
            }
            return resultTable;
        }

        public static DataTable CommandSqlServer(string sqlQuery, string[] values, string userName, string password)
        {
            DataTable resultTable = new DataTable();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = serverIpAddress + "," + serverPort;
                builder.UserID = userName;
                builder.Password = password;
                builder.InitialCatalog = catalog;
                builder.TrustServerCertificate = true;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            command.Parameters.AddWithValue("@" + i, values[i]);
                        }
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                throw new Exception("Error connecting to SQL Server: " + e.Message);
            }
            return resultTable;
        }
    }
}
