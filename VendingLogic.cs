using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine {
    public class VendingLogic {
        private const string FileName = "products.json";

        private List<Product> _products;
        private CashManager _cashManager;
        private int _credit = 0;

        public event Action OnStateChanged;

        public List<Product> Products {
            get {
                return _products;
            }
        }

        public int Credit {
            get {
                return _credit;
            }
            set {
                _credit = value;
                CreditChanged();
            }
        }

        public VendingLogic(CashManager cashManager) {
            _cashManager = cashManager;

            Restore();
        }

        private void CreditChanged() {
            foreach (var product in _products) {
                if (product.Price > _credit || product.Quantity == 0 || 
                    _cashManager.GiveChange(_credit - product.Price, out _, persist: false) != 0)
                    product.Enabled = false;
                else
                    product.Enabled = true;
            }            
            OnStateChanged?.Invoke();
        }

        public bool ReturnChange(out Dictionary<int, int> changeDistribution) {
            Credit = _cashManager.GiveChange(Credit, out changeDistribution, persist: true);
            return Credit == 0;
        }

        public void BillAccepted(int billValue) {
            _credit += billValue;
            CreditChanged();
        }

        public void SellProduct(Product product) {
            if (product.Enabled) {
                product.Quantity--;
                Credit -= product.Price;
                Save();
            }
        }

        private void Restore() {
            try {
                var json = File.ReadAllText(FileName);
                _products = JsonConvert.DeserializeObject<List<Product>>(json);
            }
            catch {
                _products = new List<Product>();
            }
            foreach (var product in _products) {
                product.FullPath = Path.Combine(Environment.CurrentDirectory, "Images", product.Path);
            }
        }

        public void Save() {
            var json = JsonConvert.SerializeObject(_products);
            File.WriteAllText(FileName, json);
        }
        
    }
}
