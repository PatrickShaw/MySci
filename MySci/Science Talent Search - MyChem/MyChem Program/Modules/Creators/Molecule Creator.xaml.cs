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
using System.Windows.Threading;
using System.Diagnostics;
namespace MyChem_Program
{
    /// <summary>
    /// Interaction logic for Molecule_Creator.xaml
    /// </summary>
    public partial class Molecule_Creator : UserControl  
    { 
        System.Windows.Forms.OpenFileDialog openDialog = new System.Windows.Forms.OpenFileDialog();
        System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();
        List<ElementPane> elePanes = new List<ElementPane>();
        public Molecule_Creator()
        {
            // Save Dialog setttings
            saveDialog.DefaultExt = ".mol";
            saveDialog.Filter = "Molecule Files|*.mol";
            saveDialog.RestoreDirectory = true;
            saveDialog.InitialDirectory = Cache.defaultMoleculeFolder;

            // Open Dialog Settings
            openDialog.DefaultExt = ".mol";
            openDialog.Filter = "Molecule Files|*.mol";
            openDialog.CheckPathExists = true;
            openDialog.CheckFileExists = true;
            openDialog.RestoreDirectory = true;
            openDialog.InitialDirectory = Cache.defaultMoleculeFolder;

            InitializeComponent();
            
            if (Cache.moleculesSelected.Count() > 0 )
            {
                CalcFromCache();
            }
            else
            {
                if (Cache.elementsSelected.Count() > 0)
                {
                    elePanes.AddRange(Cache.panesSelected(TableMode.standard));
                    foreach (ElementPane elePane in elePanes)
                    {
                        eleStack.Children.Add(elePane);
                    }
                }
            }
            CalcMolecule();
        }
        private void btnAddElement_Click(object sender, RoutedEventArgs e)
        {
            Cache.elementsSelected.Clear();
            UndockedModule browserTemp = new UndockedModule(new Periodic_Table(TableMode.selection),(string)(Parent as ModuleTabItem).Header +" - Periodic Table Browser - Add Elements",0);
            browserTemp.ShowDialog();
            foreach(Element ele in Cache.elementsSelected)
            {
                elePanes.Add(new ElementPane(TableMode.standard, ele.AtomicNumber - 1)); eleStack.Children.Add(elePanes[elePanes.Count() - 1]);
                elePanes[elePanes.Count - 1].OnChangeValue = CalcMolecule;
                elePanes[elePanes.Count - 1].Loaded += new RoutedEventHandler(RoutedCalcMolecule);
                elePanes[elePanes.Count - 1].Unloaded += new RoutedEventHandler(RoutedCalcMolecule);
            }
            CalcMolecule();
        }
        private void RoutedCalcMolecule(object sender, RoutedEventArgs e)
        {
            CalcMolecule();
        }
        private void CalcMolecule()
        {
            lblWarning.Content = "";
            if (elePanes.Count() == 1)
            {
                if ((int)elePanes[0].Percentage == 1) { lblWarning.Content = "You only have 1 atom in your molecule, thus, it isn't actually a molecule. Please add some more elements."; return; }
            }
            if (elePanes.Count() <= 0) { lblWarning.Content = "Elements are required to create a molecule. To do so, press 'Add Elements'."; return; }
            try
            {
                List<LiveElement> input = new List<LiveElement>();
                foreach (ElementPane elePane in elePanes)
                {
                    if (elePane.Percentage <= 0) { lblWarning.Content = "WARNING: One or more elements have an amount equal to or lower than 0.";  return; }
                    input.Add(new LiveElement(PeriodicTable.elements[elePane.Index], (int)elePane.Percentage));
                }
                empMoleGrid.Children.Add(new MoleculePane(input.ToArray()));
                Cache.moleculesSelected.Clear();
                Cache.moleculesSelected.Add(new Molecule(input.ToArray()));
            }
            catch { Debug.WriteLine("Failed"); }
            lblWarning.Content = "";
        }
        private void CalcFromCache()
        {
            empMoleGrid.Children.Add(new MoleculePane(Cache.moleculesSelected[0]));
            for (int i = 0; i < Cache.moleculesSelected[0].elementList.Count; i++)
            {
                elePanes.Add(new ElementPane(TableMode.standard, Cache.moleculesSelected[0].elementList[i].element.AtomicNumber - 1));
                eleStack.Children.Add(elePanes[i]);
                elePanes[i].Percentage = Cache.moleculesSelected[0].elementList[i].amount;
                elePanes[i].eleName = ((int)elePanes[i].Percentage).ToString();
            }
        }
        private void btnRemoveElement_Click(object sender, RoutedEventArgs e)
        {
            Cache.elementsSelected.Clear();
            for(int i = elePanes.Count() -1; i>=0;i--)
            {
                if (elePanes[i].selected) { eleStack.Children.RemoveAt(i); elePanes.RemoveAt(i); }
            }
            CalcMolecule();
        }
        public void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            Cache.elementsSelected.Clear();
            empMoleGrid.Children.Clear();
            elePanes.Clear();
            eleStack.Children.Clear();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as TabItem).Parent as TabControl).Items.Remove(this);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) { return; }
                if (saveDialog.FileName == "" || saveDialog.FileName == null) { return; }
                MoleculeFileManager.SaveMolecule(Cache.moleculesSelected[0], saveDialog.FileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) { return;}
                if (openDialog.FileName == "" || openDialog.FileName == null ) { return; } 
                elePanes.Clear();
                Cache.moleculesSelected.Clear();
                empMoleGrid.Children.Clear();
                eleStack.Children.Clear();
                Cache.moleculesSelected.Add(MoleculeFileManager.ReadMolecule(openDialog.FileName));
                CalcFromCache();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            } 
        }
        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as TabItem).Parent as TabControl).Items.Remove(this);
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.LeftCtrl))
            {

            }
        }


    }
}
