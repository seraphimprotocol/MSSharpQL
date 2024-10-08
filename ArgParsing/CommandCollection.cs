using System;
using System.Collections.Generic;
using MSSharpQL.CommandArgs;

namespace MSSharpQL.ArgParsing
{
    internal class CommandCollection
    {
        private readonly Dictionary<string, Func<ICommand>> _availableCommands = new Dictionary<string, Func<ICommand>>();

        // How To Add A New Comand:
        //  1. Create your command class in the Commands Folder
        //      a. That class must have a CommandName static property that has the Command's name
        //              and must also Implement the ICommand interface
        //      b. Put the code that does the work into the Execute() method
        //  2. Add an entry to the _availableCommands dictionary in the Constructor below.

        public CommandCollection()
        {
            _availableCommands.Add(Assembly.commandName, () => new Assembly());
            _availableCommands.Add(EnumerateLogin.CommandName, () => new EnumerateLogin());
            _availableCommands.Add(InjectUNC.CommandName, () => new InjectUNC());
            _availableCommands.Add(Interactive.CommandName, () => new Interactive());
            _availableCommands.Add(SPOACreateMethod.CommandName, () => new SPOACreateMethod());
            _availableCommands.Add(XPcmdshell.CommandName, () => new XPcmdshell()); 
        }

        public bool ExecuteCommand(string commandName, Dictionary<string, string> arguments)
        {
            bool commandWasFound;

            if (string.IsNullOrEmpty(commandName) || _availableCommands.ContainsKey(commandName) == false)
                commandWasFound = false;
            else
            {
                var command = _availableCommands[commandName].Invoke();

                command.Execute(arguments);

                commandWasFound = true;
            }

            return commandWasFound;
        }
    }
}
