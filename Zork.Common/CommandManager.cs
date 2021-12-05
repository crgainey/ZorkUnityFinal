using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zork
{
    public class CommandManager
    {
        public CommandManager() => _commands = new HashSet<Command>();

        public CommandManager(IEnumerable<Command> commands) => _commands = new HashSet<Command>(commands);

        public CommandContext Parse(string commandString)
        {
            var commandQuery = from command in _commands
                               where command.Verbs.Contains(commandString, StringComparer.OrdinalIgnoreCase)
                               select new CommandContext(commandString, command);

            return commandQuery.FirstOrDefault();
        }

        public bool PreformCommand(Game game, string commandString)
        {
            bool result;
            CommandContext commandContext = Parse(commandString);
            if(commandContext.Command != null)
            {
                commandContext.Command.Action(game, commandContext);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public void AddCommand(Command command) => _commands.Add(command);
        public void RemoveCommand(Command command) => _commands.Remove(command);
        public void AddCommand(IEnumerable<Command> command) => _commands.UnionWith(command);
        public void ClearCommands(Command command) => _commands.Clear();

        private HashSet<Command> _commands;
    }
}
