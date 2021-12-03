﻿using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            const string defaultGameFileName = "Zork.json";
            string gameFilename = args.Length > 0 ? args[(int)CommandLineArguments.GameFileName] : defaultGameFileName;

            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(gameFilename));

            ConsoleInputService input = new ConsoleInputService();
            ConsoleOutputService output = new ConsoleOutputService();

            output.WriteLine(string.IsNullOrWhiteSpace(game.WelcomeMessage) ? "Welcome to Zork!" : game.WelcomeMessage);
            game.Start(input,output);
            Game.Look(game);
            output.WriteLine(game.World.Items);
            output.WriteLine(game.World.Rooms);
            output.WriteLine(game.World.RoomsByName);
            output.WriteLine(game.Player.Location);
            output.WriteLine(game.Player.Items);


            while (game.IsRunning)
            {
                output.Write(">");
                input.ProcessInput();
            }
            
        }

        enum CommandLineArguments
        {
            GameFileName = 0
        }

    }
}
