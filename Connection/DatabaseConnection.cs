using System;
using System.Data.SqlClient;

namespace MSSharpQL.Connection
{
    internal class DatabaseConnection
    {
        public static void GetLoginInformation(string conStr, SqlConnection con)
        {
            string getCurrentSystemUser = "SELECT SYSTEM_USER;";
            string getCurrentUserSA = "Select SUSER_SNAME();";
            string getCurrentDBUser = "Select USER_NAME();";
            string getCurrentDB = "SELECT DB_NAME() AS CurrentDatabase;";
            string getCurrentServer = "SELECT LEFT(@@SERVERNAME, CHARINDEX('\\', @@SERVERNAME) - 1) AS ServerName;";

            string cu = Connection.DatabaseConnection.ExecuteQuery(getCurrentSystemUser, con);
            Console.Write($"[+] Current System User: {cu}");
            string cdbu = Connection.DatabaseConnection.ExecuteQuery(getCurrentUserSA, con);
            cdbu = Connection.DatabaseConnection.ExecuteQuery(getCurrentDBUser, con);
            Console.Write($"[+] Current Database User: {cdbu}");
            string cdb = Connection.DatabaseConnection.ExecuteQuery(getCurrentDB, con);
            Console.Write($"[+] Current Database: {cdb}");
            string cs = Connection.DatabaseConnection.ExecuteQuery(getCurrentServer, con);
            Console.Write($"[+] Current Server: {cs}");
        }

        public static void enableLinkedServerRPC(string enableRPCTargetServer, string conStr, SqlConnection con)
        {
            string checkLinkedServerRPCStatus = $"SELECT is_rpc_out_enabled FROM sys.servers WHERE name = '{enableRPCTargetServer}';";
            string enableLinkedServerRPCStatement = $"EXEC sp_serveroption @server = '{enableRPCTargetServer}', @optname = 'rpc', @optvalue = 'true'; EXEC sp_serveroption @server = '{enableRPCTargetServer}', @optname = 'rpc out', @optvalue = 'true';";

            string rpcStatusQuery = Connection.DatabaseConnection.ExecuteQuery(checkLinkedServerRPCStatus, con);
            if (rpcStatusQuery.Trim() == "True")
            {
                Console.WriteLine("[+] RPC is already enabled");
                return;
            }

            Console.WriteLine($"[+] Enabling rpc on the following server: {enableRPCTargetServer}");
            Connection.DatabaseConnection.ExecuteQuery(enableLinkedServerRPCStatement, con);
        }
        public static String ExecuteQuery(String query, SqlConnection con)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                string result = "";
                while (reader.Read())
                {
                    result += reader[0] + "\n";
                }
                reader.Close();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error: {ex.Message}");
                Environment.Exit(0);
                return "";
            }
        }
    }
}
