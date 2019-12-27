using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VendingMachine {
    /// <summary>
    /// Interaction logic for CashEmulatorWindow.xaml
    /// </summary>
    public partial class CashEmulatorWindow : Window {

        public event Action<int> BillAccepted;

        public CashEmulatorWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            BillAccepted?.Invoke(int.Parse(button.Tag as string));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            //WindowState = WindowState.Minimized;
        }
    }
}
