using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for ElementInformation.xaml
    /// </summary>
    public partial class ElementInformation : Window
    {

    int index;
        public ElementInformation(int indexT, Color eleColour)
        {
            InitializeComponent();
            index = indexT;
            lblName.Content = PeriodicTable.elements[index].name;
            lblEleName.Content = lblName.Content;
            lblSymbol.Content = PeriodicTable.elements[index].symbol;
            lblEleSymbol.Content = lblSymbol.Content;
            lblAtomNo.Content = PeriodicTable.elements[index].AtomicNumber.ToString();
            lblEleAtomNo.Content = lblAtomNo.Content;
            lblAtomicMass.Content = PeriodicTable.elements[index].atomicMass.ToString();
            lblEleMass.Content = lblAtomicMass.Content;
            txtElectronConfig.Text = PeriodicTable.elements[index].GetElectronConfig();
            lblSeries.Content = StringManipulation.AddSpaces(PeriodicTable.elements[index].type.ToString());
            lblPeriod.Content = PeriodicTable.elements[index].Period.ToString();
            lblGroup.Content = PeriodicTable.elements[index].Group.ToString();
            lblElectronegativity.Content = PeriodicTable.elements[index].electronegativity.ToString();
            if (PeriodicTable.elements[index].electronegativity < 0) { lblElectronegativity.Content = "Unknown"; }
            if ((string)(lblGroup.Content) == "0" || (string)(lblGroup.Content) == "-1") { lblGroup.Content = "f-block"; }
            txtNotes.Text = PeriodicTable.elements[index].notes;
            string sTemp = "";
            if (PeriodicTable.elements[index].radioactive) { sTemp = "Yes"; } else { sTemp = "No"; }
            lblRadioactive.Content = sTemp;
            rectEle.Fill = new SolidColorBrush(eleColour);
        }



        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnEdit.IsEnabled = false;
            btnApply.IsEnabled = true;
            txtNotes.IsEnabled = true;
            txtNotes.IsReadOnly = false;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            btnEdit.IsEnabled = true;
            btnApply.IsEnabled = false;
            txtNotes.IsEnabled = false;
            txtNotes.IsReadOnly = true;
            PeriodicTable.elements[index].notes = txtNotes.Text;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\ElementNotes.dtb"))
            {
                foreach (Element ele in PeriodicTable.elements)
                {
                    file.WriteLine(ele.notes);
                    file.WriteLine("_");
                }
            }
        }

    }
}
