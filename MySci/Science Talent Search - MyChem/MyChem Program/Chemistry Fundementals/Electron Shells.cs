using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MyChem_Program
{
    public  class SubShell
    {
         public  char name;
         protected int maxElectrons;
         public int electrons;
         public int shellNo;
        public string GetElectrons()
        {
            return StringManipulation.ConvertToSuperscript(shellNo);
        }
        public bool IsFull()
        {
            if (electrons >= maxElectrons) {return true;}
            return false;
        }
    }
    public  class s: SubShell
    {
        public s(int shellNoT)
        {
            name = 's';
            maxElectrons = 2;
            shellNo = shellNoT;
        }
    }
    public  class p:SubShell
    {
        public p(int shellNoT)
        {
            name = 'p';
            maxElectrons = 6;
            shellNo = shellNoT;
        }
    }
    public  class d:SubShell
    {
        public d(int shellNoT)
        {
            name = 'd';
            maxElectrons = 10;
            shellNo = shellNoT;
        }
    }
    public  class f : SubShell
    {
        public f(int shellNoT)
        {
            name = 'f';
            maxElectrons = 14;
            shellNo = shellNoT;
        }
    }
    public class g : SubShell
    {
        public g(int shellNoT)
        {
            name = 'g';
            maxElectrons = 18;
            shellNo = shellNoT;
        }
    }
}
