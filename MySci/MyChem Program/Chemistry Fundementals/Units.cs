using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MyChem_Program
{

    public class Unit
    {
        public List<SubUnit> subUnits = new List<SubUnit>();

        public decimal TrueStandardFactor()
        {
            decimal standardizedFactorT = 1;
            for (int i = 0; i < subUnits.Count; i++)
                standardizedFactorT *= (decimal)Math.Pow((double)subUnits[i].unitPrefix / (double)UnitPrefix.none, (double)subUnits[i].power);
            return standardizedFactorT;
        }
        public decimal StandardizeFactor()
        {
                            decimal standardizedFactorT = (decimal)UnitPrefix.none;
                for (int i = 0; i < subUnits.Count; i++)
                    standardizedFactorT *= (decimal)subUnits[i].unitPrefix / (decimal)UnitPrefix.none;
                return standardizedFactorT;
        }
        public decimal UnstandardFactor()
        {
            decimal standardizedFactorT = (decimal)UnitPrefix.none;
            for (int i = 0; i < subUnits.Count; i++)
                standardizedFactorT *= (decimal)subUnits[i].unitPrefix;
            return standardizedFactorT;
        }
        public string PrintAmount()
        {
            return decimal.Parse((totalAmount ).ToString()).ToString("G29");
        }
        private decimal totalAmount = 0;
        //WARNING: 0.1nm will print out as 0.0000000000000000000001m or something
        public decimal TotalAmount
        {
            get
            {
                return StandardizeFactor() * totalAmount;
            }
            set 
            {
                totalAmount = value / StandardizeFactor();
            }
        }
        public Unit(decimal amount,params SubUnit[] subUnitsT)
        {
            TotalAmount = amount;
            subUnits.AddRange(subUnitsT);
                subUnits.Sort((x, y) => string.Compare(x.baseUnit.ToString(), y.baseUnit.ToString()));
        }
        public override string ToString()
        {
            string sTemp = "";
            sTemp += decimal.Parse(TotalAmount.ToString("G29")) + " ";
            for (int i = 0; i < subUnits.Count();i++ )
            {
                if (subUnits[i].unitPrefix != UnitPrefix.none)
                    sTemp += subUnits[i].unitPrefix.ToString() ;
                sTemp += subUnits[i].ToString();
            }
                return sTemp;
        }
        public static bool operator ==(Unit me, Unit you)
        {
            try{
            for(int i = 0; i<me.subUnits.Count();i++)
                if (me.subUnits[i] != you.subUnits[i]) { return false; }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool operator !=(Unit me, Unit you)
        {
            try
            {
                for (int i = 0; i < me.subUnits.Count(); i++)
                    if (me.subUnits[i] == you.subUnits[i]) { return false; }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static Unit operator +(Unit me, Unit you)
        {
            Unit newUnit = me;
            newUnit.TotalAmount += you.TotalAmount;
            return  me;
        }
        public static Unit operator -(Unit me, Unit you)
        {
            Unit newUnit = me;
            newUnit.TotalAmount -= you.TotalAmount;
            return newUnit;
        }
        public static Unit operator *( Unit  me, decimal you)
        {
            Unit newUnit = me;
            newUnit.TotalAmount *= you;
            return newUnit;
        }

        public static Unit operator *(Unit me, Unit you)
        {
            List<SubUnit> subunitsList = new List<SubUnit>();
            for (int i = 0; i < me.subUnits.Count;i++ )
            {
                subunitsList.Add(me.subUnits[i]);
            }
            for (int i = 0; i < you.subUnits.Count;i++ )
            {
                for(int o = 0; o < subunitsList.Count; o++)
                {
                    if (you.subUnits[i] == subunitsList[o])
                    {
                        subunitsList[o].power += you.subUnits[i].power;
                    }
                }
            }
            return new Unit(me.TotalAmount * you.TotalAmount, subunitsList.ToArray());
        }
        public static Unit operator /(Unit me, Unit you)
        {
            List<SubUnit> subunitsList = new List<SubUnit>();
            for (int i = 0; i < me.subUnits.Count; i++)
            {
                subunitsList.Add(me.subUnits[i]);
            }
            for (int i = 0; i < you.subUnits.Count; i++)
            {
                for (int o = 0; o < subunitsList.Count; o++)
                {
                    if (you.subUnits[i] == subunitsList[o])
                    {
                        subunitsList[o].power -= you.subUnits[i].power;
                    }
                }
            }
            return new Unit(me.TotalAmount / you.TotalAmount, subunitsList.ToArray());
        }
    }

}
