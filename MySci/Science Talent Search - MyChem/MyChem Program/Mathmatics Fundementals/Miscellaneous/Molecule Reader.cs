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
    static class MoleculeFileManager
    {
        
        public static Molecule ReadMolecule(string path)
        {
             // TEXT LOOKS LIKE:
             // C,6
             // H,12
             // O,6
             // Which results in: C6H12O6
            string[] dtbAtomicTemp = File.ReadAllLines(path);
            List<LiveElement> elementList = new List<LiveElement>();
            string[][] splited = new string[dtbAtomicTemp.Count()][];
            for (int i = 0; i < dtbAtomicTemp.Count(); i++)
            {
                splited[i] = dtbAtomicTemp[i].Split(',');
                Element temp = null;
                for (int o = 0; o < PeriodicTable.elements.Count();o++)
                {
                    if (splited[i][0] == PeriodicTable.elements[o].symbol)
                    {
                        temp = PeriodicTable.elements[o];
                    }
                }
                int amount = Convert.ToInt16(splited[i][1]);
                elementList.Add(new LiveElement(temp,amount));
            }
            return new Molecule(elementList.ToArray());
        }
        public static void SaveMolecule(Molecule molecule, string path)
         {
             using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
             {
                 foreach (LiveElement lElement in molecule.elementList)
                 {
                     file.WriteLine(lElement.element.symbol+","+lElement.amount.ToString());
                     
                 }
             }
         }
    }
}
