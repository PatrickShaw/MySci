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
    /// Interaction logic for EmpiricalCalculator.xaml
    /// </summary>
    public partial class EmpiricalCalculator : UserControl 
    {
        List<ElementPane> elePanes = new List<ElementPane>();
        public EmpiricalCalculator()
        {
            InitializeComponent();
            elePanes.AddRange(Cache.panesSelected(TableMode.empirical));
            foreach (ElementPane elePane in elePanes)
            {
                eleStack.Children.Add(elePane);
            }
        }

        private void btnAddElement_Click(object sender, RoutedEventArgs e)
        {
            Cache.elementsSelected.Clear();
            Periodic_Table browserTemp = new Periodic_Table(TableMode.selection);//TableMode.selection);
            UndockedModule browserDock = new UndockedModule(browserTemp, (string)(Parent as ModuleTabItem).Header + " - Periodic Table Browser - Add Elements", 0);
            browserDock.ShowDialog();
            foreach(Element ele in Cache.elementsSelected)
            {
                bool alreadyAdded = false;
                foreach (ElementPane elePane in elePanes)
                {
                }
                if (!alreadyAdded) { elePanes.Add(new ElementPane(TableMode.empirical, ele.AtomicNumber - 1)); eleStack.Children.Add(elePanes[elePanes.Count()-1]); }
            }
        }

        private void btnRemoveElement_Click(object sender, RoutedEventArgs e)
        {
            Cache.elementsSelected.Clear();
            for(int i = elePanes.Count() -1; i>=0;i--)
            {
                if (elePanes[i].selected) { eleStack.Children.RemoveAt(i); elePanes.RemoveAt(i); }
            }
        }
        public void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            Cache.elementsSelected.Clear();
            lstOtherCandidates.Items.Clear();
            empMoleGrid.Children.Clear();
            elePanes.Clear();
            eleStack.Children.Clear();
        }
        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            lstOtherCandidates.Items.Clear();
            empMoleGrid.Children.Clear();
            if (elePanes.Count() <= 0) { MessageBox.Show("Elements are required to calculate the empirical formula. To do so, press 'Add Elements'."); return; }
            try
            {
                List<EmpiricalElement> input = new List<EmpiricalElement>();

                foreach (ElementPane elePane in elePanes)
                {
                    if (elePane.Percentage <= 0) { MessageBox.Show("Values cannot be below or equal to 0."); return; }
                    input.Add(new EmpiricalElement(elePane.Percentage, PeriodicTable.elements[elePane.Index]));
                }

                List<Molecule> candidates = EmpiricalCalc.FindEmpiricalCandidates(1000, 0.05M, input.ToArray());

                if (candidates.Count() <= 0) { return; }
                empMoleGrid.Children.Add(new MoleculePane(candidates[0]));
                try
                {
                    for (int i = 1; i < candidates.Count(); i++)
                    {
                        lstOtherCandidates.Items.Add(new MoleculePane(candidates[i]));
                    }
                }
                catch { }
            }
            catch { }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as TabItem).Parent as TabControl).Items.Remove(this);
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCopyOther_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
