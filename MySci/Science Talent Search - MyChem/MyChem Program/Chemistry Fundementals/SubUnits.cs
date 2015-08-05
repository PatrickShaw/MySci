using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MyChem_Program
{
    public enum UnitPrefix : ulong 
    {
        M =     1000000000000000000,
        k =     1000000000000000,
        none =  1000000000000,
        m =     1000000000,
        µ =     1000000,
        n =     1000,
        p =     1
    }
public enum BaseUnit
{
    g,
    atoms,
    mol,
    L,
    error
}
    public static class StringReader
{
        //


}
    public class SubUnit
    {
        public UnitPrefix unitPrefix;
        public BaseUnit baseUnit;
        public int power;
        public SubUnit(UnitPrefix  unitPrefixT,BaseUnit baseUnitT, int powerT)
        {
            unitPrefix = unitPrefixT;
            baseUnit = baseUnitT;
            power = powerT;
        }
        public override string ToString()
        {
            string sTemp = "";
            return sTemp+baseUnit.ToString()+StringManipulation.ConvertToSuperscript(power,true);
        }
        // Use this if you are dividing
        public SubUnit invert()
        {
            return new SubUnit(unitPrefix, baseUnit, -power);
        }
        public static SubUnit operator /(SubUnit me, SubUnit you)
        {
            return new SubUnit(me.unitPrefix,me.baseUnit, me.power - you.power);
        }
        public static SubUnit operator *(SubUnit me, SubUnit you)
        {
            return new SubUnit(me.unitPrefix,me.baseUnit, me.power + you.power);
        }
        // Use this for multiplication and division
        public static bool operator ==(SubUnit me, SubUnit you)
        {
            if (me.baseUnit == you.baseUnit) { return true; }
            return false;
        }
        // Use this for addition and subtraction validation
        public override bool Equals(object obj)
        {
            if (((SubUnit)obj).baseUnit == baseUnit && ((SubUnit)obj).power == power) { return true; }
            return false;
        }
        public static bool operator !=(SubUnit me, SubUnit you)
        {
            if (me.baseUnit != you.baseUnit) { return true; } 
               return false;
        }
    
    }

}
