# MSSharpQL

This is a MSSQL tool that can do some basic enumeration and exploitation. This tool is meant to be executed on a domain joined windows host where the current user session has access to the MSSQL servers available in the domain.

# Info

```
.------..------..------..------..------..------..------..------..------.
|M.--. ||S.--. ||S.--. ||H.--. ||A.--. ||R.--. ||P.--. ||Q.--. ||L.--. |
| (\/) || :/\: || :/\: || :/\: || (\/) || :(): || :/\: || (\/) || :/\: |
| :\/: || :\/: || :\/: || (__) || :\/: || ()() || (__) || :\/: || (__) |
| '--'M|| '--'S|| '--'S|| '--'H|| '--'A|| '--'R|| '--'P|| '--'Q|| '--'L|
`------'`------'`------'`------'`------'`------'`------'`------'`------'
[+] By: Seraph

[*] Modes:

    [+] assem           Run system command via managed code custom assembly
    [+] enum            Enumerate MSSQL server privileges for current user
    [+] interactive     Create an interactive MSSQL session
    [+] spoa            Enable Sp_OACreate & Sp_OAMethod to run system commands
    [+] unc             Force MSSQL server to authenticate against a remote SMB share
    [+] xpcmdshell      Run system command via xp_cmdshell

[!] For more information on arguments for each command run the '/help' argument when choosing a mode.
    [+] Ex: MSSharpQL.exe assem /help
```
