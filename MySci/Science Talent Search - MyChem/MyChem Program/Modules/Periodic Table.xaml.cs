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
    /// Interaction logic for Periodic_Table.xaml
    /// </summary>
    public enum TableMode
    {
        information,
        selection,
        empirical,
        standard, // Like empirical but with no % (uses arbitrary units)
        display
    }
    public partial class Periodic_Table : UserControl
    {
        TableMode mode; 
        List<ElementPane> elePanes = new List<ElementPane>();
        public static ElementPane[] elementPanes = new ElementPane[PeriodicTable.elements.Count()];
        public Periodic_Table()
        {
            InitializeComponent();
            mode = TableMode.selection;
        }
        // To me: leave this here if u dont want an error EDIT: Might not be needed anymore now that i've gone module mode
        public Periodic_Table(TableMode modeT = TableMode.information)
        {
            InitializeComponent();
            mode = modeT;
        }
 
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < PeriodicTable.elements.Count(); i++)
            {
                elementPanes[i] = new ElementPane(mode, i);
                theCanvas.Children.Add(elementPanes[i]);
                Canvas.SetLeft(elementPanes[i], 20 + ((PeriodicTable.elements[i].Group - 1) * elementPanes[i].Width));
                Canvas.SetTop(elementPanes[i], PeriodicTable.elements[i].Period * elementPanes[i].Height);
                if (PeriodicTable.elements[i].type == ElementGroup.Lanthanide || PeriodicTable.elements[i].type == ElementGroup.Actinide)
                {
                    Canvas.SetTop(elementPanes[i], (PeriodicTable.elements[i].Period + 3) * elementPanes[i].Height);
                    Canvas.SetLeft(elementPanes[i], 20 + (((PeriodicTable.elements[i].AtomicNumber) - Element.periodProtonsRequired[PeriodicTable.elements[i].Period - 1]) - 1) * elementPanes[i].Width);
                }

            }
        }
        private void btnClearElements_Click(object sender, RoutedEventArgs e)
        {
            foreach (ElementPane elePaneT in theCanvas.Children)
            {
                elePaneT.ResetColours();
                elePaneT.selected = false;
            }
            Cache.elementsSelected.Clear();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To select multiple elements at a time: CTRL + Left Click\nTo view an element's information: Select the element and then left click on the element pane at the bottom of the user interface titled \"Selected Elements\"");
        }
    }
}
