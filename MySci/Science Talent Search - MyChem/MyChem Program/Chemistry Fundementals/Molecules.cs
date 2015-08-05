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
    public class LiveMolecule
    {
        public LiveMolecule(Molecule moleculeT, int amountT)
        {
            amount = amountT;
            molecule = moleculeT;
        }
        public override string ToString()
        {
            return amount.ToString() + molecule.ToString();
        }
        public int amount;
        public Molecule molecule;
    }
    public class Molecule
    {

        public List<LiveElement> elementList = new List<LiveElement>();
        public Molecule(params LiveElement[] liveElements)
        {
            elementList.AddRange(liveElements);
        }
        public void Add(params LiveElement[] eles)
        {
            elementList.AddRange(eles);
        }
        public Color GetColor()
        {
            int atomsInTotal = 0;
            int R = 0;
            int G = 0;
            int B = 0;
            foreach (LiveElement element in elementList)
            {
                atomsInTotal += element.amount;
                Color colourTemp= Element.GetElementColour(element.element.AtomicNumber-1);
                R += colourTemp.R * element.amount;
                G += colourTemp.G * element.amount;
                B += colourTemp.B * element.amount;
            }
            R /= atomsInTotal;
            G /= atomsInTotal;
            B /= atomsInTotal;
            return Color.FromRgb((byte)R, (byte)G, (byte)B);
        }
        // Eg. C6H12O6.GetAtoms() = 24
        public int GetAtoms()
        {
            int amountTemp = 0;
            for(int i = 0 ; i < elementList.Count; i++)
            {
                amountTemp += elementList[i].amount;
            }
            return amountTemp;
        }
        public Unit GetMass(Unit mol)
        {
            return GetMolecularMass()*mol;
        }
        public Unit GetMolecularMass(UnitPrefix gramsUnitPrefix = UnitPrefix.none, UnitPrefix molUnitPrefix = UnitPrefix.none)
        {
            Unit grams = new Unit(0, new SubUnit(gramsUnitPrefix, BaseUnit.g, 1), new SubUnit(molUnitPrefix, BaseUnit.mol, -1));
            for (int i = 0; i < elementList.Count; i++)
            {
                grams.TotalAmount += elementList[i].amount * elementList[i].element.atomicMass.TotalAmount;
            }
            return grams;
        }
        public override string ToString()
        {
            string sTemp = "";
            for(int i = 0; i < elementList.Count();i++)
            {
                sTemp += elementList[i].ToString();
            }
            return sTemp;
        }
        public Molecule EmpiricalFormula()
        {
            Molecule moleculeTemp = new Molecule();
            int[] eleAmounts = new int[elementList.Count()];
            for(int i = 0;i< eleAmounts.Count();i++)
            {
                eleAmounts[i] = elementList[i].amount;
            }

            if (eleAmounts.Count() > 1) { 
                int temp = MyMath.GCD(eleAmounts);
                bool breakif = false;
                    for (int i = 0; i < eleAmounts.Count(); i ++ )
                        if (eleAmounts[i] < temp) { breakif = true; }
                if (!breakif)
                {
                        for (int i = 0; i < eleAmounts.Count(); i++)
                        {
                            eleAmounts[i] = eleAmounts[i] / temp;
                        }
                }
            }
            
            for(int i = 0;i<eleAmounts.Count();i++)
            {
                moleculeTemp.Add(new LiveElement(elementList[i].element,eleAmounts[i]));
            }
            return new Molecule(moleculeTemp.elementList.ToArray());
        }
        // Eg. C6H12O6.NumberOfElement(Carbon) = 6
        public int NumberOfElement(Element ele)
        {
            int TotalAmount = 0;
            for(int i = 0; i<elementList.Count();i++)
            {
                if (ele.AtomicNumber == elementList[i].element.AtomicNumber) { TotalAmount += elementList[i].amount; }
            }
            return TotalAmount;
        }
    }

}

