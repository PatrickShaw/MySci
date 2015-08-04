using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChem_Program
{
    public static class MyMath
    {
        public static bool Between(int value, int min, int max)
        {
            if (value > min && value < max) { return true; }
            return false;
        }
        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
        public static int GCD(int[] numbers)
        {
            int gcd = 0;
            int a = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                gcd = GCD(a, numbers[i]);
                a = numbers[i];
            }

            return gcd;
        }
    }
    public struct Coord
    {
        public int x;
        public int y;
        public static bool operator ==(Coord tCoord, Coord oCoord)
        {
            if (tCoord.x == oCoord.x && tCoord.y == oCoord.y) { return true; }
            return false;
        }
        public static bool operator !=(Coord tCoord, Coord oCoord)
        {
            if (tCoord.x != oCoord.x || tCoord.y != oCoord.y) { return true; } else { return false; }
        }
        public static Coord operator +(Coord tCoord, Coord oCoord)
        {
            tCoord.x += oCoord.x;
            tCoord.y += oCoord.y;
            return tCoord;
        }
        public static Coord operator -(Coord tCoord, Coord oCoord)
        {
            return new Coord(tCoord.x - oCoord.x, tCoord.y - oCoord.y);
        }
        public Coord(int xT, int yT)
        {
            x = xT;
            y = yT;
        }
    }
}