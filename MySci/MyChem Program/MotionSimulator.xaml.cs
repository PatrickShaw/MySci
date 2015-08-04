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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyChem_Program
{
    /// <summary>
    /// Interaction logic for MotionSimulator.xaml
    /// </summary>
    public partial class MotionSimulator : UserControl
    {
        public MotionSimulator()
        {
            InitializeComponent();
        }

        private void txtMass_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CheckIsNumeric((TextBox)sender, e);
        }
        private void CheckIsNumeric(TextBox sender, TextCompositionEventArgs e)
        {
            decimal result;
            bool dot = sender.Text.IndexOf(".") < 0 && e.Text.Equals(".") && sender.Text.Length > 0;
            if (!(Decimal.TryParse(e.Text, out result) || dot))
            {
                e.Handled = true;
            }
        }
    }
}
