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

namespace poker_game
{
    /// <summary>
    /// Interaction logic for RaiseAmountDialog.xaml
    /// </summary>
    public partial class RaiseAmountDialog : Window
    {
        public int RaiseAmount { get; private set; }

        public RaiseAmountDialog()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtRaiseAmount.Text, out int raiseAmount) && raiseAmount > 0)
            {
                RaiseAmount = raiseAmount;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
