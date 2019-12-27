using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine {
    public class Product : INotifyPropertyChanged {
        public string Name { get; set; }
        public string Path { get; set; }
        [JsonIgnore]
        public string FullPath { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        private bool _enabled;

        public bool Enabled {
            get { return _enabled; }
            set {
                _enabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Enabled"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
