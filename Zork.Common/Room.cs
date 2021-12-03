using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace Zork
{
    public class Room : IEquatable<Room>, INotifyPropertyChanged
    {
#pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        [JsonProperty(Order = 2)]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "Neighbors", Order = 3)]
        private Dictionary<Directions, string> NeighborNames { get; set; } = new Dictionary<Directions, string>();

        [JsonIgnore]
        public IReadOnlyDictionary<Directions, Room> Neighbors => _neighbors;

        //[JsonProperty(PropertyName = "ItemsInRoom", Order = 4)]
        //public List<string> ItemsInRoomNames { get; set; }

        //[JsonIgnore]
        //public List<Item> ItemsInRoom { get; set; }


        public Room(string name = null, List<string> itemsInRoomNames = null)
        {
            Name = name;
            //ItemsInRoomNames = itemsInRoomNames ?? new List<string>();
           //ItemsInRoom = new List<Item>();
        }

        public static bool operator ==(Room lhs, Room rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if (lhs is null || rhs is null)
            {
                return false;
            }

            return string.Compare(lhs.Name, rhs.Name, ignoreCase: true) == 0;
        }

        public static bool operator !=(Room lhs, Room rhs) => !(lhs == rhs);

        public override bool Equals(object obj) => obj is Room room && this == room;

        public bool Equals(Room other) => this == other;

        public override string ToString() => Name;

        public override int GetHashCode() => Name.GetHashCode();

        //public void UpdateItemsInRoom(World world)
        //{
        //    ItemsInRoom = (from itemName in ItemsInRoomNames
        //                   let item = world.Items.Find(i => i.Name.Equals(itemName, StringComparison.InvariantCultureIgnoreCase))
        //                   where item != null
        //                   select item).ToList();

        //    ItemsInRoomNames.Clear();
        //    //_items.Clear();
        //    //foreach(var entry in ItemsInRoomNames)
        //    //{
        //    //    _items.Add(world.ItemsByName[entry]);
        //    //}


        //}

        public void UpdateNeighbors(World world)
        {
            _neighbors.Clear();
            foreach (var entry in NeighborNames)
            {
                _neighbors.Add(entry.Key, world.RoomsByName[entry.Value]);
            }
        }

        public void RemoveNeighbor(Directions direction)
        {
            _neighbors.Remove(direction);
            NeighborNames.Remove(direction);
        }

        public void AssignNeighbor(Directions direction, Room neighbor)
        {
            _neighbors[direction] = neighbor;
            NeighborNames[direction] = neighbor.Name;
        }

        private Dictionary<Directions, Room> _neighbors = new Dictionary<Directions, Room>();
        //private List<Item> _items = new List<Item>();
    }
}