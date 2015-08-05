using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
namespace MyChem_Program.Miscellaneous
{
    public static class ColourFunctions
    {
        public static Color FindAverage(params Color[] colours)
        {
            byte R = 0;
            byte G = 0;
            byte B = 0;
            foreach(Color colour in colours)
            {
                R += colour.R;
                G += colour.G;
                B += colour.B;
            }
            R = (byte)((int)R /colours.Count());
            G /= (byte)((int)G/colours.Count());
            B /= (byte)((int)B/colours.Count());
            return Color.FromRgb(R,G,B);
        }
    }
}
