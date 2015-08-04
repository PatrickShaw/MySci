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
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;
namespace MyChem_Program
{
    class BODY
    {
        Vector a, v, x; // acceleration, velocity, displacement
        double d; // distance
        double friction; //friction
        double m; //mass

        // SPEED AND VELOCITY
        //public double Speed
        //{
        //    get { return v; }
        //    set { return; }
        ////}

        // FORCE, MOMENTUM AND WORK DONE
        public double WorkDone()
        {
            return 0;
        }
        public Vector SumForce()
        {
            return a*m; 
        }
        public double FrictionForce()
        {
            return 0;
        }
        public double KineticEnergy()
        {
            return 0.5D * m * v * v;
        }
        public void Update(double secondsPerIteration)
        {

        }
    }
    class CompressionSpring
    {
        double k;
        double l;
        double effeciency;
        public void Rebound(BODY body)
        {

        }
    }
    class gBody
    {

    }
}
