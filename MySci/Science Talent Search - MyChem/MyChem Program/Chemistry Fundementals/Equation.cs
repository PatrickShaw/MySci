using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChem_Program
{
    public class Equation
    {
        public Equation(List<LiveMolecule> reactantsT, List<LiveMolecule> productsT, bool balance = true)
        {
            reactants = reactantsT;
            products = productsT;
            if (balance) { Balance(); }
        }
        List<LiveMolecule> reactants;
        List<LiveMolecule> products;
        public void Balance() 
        { 
            
        }
        public Equation GetUnbalanced()
        {
            List<LiveMolecule> unbReactants = new List<LiveMolecule>();
            List<LiveMolecule> unbProducts = new List<LiveMolecule>();
            foreach (LiveMolecule lvMolecule in reactants)
            {
                unbReactants.Add(new LiveMolecule(lvMolecule.molecule, 1));
            }
            foreach (LiveMolecule lvMolecule in products)
            {
                unbProducts.Add(new LiveMolecule(lvMolecule.molecule, 1));
            }
            return new Equation(unbReactants,unbProducts,false);
        }
    }
}
