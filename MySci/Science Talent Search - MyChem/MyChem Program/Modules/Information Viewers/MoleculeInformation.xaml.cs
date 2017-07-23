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

using System.Threading;
using System.Windows.Threading;
namespace MyChem_Program
{
    /// <summary>
    /// Interaction logic for MoleculeInformation.xaml
    /// </summary>
    public partial class MoleculeInformation : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        Molecule molecule;
        
        public MoleculeInformation(Molecule moleculeT)
        {
            InitializeComponent();

            molecule = moleculeT;
            lblFormula.Content = molecule.ToString();
            lblMolecularMass.Content = molecule.GetMolecularMass().ToString();
            grid.Children.Add(new MoleculePane(molecule.EmpiricalFormula()));
            grdMolecule.Children.Add(new MoleculePane(molecule,MoleculePane.MoleculePaneMode.display));
            lblMolecularMass_Copy.Content = "";
            timer.Interval += TimeSpan.FromMilliseconds(17);
            timer.Start();
            timer.Tick += new EventHandler(tick);
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
        }
        private void tick(object sender, EventArgs e)
        {
            try
            {
                RecalcValues();
            }
            catch { }
        }
        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!Cache.CheckIsNumeric(txtAmount.Text)) { return; };
            try
            {
                RecalcValues();
            }
            catch { }
        }
        void RecalcValues()
        {
            decimal amount = Convert.ToDecimal(txtAmount.Text);
            BaseUnit unit = BaseUnit.error ;
            BaseUnit unitWanted = BaseUnit.error;
            switch(cmbUnit.Text)
            {
                case "g":
                    unit = BaseUnit.g;
                    break;
                case "mol":
                    unit = BaseUnit.mol;
                    break;
            }
            switch(cmbUnitWanted.Text)
            {
                case "Atoms in: ":
                    unitWanted = BaseUnit.atoms;
                    break;
                case "Mols in: ":
                    unitWanted = BaseUnit.mol;
                    break;
                case "Grams in: ":
                    unitWanted = BaseUnit.g;
                    break;
            } 
            if (unitWanted == BaseUnit.atoms && unit == BaseUnit.g) { lblMolecularMass_Copy.Content = (Math.Round((amount * Constants.avogadrosNumber) / molecule.GetMolecularMass().TotalAmount,0)).ToString("G29") + " atoms"; }
            if (unitWanted == BaseUnit.atoms && unit == BaseUnit.mol) { lblMolecularMass_Copy.Content = (Math.Round(amount * Constants.avogadrosNumber,0)).ToString("G29") + " atoms"; }
            if (unitWanted == BaseUnit.g && unit == BaseUnit.mol) { lblMolecularMass_Copy.Content = (Math.Round((amount) * molecule.GetMolecularMass().TotalAmount,4)).ToString("G29")+ " g"; }
            if (unitWanted == BaseUnit.mol && unit == BaseUnit.g) { lblMolecularMass_Copy.Content = (Math.Round(amount / molecule.GetMolecularMass().TotalAmount, 4)).ToString("G29") + " mol"; }
            if (unitWanted == BaseUnit.g && unit == BaseUnit.g) { lblMolecularMass_Copy.Content = ""; }
            if (unitWanted == BaseUnit.mol && unit == BaseUnit.mol) { lblMolecularMass_Copy.Content = ""; }
            
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
        private void txtAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CheckIsNumeric((TextBox)sender, e);
        }

        private void cmbUnitWanted_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RecalcValues();
            }
            catch
            {

            }
        }

        private void cmbUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RecalcValues();
            }
            catch
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void grdMolecule_Initialized(object sender, EventArgs e)
        {

        }
    }
}
