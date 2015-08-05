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
using System.Diagnostics;
using System.IO;
using Rainbow_Algorithm___Patrick_Shaw;
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
        Canvas periodicTableCanvas;
        public Periodic_Table()
        {
            InitializeComponent();
            mode = TableMode.information; 
        }
        // To me: leave this here if u dont want an error EDIT: Might not be needed anymore now that i've gone module mode
        public Periodic_Table(TableMode modeT = TableMode.information)
        {
            InitializeComponent();
            mode = modeT;
        }

        public void ElePane_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClearSearch();
            eleWrap.Children.Clear();
            for(int i= 0; i<Cache.elementsSelected.Count;i++)
            {
                eleWrap.Children.Add(new ElementPane(TableMode.information,Cache.elementsSelected[i].AtomicNumber -1));
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(periodicTableCanvas != null)
            {
                theCanvas = periodicTableCanvas;
                return;
            }
            for (int i = 0; i < PeriodicTable.elements.Count(); i++)
            {
                elementPanes[i] = new ElementPane(mode, i);
                theCanvas.Children.Add(elementPanes[i]);
                if (PeriodicTable.elements[i].type == ElementGroup.Lanthanide || PeriodicTable.elements[i].type == ElementGroup.Actinide)
                {
                    Canvas.SetTop(elementPanes[i], (PeriodicTable.elements[i].Period + 3) * elementPanes[i].Height);
                    Canvas.SetLeft(elementPanes[i], 20 + (((PeriodicTable.elements[i].AtomicNumber) - Element.periodProtonsRequired[PeriodicTable.elements[i].Period - 1]) - 1) * elementPanes[i].Width);
                }
                else
                {
                    Canvas.SetLeft(elementPanes[i], 20 + ((PeriodicTable.elements[i].Group - 1) * elementPanes[i].Width));
                    Canvas.SetTop(elementPanes[i], PeriodicTable.elements[i].Period * elementPanes[i].Height);
                }
                elementPanes[i].MouseLeftButtonUp += new MouseButtonEventHandler(ElePane_MouseUp);
            }
            if (mode == TableMode.information)
            {
                btnOkay.IsEnabled = false;
                btnCancel.Content = "Close";
                
            }
            periodicTableCanvas = theCanvas;
}
        private void btnClearElements_Click(object sender, RoutedEventArgs e)
        {
            foreach (ElementPane elePaneT in theCanvas.Children)
            {
                elePaneT.ResetColours();
                elePaneT.selected = false;
            }
            Cache.elementsSelected.Clear();
            eleWrap.Children.Clear();
        }
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To select multiple elements at a time: CTRL + Left Click\nTo view an element's information: Select the element and then left click on the element pane at the bottom of the user interface titled \"Selected Elements\"");
        }


        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            ((this.Parent as ModuleTabItem).Parent as ModuleTabControl).Items.Remove(this.Parent);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ((this.Parent as ModuleTabItem).Parent as ModuleTabControl).Items.Remove(this.Parent);
        }
        private void ClearSearch()
        {
            txtSearch.Text = "";
            for (int i = 0; i < elementPanes.Count(); i++) { elementPanes[i].Visibility = System.Windows.Visibility.Visible; }
        }
        private void InitiateSeriesMode()
        {
            try
            {
                for (int i = 0; i < elementPanes.Count(); i++)
                {
                    elementPanes[i].BrushDefault = new SolidColorBrush(Element.GetElementColour(elementPanes[i].Index));
                    elementPanes[i].eleName = PeriodicTable.elements[elementPanes[i].Index].name;
                }
            }
            catch { }
        }
        private void InitiateAtomicMassMode()
        {
            try
            {
                for (int i = 0; i < elementPanes.Count(); i++)
                {
                    elementPanes[i].BrushDefault = new SolidColorBrush(PeriodicTable.elements[elementPanes[i].Index].GetAtomicMassColour());
                    elementPanes[i].eleName = PeriodicTable.elements[elementPanes[i].Index].name;
                }
            }
            catch { }
        }
        private void InitiateFirstIonisation()
        {
            for (int i = 0; i < elementPanes.Count(); i++)
            {
                elementPanes[i].BrushDefault = new SolidColorBrush(PeriodicTable.elements[elementPanes[i].Index].GetFirstIonisationColour());
                elementPanes[i].eleName = PeriodicTable.elements[elementPanes[i].Index].firstIonisation.ToString();
                if (elementPanes[i].eleName == "-1") { elementPanes[i].eleName = "Unknown"; }
            }
        }
        private void InitiateAtomicNumberMode()
        {
            try
            {
                for (int i = 0; i < elementPanes.Count(); i++)
                {
                    elementPanes[i].BrushDefault = new SolidColorBrush(PeriodicTable.elements[elementPanes[i].Index].GetAtomicNumberColour());
                    elementPanes[i].eleName = PeriodicTable.elements[elementPanes[i].Index].name;
                }
            }
            catch { }
        }
        private void InitiatePaulingScale()
        {
            
            for (int i = 0; i < elementPanes.Count(); i++)
            {
                   elementPanes[i].BrushDefault = new SolidColorBrush(PeriodicTable.elements[elementPanes[i].Index].GetPaulingScaleColor());
                   elementPanes[i].eleName = PeriodicTable.elements[elementPanes[i].Index].electronegativity.ToString();
                   if (elementPanes[i].eleName == "-1") { elementPanes[i].eleName = "Unknown"; }
            }
        }
        private void InitiateSearch()
        {
            if (txtSearch.Text == ""){ClearSearch();return;}
            string temp = "";
            temp += char.ToUpper(txtSearch.Text[0]);
            for (int i = 1; i < txtSearch.Text.Count();i++ )
            {
                temp += txtSearch.Text[i];
            }
                for (int i = 0; i < elementPanes.Count(); i++)
                {
                    if (!(elementPanes[i].eleName.Contains(temp) || elementPanes[i].Symbol.Contains(temp) || elementPanes[i].AtomicNumber.Contains(temp) || elementPanes[i].AtomicMass.Contains(temp)))
                    {
                        elementPanes[i].Visibility = System.Windows.Visibility.Hidden;
                    }
                    else
                    {
                        elementPanes[i].Visibility = System.Windows.Visibility.Visible;
                    }
                }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            InitiateSearch();
        }

        private void cmbColour_Mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbColour_Mode.SelectedIndex)
            {
                case 0:
                    InitiateSeriesMode();
                    break;
                case 1:
                    InitiateAtomicMassMode();
                    break;
                case 2:
                    InitiateAtomicNumberMode();
                    break;
                case 3:
                    InitiatePaulingScale();
                    break;
                case 4:
                        InitiateFirstIonisation();
                        break;
            }
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    try
                    {
                        if (!(cmbColour_Mode.SelectedIndex <= 1))
                        {
                            cmbColour_Mode.SelectedIndex -= 1;
                        }
                        else
                        {
                            cmbColour_Mode.SelectedIndex = 1;
                        }

                    }
                    catch
                    {

                    }
                    break;
                case Key.Down:
                    try
                    {
                        cmbColour_Mode.SelectedIndex += 1;
                    }
                    catch
                    {

                    }
                    break;
            }
        }

    }
}
