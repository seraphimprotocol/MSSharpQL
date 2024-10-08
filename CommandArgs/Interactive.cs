using System;
using System.Collections.Generic;

namespace MSSharpQL.CommandArgs
{
    internal class Interactive : ICommand
    {
        public static string CommandName => "interactive";

        public void Execute(Dictionary<string, string> arguments) 
        {
            string sqlServer = "";
            string database = "master";
            bool help = false;
            string helpInfo = @"[*] Interactive MSSQL Session

Creates an interactive MSSQL session to interact with the database via SQL statements.

[*] Arguments:

    [+] /server:        MSSQL Server to connection (hostname or IP)
    [+] /db:            MSSQL Database to use (Default: master)
    [+] /help           Display help info

[*] Examples:

    [+] Connect to DC01
        MSSharpQL.exe interactive /server:dc01
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

            // Exception Handling

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
            Console.WriteLine("[+] Mode: Creating An Interactive SQL Session...\n");

            Commands.Interact.InteractiveSession(sqlServer, database);
        }
    }
}


// Linked server logic
// 1) IF only linked server is used then run arguments on that server
// 2) IF double linked server is used then check to make sure first linked server has an argument
// 3) IF both empty then continue with normal operation