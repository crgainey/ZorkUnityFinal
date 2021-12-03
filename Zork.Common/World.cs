using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Zork
{
    public class World : INotifyPropertyChanged
    {
#pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067


        public List<Room> Rooms { get; set; }

        public List<Item> Items { get; set; }

        [JsonIgnore]
        public IReadOnlyDictionary<string, Room> RoomsByName => _roomsByName;

        [JsonIgnore]
        public IReadOnlyDictionary<string, Item> ItemsByName => _itemsByName;

        public World()
        {
            Rooms = new List<Room>();
            _roomsByName = new Dictionary<string, Room>();

            Items = new List<Item>();
            _itemsByName = new Dictionary<string, Item>();

        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            _roomsByName = Rooms.ToDictionary(room => room.Name, room => room);
            _itemsByName = Items.ToDictionary(item => item.Name, item => item);

            foreach (Room room in Rooms)
            {
                room.UpdateNeighbors(this);
                room.UpdateItems(this);
            }
        }

        private Dictionary<string, Room> _roomsByName;
        private Dictionary<string, Item> _itemsByName;
    }

}
