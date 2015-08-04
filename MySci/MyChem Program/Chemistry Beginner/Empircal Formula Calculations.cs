using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
namespace MyChem_Program
{
    public class EmpiricalElement
    {
        public EmpiricalElement(decimal percentageT,Element elementT)
        {

            percentage = percentageT;
            element = elementT;
        }
       public decimal percentage;
       public Element element;
    }
    public static class EmpiricalCalc
    {
        public static List<Molecule> FindEmpiricalCandidates(int passes =6, decimal thresholdConstant = 0.2M,params EmpiricalElement[] eles)
        {
            List<Molecule> candidates = new List<Molecule>();
            decimal threshold = thresholdConstant * eles.Count();

            
            decimal lowestMol = 99999999999;
            foreach(EmpiricalElement ele in eles) 
            {
                ele.percentage /= ele.element.atomicMass.TotalAmount;
                if (ele.percentage < lowestMol) { lowestMol = ele.percentage;}
            }
            foreach(EmpiricalElement ele in eles)
            {
                ele.percentage /= lowestMol;
            }
            for (int i = 1; i < passes;i++ )
            {
            Begin:
                if (i >= passes) { break; }

                decimal errorMargin = 0;
            EmpiricalElement[] elesCopy = new EmpiricalElement[eles.Count()];
            for (int q = 0; q < elesCopy.Count(); q++ )
            {
                elesCopy[q] = new EmpiricalElement(eles[q].percentage, eles[q].element);
            }

                foreach (EmpiricalElement ele in elesCopy)
                {
                    ele.percentage *= (decimal)i;
                    decimal errorAmmount = (decimal)Math.Abs((decimal)(Math.Round(ele.percentage,0)) - ele.percentage);
                    
                    if (errorMargin > thresholdConstant) { i++; goto Begin; }
                    errorMargin += errorAmmount;
                }

                if (errorMargin > threshold) { i++; goto Begin; }
                Molecule moleculeCandidate = new Molecule();
                foreach (EmpiricalElement ele in elesCopy)
                {
                    moleculeCandidate.Add(new LiveElement(ele.element, Convert.ToInt32(Math.Round(ele.percentage,0))));
                }
                candidates.Add(moleculeCandidate);
            }
            if (candidates.Count() <= 0)
            {
                MessageBox.Show("No empirical formula candidates were found.");
            }
            return candidates;
        }
        public static Molecule PickCandidate(List<Molecule> candidates)
        {
            if (candidates.Count() == 0) { MessageBox.Show("Could not find empirical formula."); return null; }
            return candidates[0];
        }
    }
}
