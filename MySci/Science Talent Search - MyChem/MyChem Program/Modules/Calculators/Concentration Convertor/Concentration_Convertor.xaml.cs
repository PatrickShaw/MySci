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
using System.Diagnostics;
using System.Windows.Threading;
namespace MyChem_Program
{
    /// <summary>
    /// Hi! So if you're reading this you might be here to check the code for the Science Talent Search.
    /// Unfortunately for you I tried to churn out as much code as possible in as little time as possible so
    /// everything will look quite messy. Sorry!
    /// </summary>
    /// TOO MYSELF: Create a sticky collision, physics simulator between two bodies (you can set the distance between two bodies, initial acceleration, initial velocity, etc)
    /// TOO MYSELF: Create a sticky collision, physics simulator between two bodies (you can set the distance between two bodies, initial acceleration, initial velocity, etc)
    /// TOO MYSELF: Create a sticky collision, physics simulator between two bodies (you can set the distance between two bodies, initial acceleration, initial velocity, etc)
    /// TOO MYSELF: Create a sticky collision, physics simulator between two bodies (you can set the distance between two bodies, initial acceleration, initial velocity, etc)
    /// TOO MYSELF: Create a sticky collision, physics simulator between two bodies (you can set the distance between two bodies, initial acceleration, initial velocity, etc)
    public partial class Concentration_Convertor : Window
    {
        //DispatcherTimer timer = new DispatcherTimer();

        SourceValue sourceValue = SourceValue.gL;
        enum SourceValue
        {
            gL,molarity,ppm
        }
        public Concentration_Convertor()
        {
            InitializeComponent();
            //timer = new System.Windows.Threading.DispatcherTimer();
            //timer.Tick += new EventHandler(Reca);
            //timer.Interval = TimeSpan.FromMilliseconds(34);
            //timer.Start();
        }


        private void btnMoleculeCreator_Click(object sender, RoutedEventArgs e)
        {
            Molecule_Creator frmMCreator = new Molecule_Creator();
            frmMCreator.ShowDialog();
            selectedMolecule.Children.Clear();
            try
            {
                selectedMolecule.Children.Add(new MoleculePane(Cache.moleculesSelected[0]));
            }
            catch
            {

            }
        }




        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }




    }
}
