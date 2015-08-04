using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;
using MyChem_Program.Physics___Gravity;
namespace MyChem_Program
{
    public class smallestParticle : Particle
    {
        public smallestParticle(double massT, Vector velT, Vector posT, int idT, gProps gProps, double diametreT)
            : base(massT, velT, posT, idT, gProps, diametreT)
        {
            //massT = shape.RenderedGeometry.GetArea() * gProps.smallestMass;
        }
        public smallestParticle(double massT, Vector velT, Vector posT, int idT, gProps gProps, double diametreT, Ellipse ellipse)
            : base(massT, velT, posT, idT, gProps, diametreT,ellipse)
        {
            //massT = shape.RenderedGeometry.GetArea() * gProps.smallestMass;
        }
        public override smallestParticle[] Collapse(Particle oMass, gProps gProps)
        {
            return null;
        }
    }
    public class Particle
    {
        public double mass;
        public Vector vel;
        public Vector pos;
        public double ID;
        public Ellipse shape;
        public Line accelerationDirection = new Line();
        public Line velocityDirection = new Line();
        // Bibliography: http://www.nature.com/nature/journal/v482/n7384/fig_tab/nature10780_F3.html
        // y = 0.7947ln(x) + 0.7893
        public static readonly int[] squares = { 1, 4, 9, 16, 25, 36, 49, 64, 81, 100, 121, 144, 169, 196 };//, 225, 256, 289, 324, 361, 400 };

        public double CalcConsant( Particle oPart, gProps gProps)
        {
            return (gProps.G * mass  * oPart.mass) / ( gProps.pixelPerDist * gProps.pixelPerDist);
        }
        public void InitiateParticle(double massT, Vector velT, Vector posT, int idT, ref gProps gProps)
        { 
            vel = velT;
            pos.X = posT.X;// - shape.Width /2.0d;
            pos.Y = posT.Y; //- shape.Height / 2.0d;
            if (gProps.showADirection) { TurnOnAccelerationDirection(gProps); }
            if (gProps.showVDirection) {   TurnOnVelocityDirection(gProps); }
            ID = idT;
            gProps.currentID += 1;
        }
        public Particle(double massT, Vector velT, Vector posT, int idT,ref gProps gProps)
        {
            mass = massT;
            shape = new Ellipse();
            shape.Fill = Brushes.White;
            InitiateParticle(massT, velT, posT, idT, ref gProps);
            RecalcDiametre();
            gProps.gravCanvasProp.Children.Add(shape);
        }
        public Particle(double massT, Vector velT, Vector posT, int idT, gProps gProps, double diametreT, Ellipse ellipse)
        {
            shape = ellipse;
            shape.Width = diametreT;


            shape.Height = diametreT;
            shape.Fill = Brushes.White;
            mass = massT;
            InitiateParticle(massT, velT, posT, idT, ref gProps);
            RecalcDiametre();
        }
        public Particle(double massT, Vector velT, Vector posT, int idT, gProps gProps, double diametreT)
        {
            mass = massT;
            shape = new Ellipse();
            shape.Fill = Brushes.White;
            InitiateParticle(massT, velT, posT, idT, ref gProps);
            shape.Width = diametreT;
            shape.Height = diametreT;
            gProps.gravCanvasProp.Children.Add(shape);
        }
        public void TurnOnAccelerationDirection(gProps gProps)
        {
            Line lineTemp = new Line();
            lineTemp.StrokeThickness = shape.Width / 2.0D;
            lineTemp.StrokeEndLineCap = PenLineCap.Triangle;
            lineTemp.Stroke = Brushes.Red;
            Canvas.SetZIndex(lineTemp, 9999);
            accelerationDirection = lineTemp;
            gProps.gravCanvasProp.Children.Add(lineTemp);
        }
        public void TurnOnVelocityDirection(gProps gProps)
        {
            Line lineTemp = new Line();
            lineTemp.StrokeThickness = shape.Width / 2.0d;
            lineTemp.StrokeEndLineCap = PenLineCap.Triangle;
            lineTemp.Stroke = Brushes.Magenta;
            velocityDirection = lineTemp;
            gProps.gravCanvasProp.Children.Add(lineTemp);
        }

        public virtual smallestParticle[] Collapse(Particle oPart, gProps gProps)
        {
            int noOfParticles = 1;

            for (int i = 0; i < squares.Length; i++)
            {
                noOfParticles = squares[i];
                if (gProps.smallestMass > mass / (double)noOfParticles)
                {
                    break;
                }
            }
            int squaredNumber = (int)Math.Sqrt(noOfParticles);
            // DETERMINE IF IT SHOULD FRACTURE
            double percentMass = mass / (mass + oPart.mass);
            if (percentMass > 0.95d) { return null; }
            // THE REST OF IT  
            smallestParticle[][] newParticles = new smallestParticle[squaredNumber][];
            smallestParticle[] result = new smallestParticle[noOfParticles];
            double widthPerPart = shape.Width / squaredNumber;
            double massPerPart = mass / (double)noOfParticles;
            for (int i = 0; i < squaredNumber; i++)
            {
                newParticles[i] = new smallestParticle[squaredNumber];
                for (int o = 0; o < squaredNumber; o++)
                {
                    //newParticles = new Particle(mass / (double)noOfParticles);
                    newParticles[i][o] = new smallestParticle(massPerPart, vel, new Vector(pos.X + widthPerPart * i - (shape.Width / 2.0d), pos.Y + widthPerPart * o - (shape.Width / 2.0d)), gProps.currentID, gProps, shape.Width / (double)squaredNumber);

                }

            }
            int rawr = 0;
            for (int i = 0; i < squaredNumber; i++)
            {
                for (int o = 0; o < squaredNumber; o++)
                {
                    result[rawr] = newParticles[i][o];
                    rawr++;
                }
            } 
            return result;
        }
        public void RecalcDiametre()
        {
            shape.Width = 2.0d * Math.Sqrt((mass / gProps.smallestMass) / Math.PI);//Math.Log(mass / 300000000.0d + 1, 1.00004) / 12000.0d;
            shape.Height = shape.Width;
        }
    }
}
