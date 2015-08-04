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
namespace MyChem_Program
{
    public static class PeriodicTable
    {
        public static Element[] elements;

        public struct ElementTemp
        {
            public bool radioactive;
            public int atomicNumber;
            public string symbol;
            public decimal atomicMass;
            public string name;
        }
         static PeriodicTable()
        {
            string[] electronegativitiesT = File.ReadAllLines(@".\Data\Electronegativities.dtb");
             string[] firstionisations = File.ReadAllLines(@".\Data\First Ionisations.dtb");
            string[] electronegativities = electronegativitiesT[0].Split(',');
            string[] dtbAtomicTemp = File.ReadAllLines(@".\Data\ElementDatabase.dtb");
            List<Element> elementList = new List<Element>();
            for (int i = 0; i < dtbAtomicTemp.Count(); i++)
            {
                ElementTemp eTemp = new ElementTemp();
                int propertyNo = 0;
                string stringBuilding = "";
                char cRead = new char();
                for (int o = 0; o < dtbAtomicTemp[i].Count(); o++)
                {
                    cRead = dtbAtomicTemp[i][o];
                    if (char.IsWhiteSpace(cRead))
                    {
                        if (o == 0) { continue; }
                        if (char.IsWhiteSpace(dtbAtomicTemp[i][o - 1])) { continue; }
                        switch (propertyNo)
                        {
                            case 0:
                                eTemp.atomicNumber = Convert.ToInt16(stringBuilding);
                                break;
                            case 1:
                                eTemp.name = stringBuilding;
                                if (stringBuilding.Last() == '*') { eTemp.radioactive = true; eTemp.name.Remove(eTemp.name.Count() - 1, 1); } else { eTemp.radioactive = false; }
                                break;
                            case 2:
                                eTemp.symbol = stringBuilding;
                                break;
                            case 3:
                                eTemp.atomicMass = Convert.ToDecimal(stringBuilding);
                                break;
                        } 
                        propertyNo += 1;
                        stringBuilding = "";
                        continue;
                    }
                    if (char.IsLetterOrDigit(cRead) || cRead == '.') { stringBuilding += cRead; }
                }
                elementList.Add(new Element(eTemp.atomicNumber, eTemp.symbol, eTemp.name, eTemp.atomicMass, eTemp.radioactive));
            }
            elements = elementList.ToArray();

            if (!File.Exists(@".\Data\ElementNotes.dtb"))
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\Data\ElementNotes.dtb"))
                {
                    foreach (Element ele in elements)
                    {

                        file.WriteLine("Currently no notes for this element...");
                        file.WriteLine("_");
                    }
                }
                 
             }
            string[] dtbElementNotes = File.ReadAllLines(@".\Data\ElementNotes.dtb");
            int eleNo = 0;
             for(int i = 0 ; i < dtbElementNotes.Count()-1; i++)
             {
             Begin:
                 if (dtbElementNotes[i] == "_") { eleNo += 1; i += 1; goto Begin; } 
             elements[eleNo].notes += dtbElementNotes[i]; 
             }

             for (int i = 0; i < elements.Count(); i++)
             {
                 try
                 {
                     if (firstionisations[i] == "") { elements[i].firstIonisation = -1; continue; }
                     elements[i].firstIonisation = Convert.ToDecimal(firstionisations[i]);
                     if (elements[i].firstIonisation > Cache.largestFirstIonisation) { Cache.largestFirstIonisation = elements[i].firstIonisation; }
                     if (elements[i].firstIonisation < Cache.smallestFirstIonisation) { Cache.smallestFirstIonisation = elements[i].firstIonisation; }
                 }
                 catch
                 {
                     elements[i].firstIonisation = -1;
                 }
             }
             for(int i = 0; i < elements.Count();i++)
             {
                 if (electronegativities[i] == "") { elements[i].electronegativity = -1;continue; }
                 elements[i].electronegativity  = Convert.ToDecimal(electronegativities[i]);
                 if (elements[i].electronegativity > Cache.largestElectronegativity) { Cache.largestElectronegativity = elements[i].electronegativity; }
                 if (elements[i].electronegativity < Cache.smallestElectronegativity) { Cache.smallestElectronegativity = elements[i].electronegativity;}

             }
        }
    }
}

