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

namespace MyChem_Program.Modules.Calculators
{
    /// <summary>
    /// Interaction logic for MassConcentrationBox.xaml
    /// </summary>
    public partial class MassConcentrationBox : UserControl
    {
        public Unit massConcentration = new Unit(0, new SubUnit(UnitPrefix.none, BaseUnit.g, 1), new SubUnit(UnitPrefix.none, BaseUnit.L, -1));
        public MassConcentrationBox()
        {
            InitializeComponent();
        }

        private void txtMassConcentrationValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                massConcentration.TotalAmount = Convert.ToDecimal(txtMassConcentrationValue.Text) ;
            }catch
            {
            }
        }

        private void cmbgLg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            massConcentration.subUnits[0].unitPrefix = Cache.SelectionIndexToUnitPrefix(cmbgLg.SelectedIndex);
            txtMassConcentrationValue_TextChanged(null, null);
        }

        private void cmbgLL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            massConcentration.subUnits[1].unitPrefix = Cache.SelectionIndexToUnitPrefix(cmbgLL.SelectedIndex);
            txtMassConcentrationValue_TextChanged(null, null);
        }

        private void txtMassConcentrationValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                CheckIsNumeric((TextBox)txtMassConcentrationValue, e);
            }
            catch
            {

            }
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
