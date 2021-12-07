using System;
using System.Collections.Generic;
using System.Text;

namespace Zork
{
    public static class MovementCommands
    {
        public static void North(CommandContext commandContext) => Move(commandContext.Game, Directions.North);
        public static void South(CommandContext commandContext) => Move(commandContext.Game, Directions.South);
        public static void West(CommandContext commandContext) => Move(commandContext.Game, Directions.West);
        public static void East(CommandContext commandContext) => Move(commandContext.Game, Directions.East);

        public static void Move(Game game, Directions direction)
        {
            bool playerMoved = game.Player.Move(direction);
            if (playerMoved == false)
            {
                game.Output.WriteLine("The way is shut");
            }

        }
    }
}
