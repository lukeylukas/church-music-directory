using Azure.Core;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchMusicDirectory
{
    internal class ServerCommunication
    {
        const string serverIpAddress = "localhost";
        const int serverPort = 1433;

        public static DataTable QuerySqlServer(string sqlQuery)
        {
            DataTable resultTable = new DataTable();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = serverIpAddress + "," + serverPort;
                // TODO: doesn't work unless you select "remember me"
                builder.UserID = Properties.Settings.Default.Username;
                builder.Password = Properties.Settings.Default.Password;
                builder.InitialCatalog = "ProvidenceSongs";
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
    }
}
