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

namespace MyChem_Program
{
    /// <summary>
    /// Interaction logic for ChangeEmpiricalPercentage.xaml
    /// </summary>
    public partial class ChangeEmpiricalPercentage : Window
    {
        decimal originalPercentage;
        private  void CheckIsNumeric(TextBox sender, TextCompositionEventArgs e)
        {
            decimal result;
            bool dot = sender.Text.IndexOf(".") < 0 && e.Text.Equals(".") && sender.Text.Length > 0;
            if (!(Decimal.TryParse(e.Text, out result) || dot))
            {
                e.Handled = true;
            }
        }
        public ChangeEmpiricalPercentage(int indexT, decimal originalPercentageT)
        {
            originalPercentage = originalPercentageT;
            ElementPane paneTemp = new ElementPane(TableMode.display, indexT);
            paneTemp.Margin = new Thickness(10, 10, 0, 0);
            InitializeComponent();
            grid.Children.Add(paneTemp);
            Grid.SetColumn(paneTemp, 0);
            txtPercentage.Focus();
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public decimal GetPercentage()
        {
            try
            {
                return Convert.ToDecimal(txtPercentage.Text);
            }
            catch { return 0; }
        }

        private void txtPercentage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            CheckIsNumeric((TextBox)sender, e);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            txtPercentage.Text = originalPercentage.ToString();
            Close();
        }
    }
}
