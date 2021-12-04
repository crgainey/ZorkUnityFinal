using Newtonsoft.Json;
using System.ComponentModel;

namespace Zork
{
    public class Item :INotifyPropertyChanged
    {
#pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        [JsonProperty(Order = 2)]
        public string Description { get; set; }

        [JsonProperty(Order = 3)]
        public bool IsTakeable { get; set; }


    }
}
