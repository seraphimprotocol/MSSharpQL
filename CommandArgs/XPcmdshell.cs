using System;
using System.Collections.Generic;

namespace MSSharpQL.CommandArgs
{
    public class XPcmdshell : ICommand
    {
        public static string CommandName => "xpcmdshell";

        public void Execute(Dictionary<string, string> arguments)
        {
            string sqlServer = "";
            string database = "master";
            string userToImpersonate = "";
            string cmd = "";
            string linkedServer = "";
            string doubleLinkedServer = "";
            bool enableRPC = false;
            bool exec = false;
            bool openquery = false;
            bool help = false;
            string helpInfo = @"[*] Xp_cmdshell Code Execution

Makes use of xp_cmdshell to run system commands.

Connecting to a linked server will execute system commands on the host hosting the linked server.

[*] Arguments:

    [+] /server:                MSSQL Server to connection (hostname or IP)
    [+] /db:                    MSSQL Database to use (Default: master)
    [+] /cmd:                   System Command to run
    [+] /linkedserver:          Connect to a linked server to run system commands on that host
    [+] /doublelinkedserver:    Connect to a linked server & then to a second linked server to run commands on that host
    [+] /impersonate:           Impersonate database user before attempting code execution (Only impersonates on first server connection)
    [+] /enablerpc              Enables RPC on the target specified in the linkedserver argument
    [+] /exec                   Executes system command on linked servers using exec statements (Default)
    [+] /openquery              Executes system commands on linked servers using openquery statements (No output returned)
    [+] /help                   Display help info

[*] Examples:

    [+] Connect to DC01 & run whoami on DC01
        MSSharpQL.exe xpcmdshell /server:dc01 /cmd:whoami

    [+] Connect to DC01, Impersonate sa, & run whoami /all on DC01
        MSSharpQL.exe xpcmdshell /server:dc01 /impersonate:sa /cmd:'whoami /all'

    [+] Connect to DC01, Connect to linked server appsrv01, & run whoami on appsrv01
        MSSharpQL.exe xpcmdshell /server:dc01 /linkedserver:appsrv01 /cmd:whoami

    [+] Connect to DC01, Connec to linked server appsrv01, & run whoami on appsrv01 via openquery statements
        MSSharpQL.exe xpcmdshell /server:dc01 /linkedserver:appsrv01 /cmd:whoami /openquery

    [+] Connect to DC01, Connect to first linked server appsrv01, Connect to second linked server dc01, & run whoami on DC01
        MSSharpQL.exe xpcmdshell /server:dc01 /linkedserver:appsrv01 /doublelinkedserver:dc01 /cmd:whoami
";

            // Arguments

            if (arguments.ContainsKey("/server"))
            {
                sqlServer = arguments["/server"];
            }
            if (arguments.ContainsKey("/db"))
            {
                database = arguments["/db"];
            }
            if (arguments.ContainsKey("/impersonate"))
            {
                userToImpersonate = arguments["/impersonate"];
            }
            if (arguments.ContainsKey("/cmd"))
            {
                cmd = arguments["/cmd"];
            }
            if (arguments.ContainsKey("/linkedserver"))
            {
                linkedServer = arguments["/linkedserver"];
            }
            if (arguments.ContainsKey("/doublelinkedserver"))
            {
                doubleLinkedServer = arguments["/doublelinkedserver"];
            }
            if (arguments.ContainsKey("/enablerpc"))
            {
                enableRPC = true;
            }
            if (arguments.ContainsKey("/exec"))
            {
                exec = true;
            }
            if (arguments.ContainsKey("/openquery"))
            {
                openquery = true;
            }
            if (arguments.ContainsKey("/help"))
            {
                help = true;
            }
            
            // Null Arguments Handling


            if (help)
            {
                Console.WriteLine(helpInfo);
                Environment.Exit(0);
            }
            if (string.IsNullOrEmpty(sqlServer))
            {
                Console.WriteLine("[!] Please provide a valid server with the \"/server:\" arugment. Exiting...");
                Environment.Exit(0);
            }
            if (string.IsNullOrEmpty(cmd))
            {
                Console.WriteLine("[!] Please provide a valid command with the \"/cmd:\" arugment. Exiting...");
                Environment.Exit(0);
            }
            if (string.IsNullOrEmpty(linkedServer) && !string.IsNullOrEmpty(doubleLinkedServer))
            {
                Console.WriteLine("[!] Please provide the first linked server with the \"/linkedserver:\" argument when using the \"/doublelinkedserver:\" argument. Exiting...");
                Environment.Exit(0);
            }
            if(string.IsNullOrEmpty(linkedServer) && enableRPC)
            {
                Console.WriteLine("[!] Please provide the target linked server with the \"/linkedserver:\" argument when using the \"/enablerpc\" argument. Exiting...");
                Environment.Exit(0);
            }
            if (exec && openquery)
            {
                Console.WriteLine("[!] Please select either the \"/exec\" or \"/openquery\" argument. Exiting...");
                Environment.Exit(0);
            }
            else if (!exec && !openquery)
            {
                exec = true;
            }
            Console.WriteLine("[+] Mode: Executing System Commands Through xp_cmdshell...\n");

            Commands.XPcmd.XPcmdshell(cmd, userToImpersonate, sqlServer, database, linkedServer, doubleLinkedServer, enableRPC, exec, openquery);
        }
    }
}
