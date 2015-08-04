using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Rainbow_Algorithm___Patrick_Shaw;
namespace MyChem_Program
{
    public enum ElementGroup
    {
        AlkaliMetal,
        AlkalineEarth,
        Lanthanide,
        Actinide,
        TransitionMetal,
        BasicMetal,
        Metalloid,
        NonMetal,
        Halogen,
        NobleGas,
        Unknown
    }
    public class Element
    {
        public decimal electronegativity;
        public decimal firstIonisation;
        private int atomicNumber;
        public Color GetPaulingScaleColor()
        {
            if (electronegativity == -1) { return Color.FromRgb(200, 200, 200); }
            //MessageBox.Show(((electronegativity - Cache.smallestElectronegativity) / (Cache.largestElectronegativity - Cache.smallestElectronegativity)).ToString());
            try
            {
                return Color.FromRgb(255, (byte)(255-(electronegativity - Cache.smallestElectronegativity) / (Cache.largestElectronegativity - Cache.smallestElectronegativity) * 255.0M), 0);
            }
            catch
            {
                return Color.FromRgb(200,200,200);
            }
        }
        public Color GetFirstIonisationColour()
        {
            if (firstIonisation == -1) { return Color.FromRgb(200, 200, 200); }
            //MessageBox.Show(((electronegativity - Cache.smallestElectronegativity) / (Cache.largestElectronegativity - Cache.smallestElectronegativity)).ToString());
            try
            {
                return Color.FromRgb(255, (byte)(255 - (firstIonisation - Cache.smallestFirstIonisation) / (Cache.largestFirstIonisation - Cache.smallestFirstIonisation) * 255.0M), 0);
            }
            catch
            {
                return Color.FromRgb(200, 200, 200);
            }
        }
        public Color GetAtomicMassColour()
        {
            return ColourAlgorithms.ColourValue(Convert.ToDouble((atomicMass.TotalAmount / 25.1M)+255.0M), 6);
        }
        public Color GetAtomicNumberColour()
        {
            return ColourAlgorithms.ColourValue(Convert.ToDouble((decimal)AtomicNumber / 10.0M +255), 6);
        }
        public int AtomicNumber
        {
            get { return atomicNumber; }
            set
            {
                atomicNumber = value;
                if (atomicNumber > 83) { radioactive = true; }
                for (int i = 0; i < periodProtonsRequired.Length; i++)
                {
                    if (atomicNumber > periodProtonsRequired[i]) { continue; } else { Period = i; break; }
                }
            }
        }
        public string symbol;
        public string name;
        public Unit atomicMass = new Unit( 0, new SubUnit(UnitPrefix.none, BaseUnit.g, 1), new SubUnit(UnitPrefix.none,BaseUnit.mol, -1));
        int period;
        public int Period
        {
            get { return period; }
            set
            {
                period = value;
                Group = groupsAllowed[period - 1][(atomicNumber - periodProtonsRequired[period - 1] - 1)];

            }
        }
        public static readonly int[] periodProtonsRequired = { 0, 2, 10, 18, 36, 54, 86, 999999 };
        static readonly int[] allGroups = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
        // Check this first if there is a group related error
        static readonly int[][] groupsAllowed = { new int[] { 1, 18 }, new int[] { 1, 2, 13, 14, 15, 16, 17, 18 }, new int[] { 1, 2, 13, 14, 15, 16, 17, 18 }, allGroups, allGroups, new int[] { 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 }, new int[] { 1, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 } };
        // Metalloids are exclusive to these coordinates
        static readonly Coord[] metalloidsCoords = { new Coord(2, 13), new Coord(3, 14), new Coord(4, 14), new Coord(4, 15), new Coord(5, 15), new Coord(5, 16), new Coord(6, 17) };
        int group;
        public int Group
        {
            get { return group; }
            set
            {
                group = value;
                switch (group)
                {
                    case 0:
                        type = ElementGroup.Lanthanide;
                        break;
                    case -1:
                        type = ElementGroup.Actinide;
                        break;
                    case 1:
                        type = ElementGroup.AlkaliMetal;
                        break;
                    case 2:
                        type = ElementGroup.AlkalineEarth;
                        break;
                    case 17:
                        type = ElementGroup.Halogen;
                        break;
                    case 18:
                        type = ElementGroup.NobleGas;
                        break;
                    default:
                        type = ElementGroup.TransitionMetal;
                        break;
                    case -2:
                        type = ElementGroup.Unknown;
                        break;
                }
                for (int i = 0; i < metalloidsCoords.Length; i++)
                {
                    if (period > metalloidsCoords[i].x && group == metalloidsCoords[i].y && type != ElementGroup.Metalloid && type != ElementGroup.Halogen) { type = ElementGroup.BasicMetal; }
                    if (period < metalloidsCoords[i].x && group == metalloidsCoords[i].y && type != ElementGroup.Metalloid && type != ElementGroup.Halogen) { type = ElementGroup.NonMetal; }
                    if (period == metalloidsCoords[i].x && group == metalloidsCoords[i].y) { type = ElementGroup.Metalloid; break; }
                }
                for (int i = 0; i < unknownCoords.Length; i++)
                {
                    if (unknownCoords[i] == new Coord(period, group)) { type = ElementGroup.Unknown; }
                }
            }
        }
        public static Color GetElementColour(int pnlNo)
        {
            switch (PeriodicTable.elements[pnlNo].type)
            {
                case ElementGroup.AlkalineEarth:
                    return Color.FromRgb(255, 255, 0);
                case ElementGroup.AlkaliMetal:
                    return Color.FromRgb(255, 174, 66);
                case ElementGroup.TransitionMetal:
                    return Color.FromRgb(173, 255, 47);
                case ElementGroup.BasicMetal:
                    return Color.FromRgb(124, 252, 0);
                case ElementGroup.Metalloid:
                    return Color.FromRgb(0, 183, 235);
                case ElementGroup.NonMetal:
                    return Color.FromRgb(173, 216, 230);
                case ElementGroup.Halogen:
                    return Color.FromRgb(255, 127, 80);
                case ElementGroup.NobleGas:
                    return Color.FromRgb(76, 80, 169);
                case ElementGroup.Lanthanide:
                    return Color.FromRgb(210, 110, 230);
                case ElementGroup.Actinide:
                    return Color.FromRgb(255, 110, 150);
                case ElementGroup.Unknown:
                    return Color.FromRgb(210, 245, 210);
            }
            return Color.FromRgb(0, 0, 0);
        }
        public Coord[] unknownCoords = { new Coord(7, 9), new Coord(7, 10), new Coord(7, 11), new Coord(7, 13), new Coord(7, 14), new Coord(7, 15), new Coord(7, 16), new Coord(7, 17), new Coord(7, 18) };
        public SubShell[] NewSubShells()
        {
            SubShell[][] ss = {          new SubShell[] { new s(1) },                                     // 1
                                                            new SubShell[] { new s(2), new p(2) },                   // 2
                                                            new SubShell[]{new s(3), new p(3), new d(3)},             // 3
                                                            new SubShell[]{new s(4), new p(4), new d(4), new f(4)},    // 4
                                                            new SubShell[]{new s(5), new p(5), new d(5), new f(5)},             // 5
                                                            new SubShell[]{new s(6), new p(6), new d(6)},                      // 6
                                                            new SubShell[]{new s(7),new p(7)}                                // 7
                                                        };
            SubShell[] SUBSHELL_ORDER = {         ss[0][0],
                                                               ss[1][0],
                                                               ss[1][1],ss[2][0],
                                                               ss[2][1],ss[3][0],
                                                               ss[2][2],ss[3][1],ss[4][0],
                                                               ss[3][2],ss[4][1],ss[5][0],
                                                               ss[3][3],ss[4][2],ss[5][1],ss[6][0],
                                                               ss[4][3],ss[5][2],ss[6][1]
                                                           };
            return SUBSHELL_ORDER;
        }

        public SubShell[] sShells;
        public ElementGroup type;
        public bool radioactive;
        public Element() { }
        public Element(int atomicNumberT, string symbolT, string nameT, decimal atomicMassT, bool radioactiveT)
        {
            radioactive = radioactiveT;
            AtomicNumber = atomicNumberT;
            symbol = symbolT;
            name = nameT;
            atomicMass.TotalAmount = atomicMassT;
            SetShells(atomicNumber);
        }
        public string GetDetails()
        {
            string sTemp = atomicNumber + ",\t\t" + name + ",\t\t" + symbol + ",\t\t" + period + ",\t\t" + group + ",\t\t" + atomicMass + ",\t\t" + type.ToString() + ",\t\t";
            foreach (SubShell sShell in sShells)
            {
                if (sShell.electrons == 0) { continue; }
                sTemp += sShell.shellNo + sShell.name.ToString() + sShell.GetElectrons();
            }
            return "{" + sTemp + "}";
        }
        public string GetElectronConfig()
        {
            string sTemp = "";
            foreach (SubShell sShell in sShells)
            {
                if (sShell.electrons == 0) { continue; }
                sTemp += sShell.shellNo + sShell.name.ToString() + sShell.GetElectrons();
            }
            return sTemp;
        }
        public void SetShells(int electrons)
        {
            sShells = NewSubShells();
            int electronsAvailable = electrons;
            int ssNo = 0;
        Begin:
            if (sShells[ssNo].IsFull()) { ssNo += 1; }
            sShells[ssNo].electrons += 1;
            electronsAvailable -= 1;
            if (electronsAvailable > 0) { goto Begin; }
        }
        public string notes = "";
    }
    public class LiveElement
    {
        public int amount;
        public Element element;
        public override string ToString()
        {
            return element.symbol + StringManipulation.ConvertToSubscript(amount,true);
        }
        public LiveElement(Element elementT, int amountT)
        {
            amount = amountT;
            element = elementT;
        }
        public static LiveElement operator +(LiveElement me, LiveElement you)
        {
            if (me.element == you.element) { return new LiveElement(me.element, me.amount + you.amount); }
            return new LiveElement(PeriodicTable.elements[0], 0);
        }
    }

}

