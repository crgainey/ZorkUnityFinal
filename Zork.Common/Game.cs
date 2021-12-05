using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Zork
{
    public class Game : INotifyPropertyChanged
    {
#pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

        public World World { get; set; }

        public string StartingLocation { get; set; }

        public string WelcomeMessage { get; set; }

        public string ExitMessage { get; set; }

        public IOutputService Output { get; set; }

        public IInputService Input { get; set; }

        [JsonIgnore]
        public Player Player { get; set; }
        [JsonIgnore]
        public bool IsRunning { get; set; }
        [JsonIgnore]
        public Dictionary<string, Command> Commands { get; private set; }
        [JsonIgnore]
        public CommandManager CommandManager { get; }

        public Game(World world, Player player)
        {
            World = world;
            Player = player;
        }
        public Game()
        {
            Command[] commands =
            {
                new Command("QUIT", new string[] { "QUIT", "Q", "BYE" }, Quit),
                new Command("LOOK", new string[] { "LOOK", "L" }, Look),
                new Command("REWARD", new string[] { "REWARD", "R" }, Reward),
                new Command("SCORE", new string[] { "SCORE" }, Score),
                new Command("INVENTORY", new string[] { "INVENTORY", "I" }, Inventory),
                new Command("TAKE", new string[] { "TAKE", "T" }, TakeItem),
                new Command("DROP", new string[] { "DROP", "D" }, DropItem),
                new Command("NORTH", new string[] { "NORTH", "N" }, MovementCommands.North),
                new Command("SOUTH", new string[] { "SOUTH", "S" }, MovementCommands.South),
                new Command("WEST", new string[] { "WEST", "W" }, MovementCommands.West),
                new Command("EAST", new string[] { "EAST", "E" }, MovementCommands.East)

            };

            CommandManager = new CommandManager(commands);
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context) => Player = new Player(World, StartingLocation);

        public void Start(IInputService input, IOutputService output)
        {
            Assert.IsNotNull(input);
            Input = input;
            Input.InputRecieved += InputRecievedHandler;

            Assert.IsNotNull(output);
            Output = output;

            output.WriteLine(string.IsNullOrWhiteSpace(WelcomeMessage) ? "Welcome to Zork!" : WelcomeMessage);
            CommandManager.PreformCommand(this, "LOOK");

            IsRunning = true;
        }

        private void InputRecievedHandler(object sender, string commandString)
        {
            Room previousRoom = Player.Location;
            
            if (CommandManager.PreformCommand(this, commandString))
            {
                Player.NumberOfMoves++;

                if (previousRoom != Player.Location)
                {
                    CommandManager.PreformCommand(this, "LOOK");
                    previousRoom = Player.Location;
                }
            }
            else
            {
                Output.WriteLine("Unknown command.");
            }

        }

        public static void Look(Game game, CommandContext commandContext)
        {
            game.Output.WriteLine($"{game.Player.Location}\n {game.Player.Location.Description}");

            foreach (Item item in game.Player.Location.Items)
            {
                game.Output.WriteLine($"{item.Name} {item.Description}");
            }
        }

        public static void Inventory(Game game, CommandContext commandContext)
        {
            if (game.Player.Inventory.Count == 0)
            {
                game.Output.WriteLine("You are empty handed.");
            }
            else
            {
                foreach (Item item in game.Player.Inventory)
                {
                    game.Output.WriteLine(item.Name);
                }
            }
        }

        public static void TakeItem(Game game, CommandContext commandContext)
        {
            foreach (Item item in game.Player.Location.Items)
            {
                if (item.IsTakeable)
                {
                    //add to inventory
                }
            }
        }

        public static void DropItem(Game game, CommandContext commandContext)
        {

        }

        public static void Reward(Game game, CommandContext commandContext) => game.Player.CurrentScore += 5;
        public static void Score(Game game, CommandContext commandContext) => game.Output.WriteLine($"Your score would be {game.Player.CurrentScore}, in {game.Player.NumberOfMoves} move(s).");

        public static void Quit(Game game, CommandContext commandContext)
        {
            game.IsRunning = false;
            game.Output.WriteLine(string.IsNullOrWhiteSpace(game.ExitMessage) ? "Thank you for playing!" : game.ExitMessage);
        }
    }

}
