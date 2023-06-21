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
        const string serverName = "ChurchMusicServer1";
        const string serverIpAddress = "localhost";
        const int serverPort = 1433;

        public static bool QuerySqlServer(string sqlQuery, out DataTable resultTable)
        {
            bool dataRetrieved = false;
            resultTable = new DataTable();
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
                            resultTable.Load(reader);
                            dataRetrieved = true;
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new Exception("Error connecting to SQL Server: " + e.Message);
            }
            return dataRetrieved;
        }
    }
}
