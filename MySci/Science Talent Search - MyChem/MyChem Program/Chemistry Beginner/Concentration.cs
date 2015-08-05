using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyChem_Program
{
    class gramLitreConcentration
    {
        public Molecule molecule;
        public Unit concentration;
        public gramLitreConcentration(decimal concentrationValueT, Molecule moleculeT, UnitPrefix gramsPrefix, UnitPrefix litrePrefix)
        {
            molecule = moleculeT;
            concentration = new Unit(concentrationValueT, new SubUnit(gramsPrefix, BaseUnit.g, 1), new SubUnit(litrePrefix, BaseUnit.L, -1));
        }
        public static explicit operator MolarConcentration(gramLitreConcentration gLConcentration)
        {
            Unit MConcentration = new Unit(0.0M, new SubUnit(gLConcentration.concentration.subUnits[0].unitPrefix, BaseUnit.mol, 1), new SubUnit(gLConcentration.concentration.subUnits[1].unitPrefix, BaseUnit.L, -1));
            MConcentration.TotalAmount += gLConcentration.concentration.TotalAmount;
            MConcentration /= gLConcentration.molecule.GetMolecularMass();
            return new MolarConcentration(MConcentration.TotalAmount, gLConcentration.molecule, MConcentration.subUnits[0].unitPrefix, MConcentration.subUnits[1].unitPrefix);
        }
    }
    class MolarConcentration
    {
        public Molecule molecule;
        public Unit concentration;
        public MolarConcentration(decimal concentrationValueT,Molecule moleculeT, UnitPrefix molPrefix, UnitPrefix litrePrefix)
        {
            molecule = moleculeT;
            concentration = new Unit(concentrationValueT, new SubUnit(molPrefix, BaseUnit.mol, 1), new SubUnit(litrePrefix, BaseUnit.L, -1));
        }
        public static explicit operator gramLitreConcentration(MolarConcentration MConcentration)
        {
            Unit gLitreConcentration = new Unit(0.0M, new SubUnit(MConcentration.concentration.subUnits[0].unitPrefix, BaseUnit.g, 1), new SubUnit(MConcentration.concentration.subUnits[1].unitPrefix, BaseUnit.L, -1)) ;
            gLitreConcentration.TotalAmount += MConcentration.concentration.TotalAmount;
            gLitreConcentration *= MConcentration.molecule.GetMolecularMass();
            return new gramLitreConcentration(gLitreConcentration.TotalAmount, MConcentration.molecule , gLitreConcentration.subUnits[0].unitPrefix, gLitreConcentration.subUnits[1].unitPrefix);
        }
    } 
}
