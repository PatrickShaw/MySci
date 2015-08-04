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
    /// Interaction logic for MoleculePane.xaml
    /// </summary>
    public partial class MoleculePane : UserControl, INotifyPropertyChanged
    {
        public Molecule molecule;
        public MoleculePane() { }
        public enum MoleculePaneMode
        {
            display, select
        }
        MoleculePaneMode mode = MoleculePaneMode.select;
        public MoleculePane(Molecule moleculeT, MoleculePaneMode modeT = MoleculePaneMode.select)
        {
            InitializeComponent();
            molecule = moleculeT;
            mode = modeT;
            InitializeStuff();
        }
        public MoleculePane(LiveElement[] liveElementsT, MoleculePaneMode modeT = MoleculePaneMode.select)
        {
            InitializeComponent();
            mode = modeT;
            molecule = new Molecule(liveElementsT);
            InitializeStuff();
        }
        public void InitializeStuff()
        {
            txbMoleculeName.Content = molecule.ToString();
            BrushDefault = new SolidColorBrush(molecule.GetColor());
            Width = txbMoleculeName.Width + 20;
            canvas.Width = txbMoleculeName.Width + 20;
            txbMoleculeName.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));

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

        protected Color colourDefault = Color.FromRgb(0,0,0);

        public Brush BrushDefault
        {
            get { return new SolidColorBrush(colourDefault); }
            set
            {
                colourDefault = ((SolidColorBrush)value).Color;
                canvas.Background = BrushDefault;
            }
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
            if (mode == MoleculePaneMode.display) { return; }
            canvas.Background = new SolidColorBrush(Color.FromRgb(CalcPreSelVal(colourDefault.R), CalcPreSelVal(colourDefault.G), CalcPreSelVal(colourDefault.B)));
            TextColour = new SolidColorBrush(Color.FromRgb(150, 150, 150));
        }
        public void SelectionHighlight()
        {
            if (mode== MoleculePaneMode.display) { return; }
            canvas.Background = new SolidColorBrush(Color.FromRgb(CalcSelVal(colourDefault.R), CalcSelVal(colourDefault.G), CalcSelVal(colourDefault.B)));
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
            if (mode == MoleculePaneMode.display) { return; }
            canvas.Background = new SolidColorBrush(Color.FromRgb(calcHighVal(colourDefault.R), calcHighVal(colourDefault.G), calcHighVal(colourDefault.B)));
            TextColour = new SolidColorBrush(Color.FromRgb(169, 169, 169));
        }
        public void ResetColours()
        {
            canvas.Background = new SolidColorBrush(colourDefault);
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

        public void Molecule_Enter(object sender, MouseEventArgs e)
        {
            if (selected ) { return; }
            Highlight();
        }

        public void Molecule_Leave(object sender, MouseEventArgs e)
        {
            if (selected ) { return; }
            ResetColours();
        }
        public void Molecule_Down(object sender, MouseButtonEventArgs e)
        {
            PreSelectionHighlight();
        }

        private void Molecule_Up(object sender, MouseButtonEventArgs e)
        {

            if (mode == MoleculePaneMode.display) { return; }
            ResetColours();
            MoleculeInformation moleculeInfo = new MoleculeInformation(molecule);
            moleculeInfo.Show();
        }
        

    }
}
