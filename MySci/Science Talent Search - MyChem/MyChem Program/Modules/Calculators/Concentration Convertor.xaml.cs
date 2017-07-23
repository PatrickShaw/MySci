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
    /// Interaction logic for Concentration_Convertor.xaml
    /// </summary>
    public partial class Concentration_Convertor : UserControl
    {
        enum CConvertorMode
        {
            fromMassC, fromMolC, fromPPM
        }
        public Concentration_Convertor()
        {
            InitializeComponent();
            cmbSelected_SelectionChanged(null, null);
        }
        CConvertorMode mode = CConvertorMode.fromMolC;
        Molecule selectedMolecule = null;
        private void btnOpenMolecule_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openDialog = new System.Windows.Forms.OpenFileDialog();
            try
            {
                openDialog.ShowDialog();
                MoleGrid.Children.Clear();
                if (openDialog.FileName == "" || openDialog.FileName == null) { return; } 
                selectedMolecule = MoleculeFileManager.ReadMolecule(openDialog.FileName);
                MoleGrid.Children.Add(new MoleculePane(selectedMolecule));
            }
            catch  
            { 
            }
        }

        MassConcentrationBox gLB = new MassConcentrationBox();
        MolarConcentrationBox molLB = new MolarConcentrationBox();
      
        private void ValueChange(object sender, RoutedEventArgs e)
        {
            lblErrorMsg.Content = "";
            if (selectedMolecule == null) { lblErrorMsg.Content = "You need to select a molecule."; return; }
            try {  
            switch (mode)
            {
                case CConvertorMode.fromMolC:
                    gLB.txtMassConcentrationValue.Text = (Convert.ToDouble(molLB.txtMolConcentration.Text) * ((double)gLB.massConcentration.subUnits[1].unitPrefix / (double)molLB.molarity.subUnits[1].unitPrefix)*((double)UnitPrefix.none/(double)gLB.massConcentration.subUnits[0].unitPrefix)/(double)selectedMolecule.GetMolecularMass().TotalAmount).ToString();
                    break;
                case CConvertorMode.fromMassC:
                    molLB.txtMolConcentration.Text = (Convert.ToDouble(gLB.txtMassConcentrationValue.Text) * ((double)molLB.molarity.subUnits[1].unitPrefix / (double)gLB.massConcentration.subUnits[1].unitPrefix) * ((double)gLB.massConcentration.subUnits[0].unitPrefix / (double)UnitPrefix.none) * (double) selectedMolecule.GetMolecularMass().TotalAmount).ToString();
                    break;
                case CConvertorMode.fromPPM:
                    break;
            }
            }
            catch {
            }
        }
        private void cmbSelected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdConcentration == null) { return; }
             gLB = new MassConcentrationBox();
             molLB = new MolarConcentrationBox();
            grdConcentration.Children.Clear();
            grdConcentration.Children.Add(gLB);
            grdConcentration.Children.Add(molLB); 
            GradientBrush gBrush = new LinearGradientBrush(Color.FromRgb(45, 45, 58),Color.FromRgb(35, 35, 57),new Point(0,0),new Point(0,1));
            switch(cmbSelected.SelectedIndex)
            {
                case 0:
                    mode = CConvertorMode.fromMolC;
                    Grid.SetRow(molLB,0);
                    Grid.SetRow(gLB,1); 
                    gLB.txtMassConcentrationValue.IsEnabled = false; 
                    molLB.txtMolConcentration.TextChanged += new TextChangedEventHandler(ValueChange);
                  molLB.cmbmolLL.SelectionChanged += new SelectionChangedEventHandler(ValueChange);
                  gLB.cmbgLL.SelectionChanged += new SelectionChangedEventHandler(ValueChange);
                  gLB.cmbgLg.SelectionChanged += new SelectionChangedEventHandler(ValueChange);
                  molLB.grid.Background = gBrush;
                    break;
                case 1:
                    mode = CConvertorMode.fromMassC;
                    Grid.SetRow(gLB,0);
                    Grid.SetRow(molLB,1); 
                    molLB.txtMolConcentration.IsEnabled = false; 
                    gLB.txtMassConcentrationValue.TextChanged += new TextChangedEventHandler(ValueChange);
                  molLB.cmbmolLL.SelectionChanged += new SelectionChangedEventHandler(ValueChange);
                  gLB.cmbgLL.SelectionChanged += new SelectionChangedEventHandler(ValueChange);
                  gLB.cmbgLg.SelectionChanged += new SelectionChangedEventHandler(ValueChange);
                  gLB.grid.Background = gBrush;
                    break;
            }
           
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbSelected.SelectedIndex = 1;
                cmbSelected.SelectedIndex = 0;
                cmbSelected.SelectedIndex = 1;
                selectedMolecule = null;
                MoleGrid.Children.Clear();
            }
            catch
            {

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as TabItem).Parent as TabControl).Items.Remove(this);
        }
    }
}
