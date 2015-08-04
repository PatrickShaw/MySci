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
using System.ComponentModel;
namespace MyChem_Program
{
    /// <summary>
    /// Interaction logic for ElementPane.xaml
    /// </summary>
    public partial class ElementPane : UserControl, INotifyPropertyChanged
    {
        public TableMode mode;
        private decimal percentage= 100.0M;
        public Action OnChangeValue = null;
        public decimal Percentage
        {
            get { return percentage; }
            set { percentage = value; try { OnChangeValue(); } catch { } }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public bool selected = false;
        protected int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        protected Color colourDefault = Color.FromRgb(0,0,0);

        public Brush BrushDefault
        {
            get { return new SolidColorBrush(colourDefault); }
            set
            {
                colourDefault = ((SolidColorBrush)value).Color;
                grid.Background = BrushDefault;
            }
        }

        protected string atomicMass;
        public string AtomicMass
        {
            get { return atomicMass; }
            set { atomicMass = value; OnPropertyChanged("AtomicMass"); }
        }

        protected string atomicNumber;
        public string AtomicNumber
        {
            get { return atomicNumber; }
            set { atomicNumber = value; OnPropertyChanged("AtomicNumber"); }
        }

        protected string name;
        public string eleName
        {
            get { return name; }
            set { name = value; OnPropertyChanged("eleName"); }
        }

        protected string symbol;
        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; OnPropertyChanged("Symbol"); }
        }

        public ElementPane(TableMode modeT,int indexT)
        {

            InitializeComponent(); 
            mode = modeT;
            index = indexT;
            AtomicMass = decimal.Parse(Math.Round(PeriodicTable.elements[index].atomicMass.TotalAmount, 2).ToString()).ToString("G29");
            AtomicNumber = PeriodicTable.elements[index].AtomicNumber.ToString();
            Symbol = PeriodicTable.elements[index].symbol;
            switch (mode)
            {
                case TableMode.selection:
                case TableMode.information:
                case TableMode.display:
                    eleName = PeriodicTable.elements[index].name;
                    break;
                case TableMode.standard:
                    Percentage = 1;
                    eleName = Percentage.ToString();
                    lblName.FontSize = 11;
                    lblName.FontWeight = FontWeights.Bold;
                    break;
                case TableMode.empirical:
                    eleName = Percentage.ToString();
                    lblName.FontSize = 11;
                    lblName.FontWeight = FontWeights.Bold;
                    eleName += "%";
                    break;
            }
            BrushDefault = new SolidColorBrush(Element.GetElementColour(index));
        }

        protected Color textColour =Color.FromRgb(0,0,0);
        public System.Windows.Media.Brush TextColour
        {
            get { return new SolidColorBrush(textColour); }
            set { textColour = ((SolidColorBrush)value).Color; }
        }
        public System.Windows.Media.Brush TextBrush
        {
            get { return new SolidColorBrush(textColour); }
            set { }
        }




        public void PreSelectionHighlight()
        {
            grid.Background = new SolidColorBrush(Color.FromRgb(CalcPreSelVal(colourDefault.R), CalcPreSelVal(colourDefault.G), CalcPreSelVal(colourDefault.B)));
            TextColour = new SolidColorBrush(Color.FromRgb(150, 150, 150));
        }
        public void SelectionHighlight()
        {
            grid.Background = new SolidColorBrush(Color.FromRgb(CalcSelVal(colourDefault.R), CalcSelVal(colourDefault.G), CalcSelVal(colourDefault.B)));
            TextColour = new SolidColorBrush(Color.FromRgb(20, 20, 20));
        }
        public byte CalcSelVal(byte val)
        {
            return (byte)(val / 1.75);
        }
        public byte CalcPreSelVal(byte val)
        {
            return (byte)(val / 2);
        }
        public void Highlight()
        {
            grid.Background = new SolidColorBrush(Color.FromRgb(calcHighVal(colourDefault.R), calcHighVal(colourDefault.G), calcHighVal(colourDefault.B)));
            TextColour = new SolidColorBrush(Color.FromRgb(169, 169, 169));
        }
        public void ResetColours()
        {
            grid.Background = new SolidColorBrush(colourDefault);
            TextColour = new SolidColorBrush(Color.FromRgb(0,0,0));
        }
        protected byte calcHighVal(byte val)
        {
            int valTemp;
            valTemp = 255 - val;
            valTemp /= 3;
            val += (byte)valTemp;
            return val;
        }

        public void Element_Enter(object sender, MouseEventArgs e)
        {
            if (mode == TableMode.display) { return; }
            if (selected ) { return; }
            Highlight();
        }

        public void Element_Leave(object sender, MouseEventArgs e)
        {
            if (mode == TableMode.display) { return; }
            if (selected ) { return; }
            ResetColours();
        }
        public void Element_Down(object sender, MouseButtonEventArgs e)
        {
            if (mode == TableMode.display) { return; }
            PreSelectionHighlight();
        }
        public void Element_Up(object sender, MouseButtonEventArgs e)
        {
            if (mode == TableMode.display) { return; }

            SelectionHighlight();
            if (selected)
            {
                Highlight();
                Cache.elementsSelected.Remove(PeriodicTable.elements[index]);
                selected = false;
                return;
            }
            if (TableMode.selection != mode &&Keyboard.IsKeyDown(Key.LeftCtrl) == false && Keyboard.IsKeyDown(Key.RightCtrl) == false)
            {
                switch(mode)
                {
                    case TableMode.selection:
                        foreach (Element ele in Cache.elementsSelected)
                        {

                            Periodic_Table.elementPanes[ele.AtomicNumber - 1].selected = false;
                            Periodic_Table.elementPanes[ele.AtomicNumber - 1].ResetColours();
                        }
                        break;

                    case TableMode.information:
                    ResetColours();
                    ElementInformation eleInfoTemp = new ElementInformation(index, colourDefault);
                    eleInfoTemp.Show();
                        break;
                    case TableMode.standard:
                        ChangeLiveElementAmount changeATemp = new ChangeLiveElementAmount(index, (int)Percentage);
                        changeATemp.ShowDialog();
                        Percentage = changeATemp.GetPercentage();
                    lblName.Content = Percentage.ToString();
                        break;
                    case TableMode.empirical:
                        ChangeEmpiricalPercentage changePTemp = new ChangeEmpiricalPercentage(index, Percentage);
                    changePTemp.ShowDialog();
                    Percentage = changePTemp.GetPercentage();
                    lblName.Content = Percentage.ToString() + "%";

                    return;
                }
                Cache.elementsSelected.Clear();
            }

            selected = true;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) == false && Keyboard.IsKeyDown(Key.RightCtrl) == false && mode == TableMode.empirical)
            {
                selected = false;
            }
            Cache.elementsSelected.Add(PeriodicTable.elements[index]);
        }
        
    }
}
