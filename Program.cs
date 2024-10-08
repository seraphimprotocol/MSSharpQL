using MSSharpQL.ArgParsing;
using System;
using System.Collections.Generic;
using System.IO;

namespace MSSharpQL
{
    internal class Program
    {
        private static void MainExecute(string commandName, Dictionary<string, string> parsedArgs)
        {
            Info.ShowLogo();

            try
            {
                var commandFound = new CommandCollection().ExecuteCommand(commandName, parsedArgs);

                if (commandFound == false)
                    Info.ShowUsage();
            }
            catch (Exception e) 
            {
                Console.WriteLine("\r\n[!] Unhandled Exception:\r\n");
                Console.WriteLine(e);
            }
        }

        public static string MainString(string command)
        {
            string[] args = command.Split();

            var parsed = ArgumentParser.Parse(args);
            if (parsed.ParsedOk == false)
            {
                Info.ShowUsage();
                return "Error parsing arguments: ${command}";
            }

            var commandName = args.Length != 0 ? args[0] : "";

            TextWriter realStdOut = Console.Out;
            TextWriter realStdErr = Console.Error;
            TextWriter stdOutWriter = new StringWriter();
            TextWriter stdErrWriter = new StringWriter();
            Console.SetOut(stdOutWriter);
            Console.SetOut(stdErrWriter);

            MainExecute(commandName, parsed.Arguments);

            Console.Out.Flush();
            Console.Error.Flush();
            Console.SetOut(realStdOut);
            Console.SetError(realStdErr);

            string output = "";
            output += stdOutWriter.ToString();
            output += stdErrWriter.ToString();

            return output;
        }

        static void Main(string[] args)
        {
            var parsed = ArgParsing.ArgumentParser.Parse(args);
            if (parsed.ParsedOk == false)
            {
                Info.ShowUsage();
                return;
            }

            var commandName = args.Length != 0 ? args[0] : "";
            
            MainExecute(commandName, parsed.Arguments);
        }
    }
}
