using System;
using System.Collections.Generic;

namespace MSSharpQL.CommandArgs
{
    public class EnumerateLogin : ICommand
    {
        public static string CommandName => "enum";

        public void Execute(Dictionary<string, string> arguments)
        {
            string sqlServer = "";
            string database = "master";
            bool help = false;
            string helpInfo = @"[*] Enumerate Login Information

Makes use of the ability to create custom assemblies to run system commands.

[*] Arguments:

    [+] /server:        MSSQL Server to connection (hostname or IP)
    [+] /db:            MSSQL Database to use (Default: msdb)
    [+] /help           Display help info

[*] Examples:

    [+] Connect to DC01 & enumerate login information
        MSSharpQL.exe enum /server:dc01
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
            Console.WriteLine("[+] Mode: Enumerating Login Information...\n");

            Commands.Enum.EnumerateLogin(sqlServer, database);
        }
    }
}
