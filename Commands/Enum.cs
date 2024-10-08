using System;
using System.Data.SqlClient;

namespace MSSharpQL.Commands
{
    internal class Enum
    {
        public static void EnumerateLogin(string sqlServer, string database)
        {
            string conStr = $"Server = {sqlServer}; Database = {database}; Integrated Security = True;";
            string getUsersWithImpersonationPermission = "SELECT distinct b.name FROM sys.server_permissions a INNER JOIN sys.server_principals b ON a.grantor_principal_id = b.principal_id WHERE a.permission_name = 'IMPERSONATE';";
            string getLinkedServers = "EXEC sp_linkedservers;";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                try
                {
                    con.Open();

                    Connection.DatabaseConnection.GetLoginInformation(conStr, con);

                    string usersWithImpersonationPermission = Connection.DatabaseConnection.ExecuteQuery(getUsersWithImpersonationPermission, con).Trim();
                    Console.WriteLine($"[+] Users with the IMPERSONATE privilege set: {usersWithImpersonationPermission}");

                    string linkedServersAvailable = Connection.DatabaseConnection.ExecuteQuery(getLinkedServers, con).Trim();
                    Console.WriteLine($"[+] Linked Servers Available:\n{linkedServersAvailable}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Error: {ex.Message}");
                }
            }
        }
    }
}
