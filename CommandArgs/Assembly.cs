using System;
using System.Collections.Generic;

namespace MSSharpQL.CommandArgs
{
    public class Assembly : ICommand
    {
        public static string commandName => "assem";

        public void Execute(Dictionary<string, string> arguments)
        {
            string sqlServer = "";
            string database = "msdb";
            string userToImpersonate = "sa";
            string cmd = "";
            bool help = false;
            string helpInfo = @"[*] Custom Assmebly Code Execution 

Makes use of the ability to create custom assemblies to run system commands.

[*] Arguments:

    [+] /server:    MSSQL Server to connection (hostname or IP)
    [+] /db:        MSSQL Database to use (Default: msdb)
    [+] /cmd:       System Command to run
    [+] /help       Display help info

[*] Examples:

    [+] Connect to DC01 & run whoami
        MSSharpQL.exe assem /server:dc01 /cmd:whoami

    [+] Connect to DC01 & run whoami /all
        MSSharpQL.exe assem /server:dc01 /cmd:'whoami /all'
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
            Console.WriteLine("[+] Mode: Custom Assembly Command Execution...");
            Console.WriteLine("[+] Please note that the database must have the TRUSTWORTHY property set. The msdb database has this set by default.\n");

            Commands.Assem.assemCodeExecution(cmd, userToImpersonate, sqlServer, database);
        }
    }
}
