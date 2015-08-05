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
    public partial class MolarConcentrationBox : UserControl
    {
        public Unit molarity = new Unit(0, new SubUnit(UnitPrefix.none, BaseUnit.mol, 1), new SubUnit(UnitPrefix.none, BaseUnit.L, -1));
        public MolarConcentrationBox()
        {
            InitializeComponent();
        }

        private void txtMolConcentration_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            try
            {
                molarity.TotalAmount = Convert.ToDecimal(txtMolConcentration.Text) ; 
            }
            catch{
            }
        }
        private void cmbmolLL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            molarity.subUnits[1].unitPrefix = Cache.SelectionIndexToUnitPrefix(cmbmolLL.SelectedIndex);
            txtMolConcentration_TextChanged(null, null);
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
        private void txtMolConcentration_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            try
            {
                CheckIsNumeric((TextBox)txtMolConcentration, e);
            }
            catch
            {

            }
        }
    }
}
