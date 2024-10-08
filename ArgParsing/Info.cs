using System;

namespace MSSharpQL.ArgParsing
{
    public static class Info
    {
        public static void ShowLogo()
        {
            Console.WriteLine(".------..------..------..------..------..------..------..------..------.");
            Console.WriteLine("|M.--. ||S.--. ||S.--. ||H.--. ||A.--. ||R.--. ||P.--. ||Q.--. ||L.--. |");
            Console.WriteLine("| (\\/) || :/\\: || :/\\: || :/\\: || (\\/) || :(): || :/\\: || (\\/) || :/\\: |");
            Console.WriteLine("| :\\/: || :\\/: || :\\/: || (__) || :\\/: || ()() || (__) || :\\/: || (__) |");
            Console.WriteLine("| '--'M|| '--'S|| '--'S|| '--'H|| '--'A|| '--'R|| '--'P|| '--'Q|| '--'L|");
            Console.WriteLine("`------'`------'`------'`------'`------'`------'`------'`------'`------'");
            Console.WriteLine("[+] By: Seraph\n");
        }
        public static void ShowUsage()
        {
            string usage = @"[*] Modes:
    
    [+] assem           Run system command via managed code custom assembly
    [+] enum            Enumerate MSSQL server privileges for current user
    [+] interactive     Create an interactive MSSQL session
    [+] spoa            Enable Sp_OACreate & Sp_OAMethod to run system commands
    [+] unc             Force MSSQL server to authenticate against a remote SMB share
    [+] xpcmdshell      Run system command via xp_cmdshell
    
[!] For more information on arguments for each command run the '/help' argument when choosing a mode.
    [+] Ex: MSSharpQL.exe assem /help
";
            Console.WriteLine(usage);
        }
    }
}
