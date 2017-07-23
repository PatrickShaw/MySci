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
using MyChem_Program.Physics___Gravity;
using MyChem_Program.Modules.Calculators;
namespace MyChem_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>     
    public partial class PeriodicTableBrowser : Window
    {
        public PeriodicTableBrowser()
        {
                InitializeComponent();
                Title = Cache.MainWindowTitle;
                this.WindowState = WindowState.Maximized;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Cache.userControlsOpen.Clear();
            btnPeriodicTable_Click(null, null);
        }
                private void btnClose_Click(object sender, RoutedEventArgs e)
                {
                    Close();
                }
                private void btnMoleculeCreator_Click(object sender, RoutedEventArgs e)
                {
                    // RecheckElePanes();
                    theTabControl.AddTab(new Molecule_Creator(),"Molecule Creator",false);
                }
                private void btnEmpiricalFormulas_Click(object sender, RoutedEventArgs e)
                {
                    theTabControl.AddTab(new EmpiricalCalculator(), "Empirical Calculator", true);
                }
                private void btnHelp_Click(object sender, RoutedEventArgs e)
                {
                    MessageBox.Show("Select elements from the periodic table (select multiple elements by holding 'CTRL'). \n\nPressing 'ESC' or the 'Clear' button will clear your current selection.\n\nThe periodic table and the other panels on the screen can be resized by dragging the bars that divide the different sections of the window.\n\nOnce satisfied with your selection of elements click on one of the chemistry tools on the left.");
                }
                private void btnCConvertor_Click(object sender, RoutedEventArgs e)
                {
                    theTabControl.AddTab(new Concentration_Convertor(), "Calculation Calculator", true);
                }
                private void btnGravitySimulator_Click(object sender, RoutedEventArgs e)
                {
                    theTabControl.AddTab(new Gravity_Simulator(), "Gravity Simulator");
                }
                private void btnPeriodicTable_Click(object sender, RoutedEventArgs e)
                {
                        theTabControl.AddTab(new Periodic_Table(), "Periodic Table", false);
                }
                private void theTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
                {
                }
                private void theTabControl_Drop(object sender, DragEventArgs e)
                {
                    if (sender is UndockedModule)
                    {
                        UndockedModule module = (UndockedModule)sender;
                        UserControl contentT = (UserControl)module.Content;
                        //AddTab(contentT, module.Title);
                    }
                }

                private void Aim(object sender, RoutedEventArgs e)
                {
                    WebBrowser2 wb = new WebBrowser2(new Uri(System.IO.Path.GetFullPath(@".\MySci User Documentation.htm#_MySci’s_Aim")));
                    UndockedModule ud = new UndockedModule(wb, "Help", 0);
                    ud.Show();
                }

                private void btnConcentrationConvertor_Click(object sender, RoutedEventArgs e)
                {
                    theTabControl.AddTab(new Concentration_Convertor(), "Concentration Calculator");
                }

                private void btnBibliography_Click(object sender, RoutedEventArgs e)
                {
                    WebBrowser2 wb = new WebBrowser2(new Uri(System.IO.Path.GetFullPath(@".\MySci User Documentation.htm#_Bibliography")));
                    UndockedModule ud = new UndockedModule(wb, "Help", 0);
                    ud.Show();
                }
    }
}
