using System.Collections.Generic;

namespace MSSharpQL.CommandArgs
{
    public interface ICommand
    {
        void Execute(Dictionary<string, string> arguments);
    }
}
