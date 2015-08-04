using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media;
namespace MyChem_Program
{
    public static class Cache
    {
        public static List<Equation> equationsSelected = new List<Equation>();
        public static List<Molecule> moleculesSelected = new List<Molecule>();
        public static List<Element> elementsSelected = new List<Element>();
        public static string defaultMoleculeFolder;
        public static string defaultGravityFolder;
        public static bool autoCopy;
        public static bool helpEnable;
        public static List<Type> userControlsOpen = new List<Type>();
        public static decimal smallestElectronegativity = 9999;
        public static decimal largestElectronegativity = -9999;
        public static decimal smallestFirstIonisation = 999999999999;
        public static decimal largestFirstIonisation = -999999999999;
        public const string MainWindowTitle = "Science Talent Search - MySci";
        public static List<ModuleTabItem> draggedTabs = new List<ModuleTabItem>();
        public static double sWidth = SystemParameters.PrimaryScreenWidth;
        public static double sHeight = SystemParameters.PrimaryScreenHeight;
        public static UnitPrefix SelectionIndexToUnitPrefix(int i)
        {
            switch(i)
            {
                case 6:
                    return UnitPrefix.p;
                case 5:
                    return UnitPrefix.n;
                case 4:
                    return UnitPrefix.µ;
                case 3:
                    return UnitPrefix.m;
                case 2:
                    return UnitPrefix.none;
                case 1:
                    return UnitPrefix.k;
                case 0:
                    return UnitPrefix.M;
            }
                return UnitPrefix.p;
    }
        public static bool CheckIsNumeric(string sender)
        {
            decimal result;
            bool dot = sender.IndexOf(".") < 0 && sender.Equals(".") && sender.Length > 0;
            if (!(Decimal.TryParse(sender, out result) || dot))
            {
                return true;
            }
            return false;
        }
        public static int NoOpened(Type userControlType)
        {
            int count = 0;
            for (int i = 0; i < userControlsOpen.Count(); i++)
                if (userControlsOpen[i] == userControlType) { count++; }
            return count;
        }
        public static ElementPane[] panesSelected(TableMode modeT)
        {
            ElementPane[] elePanesT = new ElementPane[elementsSelected.Count()];
            for (int i = 0; i < elementsSelected.Count(); i++)
            {
                elePanesT[i] = new ElementPane(modeT,elementsSelected[i].AtomicNumber-1);
            }
            return elePanesT;
        }
    }
}
