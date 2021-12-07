namespace Zork
{
    public class CommandContext
    {
        public Game Game;
        public string CommandString { get; }
        public string Verb { get; set; }
        public string Noun { get; set; }

        public CommandContext(Game game, string commandString, string verb = null, string noun = null)
        {
            Game = game;
            CommandString = commandString;
            Verb = verb;
            Noun = noun;
        }

    }
}
