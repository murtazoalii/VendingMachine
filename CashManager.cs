using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine {
    public class CashManager {
        Dictionary<int, int> _cashQuantity;
        private const string fileName = "cash.json";

        private int[] changeDenominations = new int[] { 10, 5, 2, 1 };

        public CashManager() {
            Restore();
        }

        public void BillAccepted(int billValue) {
            if (_cashQuantity.ContainsKey(billValue))
                _cashQuantity[billValue] += 1;
            else
                _cashQuantity[billValue] = 1;            
            Save();
        }

        // Persist parameter determines whether we need to decrease coin counters or we are just checking the possibility
        public int GiveChange(int change, out Dictionary<int, int> changeDistribution, bool persist) {
            changeDistribution = new Dictionary<int, int>();
            int i = 0;
            while (change > 0 && i < changeDenominations.Length) {

                if (!_cashQuantity.TryGetValue(changeDenominations[i], out var amountAvailable))
                    amountAvailable = 0;

                var count = Math.Min(change / changeDenominations[i], amountAvailable);
                if (count > 0) {
                    changeDistribution[changeDenominations[i]] = count;

                    if (persist)
                        _cashQuantity[changeDenominations[i]] = amountAvailable - count;
                }
                
                change -= changeDenominations[i] * count;
                i++;
            }

            if (persist)
                Save();
            return change;
        }
        
        public void Save() {
            var json = JsonConvert.SerializeObject(_cashQuantity);
            File.WriteAllText(fileName, json);
        }
        
        public void Restore() {
            try {
                var json = File.ReadAllText(fileName);
                _cashQuantity = JsonConvert.DeserializeObject<Dictionary<int, int>>(json);
            }
            catch {
                _cashQuantity = new Dictionary<int, int>();
            }
        }
    }
}
