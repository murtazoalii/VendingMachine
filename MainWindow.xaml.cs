using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VendingMachine {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        CashManager _cashManager;
        VendingLogic _vendingLogic;
        CashEmulatorWindow _cashWindow;

        public MainWindow() {
            InitializeComponent();

            _cashManager = new CashManager();
            _vendingLogic = new VendingLogic(_cashManager);

            _vendingLogic.OnStateChanged += UpdateUI;

            _cashWindow = new CashEmulatorWindow();
            _cashWindow.BillAccepted += _cashManager.BillAccepted;
            _cashWindow.BillAccepted += _vendingLogic.BillAccepted;

            _cashWindow.Show();

            UpdateUI();
        }

        private void UpdateUI() {
            textBlockCredit.Text = $"Credit: {_vendingLogic.Credit}";
            listViewProducts.ItemsSource = null;
            listViewProducts.ItemsSource = _vendingLogic.Products;
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            var product = (sender as ListViewItem).Content as Product;
            _vendingLogic.SellProduct(product);
        }

        private void buttonGiveChange_Click(object sender, RoutedEventArgs e) {
            if (!_vendingLogic.ReturnChange(out var changeDistribution))
                Console.WriteLine("Not all change could be given");

            foreach (var pair in changeDistribution)
                Console.WriteLine($"{pair.Key} RUB: {pair.Value}");
        }

        private void Window_Closed(object sender, EventArgs e) {
            Application.Current.Shutdown();
        }        
    }
}
