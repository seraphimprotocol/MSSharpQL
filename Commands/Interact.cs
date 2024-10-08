using MSSharpQL.Connection;
using System;
using System.Data.SqlClient;

namespace MSSharpQL.Commands
{
    internal class Interact
    {
        public static void InteractiveSession(string sqlServer, string database)
        {
            string conStr = $"Server = {sqlServer}; Database = {database}; Integrated Security = True;";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                try
                {
                    con.Open();
                    
                    Connection.DatabaseConnection.GetLoginInformation(conStr, con);

                    while (true)
                    {
                        Console.Write("\nSQL> ");
                        string query = Console.ReadLine();

                        if (query.Trim().ToLower() == "exit")
                            break;

                        string result = DatabaseConnection.ExecuteQuery(query, con);
                        Console.WriteLine(result);
                    }
                    Console.WriteLine("[-] Session Ended. Exiting...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Error: {ex.Message}");
                }
            }
        }
    }
}
