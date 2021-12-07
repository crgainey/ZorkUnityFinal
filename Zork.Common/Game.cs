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

        public Game(World world, Player player)
        {
            World = world;
            Player = player;
        }
        public Game()
        {
            Commands = new Dictionary<string, Command>()
            {
                { "QUIT", new Command("QUIT", new string[] { "QUIT", "Q", "BYE" }, Quit) },
                { "LOOK", new Command("LOOK", new string[] { "LOOK", "L" }, Look) },
                { "REWARD", new Command("REWARD", new string[] { "REWARD", "R" }, Reward) },
                { "SCORE", new Command("SCORE", new string[] { "SCORE" }, Score) },
                { "INVENTORY", new Command("INVENTORY", new string[] { "INVENTORY", "I" }, Inventory) },
                { "TAKE", new Command("TAKE", new string[] { "TAKE", "T" }, TakeItem) },
                { "DROP", new Command("INVENTORY", new string[] { "DROP", "D" }, DropItem) },
                { "NORTH", new Command("NORTH", new string[] { "NORTH", "N" }, MovementCommands.North) },
                { "SOUTH", new Command("SOUTH", new string[] { "SOUTH", "S" }, MovementCommands.South) },
                { "EAST", new Command("EAST", new string[] { "EAST", "E"}, MovementCommands.West) },
                { "WEST", new Command("WEST", new string[] { "WEST", "W" }, MovementCommands.East) }


            };

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
            LookStart(this);

            IsRunning = true;
        }

        private void InputRecievedHandler(object sender, string commandString)
        {
            string[] splitString = commandString.Split(' ');
            CommandContext commandContext = new CommandContext(this, commandString);
            {
                if (splitString.Length == 1)
                {
                    commandContext = new CommandContext(this, commandString, splitString[0]);
                }
                else if (splitString.Length == 2)
                {
                    commandContext = new CommandContext(this, commandString, splitString[0], splitString[1]);
                }
            }

            Command foundCommand = null;
            foreach (Command command in Commands.Values)
            {
                if (command.Verbs.Contains(commandContext.Verb))
                {
                    foundCommand = command;
                    break;
                }
            }

            Room previousRoom = Player.Location;
            if (foundCommand != null)
            {
                foundCommand.Action(commandContext);
                Player.NumberOfMoves++;

                if (previousRoom != Player.Location)
                {
                    Look(commandContext);
                    previousRoom = Player.Location;
                }
            }
            else
            {
                Output.WriteLine("Unknown command.");
            }

        }
        public static void LookStart(Game game)
        {
            game.Output.WriteLine($"{game.Player.Location}\n {game.Player.Location.Description}");

            foreach (Item item in game.Player.Location.Items)
            {
                game.Output.WriteLine($"{item.Name} {item.Description}");
            }
        }

        public static void Look(CommandContext commandContext)
        {
            Game game = commandContext.Game;
            game.Output.WriteLine($"{game.Player.Location}\n {game.Player.Location.Description}");

            foreach (Item item in game.Player.Location.Items)
            {
                game.Output.WriteLine(item.Description);
            }
        }

        public static void Inventory(CommandContext commandContext)
        {
            Game game = commandContext.Game;

            if (game.Player.Inventory.Count == 0)
            {
                game.Output.WriteLine("You are empty handed.");
            }
            else
            {
                game.Output.WriteLine("The items in your inventory are: ");
                foreach (Item item in game.Player.Inventory)
                {
                    game.Output.WriteLine(item.Name);
                }
            }
        }

        public static void TakeItem(CommandContext commandContext)
        {
            Game game = commandContext.Game;

            string itemNoun;
            if (commandContext.Noun != null)
            {
                itemNoun = commandContext.Noun;
            }
            else
            {
                itemNoun = null;
                game.Output.WriteLine("What did you want to take?");
                return;
            }

            Item newItem = new Item(itemNoun);
            foreach (Item item in game.Player.Location.Items)
            {
                if (item.Name.ToUpper() == newItem.Name.ToUpper())
                {
                    newItem = item;
                    break;
                }
            }

            if (newItem.IsTakeable)
            {
                game.Output.WriteLine($"You take {newItem.Name}");
                game.Player.Inventory.Add(newItem);
                game.Player.Location.Items.Remove(newItem);
                Reward(commandContext);
            }
            else
            {
                game.Output.WriteLine($"You cannot take {newItem.Name}");
            }

        }

        public static void DropItem(CommandContext commandContext)
        {
            Game game = commandContext.Game;

            string itemNoun;
            if (commandContext.Noun != null)
            {
                itemNoun = commandContext.Noun;
            }
            else
            {
                itemNoun = null;
                game.Output.WriteLine("What did you want to drop?");
                return;
            }

            Item newItem = new Item(itemNoun);
            foreach (Item item in game.Player.Inventory)
            {
                if (item.Name.ToUpper() == newItem.Name.ToUpper())
                {
                    newItem = item;
                    break;
                }
            }

            game.Output.WriteLine($"You drop {newItem.Name}");
            game.Player.Inventory.Remove(newItem);
            game.Player.Location.Items.Add(newItem);

        }

        public static void Reward(CommandContext commandContext) => commandContext.Game.Player.CurrentScore += 5;
        public static void Score(CommandContext commandContext) => commandContext.Game.Output.WriteLine
            ($"Your score would be {commandContext.Game.Player.CurrentScore}, in {commandContext.Game.Player.NumberOfMoves} move(s).");

        public static void Quit(CommandContext commandContext)
        {
            commandContext.Game.IsRunning = false;
            commandContext.Game.Output.WriteLine
                (string.IsNullOrWhiteSpace(commandContext.Game.ExitMessage) ? "Thank you for playing!" : commandContext.Game.ExitMessage);
        }
    }
}

