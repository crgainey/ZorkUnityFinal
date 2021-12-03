using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zork
{
    public class Player
    {
        public EventHandler<Room> LocationChanged;
        public EventHandler<int> MovesChanged;
        public EventHandler<int> ScoreChanged;

        public World World { get; }

        [JsonIgnore]
        public Room Location
        {
            get => _location;
            private set
            {
                if (_location != value)
                {
                    _location = value;
                    LocationChanged?.Invoke(this, _location);
                }
            }
        }

        public int NumberOfMoves
        {
            get => _moves;
            set
            {
                if (_moves != value)
                {
                    _moves = value;
                    MovesChanged?.Invoke(this, _moves);
                }
            }
        }


        public int CurrentScore
        {
            get => _score;
            set
            {
                if (_score != value)
                {
                    _score = value;
                    ScoreChanged?.Invoke(this, _score);
                }
            }
        }

        public List<Item> Inventory
        {
            get => _inventory;
            set
            {
                if(_inventory != value)
                {
                    _inventory = value;
                }
            }
        }

        public Player(World world, string startingLocation)
        {
            Assert.IsTrue(world != null);
            Assert.IsTrue(world.RoomsByName.ContainsKey(startingLocation));

            World = world;
            Location = world.RoomsByName[startingLocation];
            _inventory = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool isValidMove = Location.Neighbors.TryGetValue(direction, out Room neighbor);
            if (isValidMove)
            {
                Location = neighbor;
            }

            return isValidMove;
        }

        private Room _location;
        private List<Item> _inventory;
        private int _moves;
        private int _score;
    }
}
