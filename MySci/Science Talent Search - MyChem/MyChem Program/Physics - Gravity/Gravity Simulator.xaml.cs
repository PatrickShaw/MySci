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
namespace MyChem_Program.Physics___Gravity
{
    /// <summary>
    /// Interaction logic for Gravity_Simulator.xaml
    /// </summary>
    /// Unfortunately due to computer power limitations there are many assumptions and inaccuracies made in the gravity simulator. These include:
    /// The universe is (arguably) 3 dimensional, however, the simulator only handles gravity in 2 dimensions.
    /// No kinetic energy is lost to heat and sound in collisions.
    /// In sticky collisions, the simulator 
    /// Typically the lower the mass of a celestial body, the less round the body will be (a good example of this is an asteroid), however, the simulator assumes all celestial bodies are perfect circles.
    /// The simulator assumes all celestial bodies have the exact same density (Area = π*r^2)
    /// Celestial bodies tend to have a some form of rotational energy, the simulator does not take this into account.
    /// Scaling: http://www.universetoday.com/110060/if-the-moon-were-only-one-pixel-a-scale-model-of-the-solar-system/
    public class gProps
    {
        public const double G = 0.0000000000667384d;
        public int currentID = 0;
        public const double smallestMass = 73476730910220000000000d;
        public static double scrollIncrement = smallestMass / 100d;
        public static double keyboardIncrement = smallestMass / 10d;
        // 1 pixel is 3747800m
        // So the unit is pixel/dist
        // dist = 3747800m
        // So moving 1 pixel per second is the same as moving 3747800 metres per second.
        // Earth moves at 29800m per second so it moves 0.00795133144778269918352099898607 pixels per second
        public double pixelPerDist = 3474.8D * 1000.0D;
        public double pixelsPerMarker = 100.0d;
        public bool showADirection = false;
        public bool showVDirection = false;
        public double zoom = 1;
        public double sWidth = 0;
        public Canvas gravCanvasProp;
    }


    public partial class Gravity_Simulator : UserControl, INotifyPropertyChanged
    {
 
        public double recalcMass()
        {
            return Convert.ToDouble(txtMass.Text) *  Convert.ToDouble(Math.Pow(10.0D,Convert.ToDouble(txtPowerMass.Text)));
        }
        public void NewOrbitingParticle(Vector pPosition)
        {
            Particle pOrbiting = null;
            double rSqr = 999999999999999999999999999999999999999999999999D;
            double dXP = 0;
            double dYP = 0;
            for (int i = 0; i < part.Count; i++)
            {
                double dX = part[i].pos.X - pPosition.X;
                double dY = part[i].pos.Y - pPosition.Y;
                double dSqr = dX * dX + dY * dY;
                if (Math.Sqrt(dSqr) > part[i].shape.Width / 2.0D && dSqr < rSqr && massSetting < part[i].mass)
                {
                    pOrbiting = part[i];
                    rSqr = dSqr;
                    dXP = dX;
                    dYP = dY;
                }
            }
            if (pOrbiting != null)
            {
                // a = v^2/r
                // Therefore v=(ar)^(-1/2)
                // radius is constant
                // mass is constant
                // therfore acceleration is constant
                // velocity can change     double angle;
                double angle;
                angle = Math.Atan2(dYP, dXP);
                if (rSqr < 1600) { rSqr = 1600; }
                double force = (gProps.G * pOrbiting.mass * massSetting) / (rSqr * gProps.pixelPerDist * gProps.pixelPerDist/*Now find the distance in metres*/);
                double accel = force / massSetting; // Find acceleration in m/s/s
                double speed = Math.Sqrt(accel * Math.Sqrt(rSqr));
                Vector vel = new Vector(-speed * Math.Sin(angle), speed * Math.Cos(angle)) + pOrbiting.vel;
                AddParticle(new Particle(massSetting, vel, new Vector(pPosition.X, pPosition.Y), gProps.currentID, ref gProps));
            }
        }
        public class TouchInfo
        {
            public bool orbitAssistance = false;
           public Point start;
           public Point now;
           public Line aim;
           public Label aimSpeed;
           public TouchDevice touchDevice;
            
        }
         gProps gProps = new gProps();
         public Point mouseDownPos;
         public Point mousePosNOW;
         public Line aim;
         public Label aimSpeed;
         public double[][] constants;
         public double[][] oldR;
        public void RecalcConstants()
         {
             constants = new double[part.Count][];
             oldR = new double[part.Count][];
            for (int i = 0 ; i < part.Count; i ++)
            {
                if (part.Count == 1)
                {
                    oldR[i] = null;
                    constants[i] = null;
                    return;
                }
                oldR[i] = new double[part.Count - (i + 1)];
                constants[i] = new double[part.Count - (i + 1)];
                for(int o = i + 1; o<part.Count; o ++)
                {

                    oldR[i] = new double[part.Count - (i + 1)];
                    double dX = (part[i].pos.X - part[o].pos.X);
                    double dY = part[i].pos.Y - part[o].pos.Y;
                    double rSqr = dX * dX + dY * dY; // Finds distance in pixels
                    if (rSqr < 1600) { rSqr = 1600; }
         
                    double r = Math.Sqrt(rSqr);
                    oldR[i][(o - 1) - i] = r;
                    constants[i][(o-1)-i] = part[i].CalcConsant( part[o], gProps);
                }
            }
         }
         List<Particle> part = new List<Particle>();
        public bool border = false;
          System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
         private double mass_Setting = gProps.smallestMass * 10;
         public event PropertyChangedEventHandler PropertyChanged;
         protected void OnPropertyChanged(string name)
         {
             PropertyChangedEventHandler handler = PropertyChanged;
             if (handler != null)
             {
                 handler(this, new PropertyChangedEventArgs(name));
             }
         }
        public double massSetting
         {
             set
             {
                 mass_Setting = value;
                 try
                 {
                     string[] noOne = mass_Setting.ToString().Split('E');
                     txtMass.Text = noOne[0];
                     txtPowerMass.Text =  noOne[1]; 
                 }
                 catch
                 {
                     txtPowerMass.Text = mass_Setting.ToString();
                 }
             }
             get
             {
                 return mass_Setting;
             }
         }
         public bool mouseDown = false;
       
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Update();
        }
        //[DllImport("CGM.dll")] 
        private void CUpdate()
        {

        }
        private double ScalingFactor()
        {
            return gravityCanvas.Width / Cache.sWidth;
        }
        List<Label> labelMarks = new List<Label>();
        List<Line> lineMarks = new List<Line>();
        Line scaleLine = new Line();
        private void RecalcScale()
        {
            for(int i = 0; i < labelMarks.Count;i++)
            {
                labelMarks[i].Content = ( ScalingFactor()*i * gProps.pixelPerDist * gProps.pixelsPerMarker / (double)1000000.0d) +"Gm";
                lineMarks[i].Y2 = gravityCanvas.Height - 21.0d;
                lineMarks[i].Y1 = gravityCanvas.Height - 16.0d;
                lineMarks[i].X1 = 16 + gProps.pixelsPerMarker * i;
                lineMarks[i].X2 = 16 + gProps.pixelsPerMarker * i;
                Canvas.SetLeft(labelMarks[i],16 + gravityCanvas.Width / (labelMarks.Count - 1) * i);
                Canvas.SetBottom(labelMarks[i],26.0d);
            }

            scaleLine.X1 = 16.0d;
            scaleLine.X2 = 16+(labelMarks.Count - 1) * gProps.pixelsPerMarker;
            scaleLine.Y1 = gravityCanvas.Height - 16.0d;
            scaleLine.Y2 = gravityCanvas.Height - 16.0d;

        }
        List<Line> AccelerationDirectionList = new List<Line>();
        List<Line> VelocityDirectionList = new List<Line>();
        public void UpdateAccelerationDirections()
        {

        }
        public void UpdateVelocityDirections()
        {

        }
        double seconds = 0.001D;
        public Gravity_Simulator()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(17);
            dispatcherTimer.Start();
            txtMass.Text = mass_Setting.ToString();
            // DIALOGS
            // Save Dialog setttings
            saveDialog.DefaultExt = ".grav";
            saveDialog.Filter = "Gravity Simulator Save Files|*.grav";
            saveDialog.RestoreDirectory = false;
            saveDialog.InitialDirectory = Cache.defaultGravityFolder;

            // Open Dialog Settings
            loadDialog.DefaultExt = ".grav";
            loadDialog.Filter = "Gravity Simulator Save Files|*.grav";
            loadDialog.CheckPathExists = true;
            loadDialog.CheckFileExists = true;
            loadDialog.RestoreDirectory = false;
            loadDialog.InitialDirectory = Cache.defaultGravityFolder;

            if (gProps.gravCanvasProp != null) { gravityCanvas = gProps.gravCanvasProp; return; }
            gravityCanvas.Width = Cache.sWidth;
            gravityCanvas.Height = Cache.sHeight;
            gProps.sWidth = gravityCanvas.Width;
            int noMarkers = (int)Math.Ceiling((gravityCanvas.Width - 32d) / gProps.pixelsPerMarker);

            gravityCanvas.Children.Add(scaleLine);
            scaleLine.StrokeThickness = 2;
            scaleLine.Stroke = Brushes.White;

            for (int i = 0; i < noMarkers; i++)// Yes you don't have to noMarkers - 1. I need a marker at position 0
            {
                Label lblT = new Label();
                labelMarks.Add(lblT);
                gravityCanvas.Children.Add(lblT);


                Line lT = new Line();
                lT.Stroke = Brushes.White;
                lT.StrokeThickness = 2;
                lineMarks.Add(lT);
                gravityCanvas.Children.Add(lT);
            }
            gProps.gravCanvasProp = gravityCanvas;
            RecalcScale();
            // MOTION BLUR IMPLEMENTATION
        }
        Particle _trackedParticle = null;
        public Particle trackedParticle
        {
            get
            {
                return _trackedParticle;
            }
            set
            {

                if (_trackedParticle != null) { try { _trackedParticle.shape.Fill = Brushes.White; } catch { } }
                _trackedParticle = value;
                if (_trackedParticle == null) { lblTrackID.Content = "NA"; }
                else{lblTrackID.Content = _trackedParticle.ID.ToString();}
            }
        }
        public  void UpdateMousePOS()
        {
            //POINT pTemp;
            //GetCursorPos(out pTemp);
        
            //mousePosNOW.X = pTemp.X;
            //mousePosNOW.Y = pTemp.Y;

            mousePosNOW = Mouse.GetPosition(gravityCanvas);
            for(int i = 0;i<touchInfos.Count;i++)
            {
                touchInfos[i].now = touchInfos[i].touchDevice.GetTouchPoint(gravityCanvas).Position;
                if (touchInfos[i].orbitAssistance) { continue; }
                touchInfos[i].aim.X2 = touchInfos[i].now.X;
                touchInfos[i].aim.Y2 = touchInfos[i].now.Y;
            }
            //mousePosNOW = ConvertPixelsToUnits(pTemp.X, pTemp.Y);
        }
        bool hack = true;
        public void Update()
        {
            UpdateMousePOS();
            if (aim != null)
            {
                ChangeAim();
            }
            if (mouseDown)
            {
                mouseDownPos = mousePosNOW;
                mouseDown = false;
            }
            if ((Parent as ModuleTabItem).IsSelected)
            {
                // TODO: Fix this bug rather than using his hack.
                // BUG: Initially, the x10 section of the mass setting is in the same textbox as the rest of the value
                if(hack == true)
                {
                    massSetting += gProps.keyboardIncrement;
                    hack =false;
                }
                if (Keyboard.IsKeyDown(Key.OemPlus))
                {
                    
                    massSetting += gProps.keyboardIncrement * (massSetting/gProps.smallestMass)/6.0d;
                }
                if (Keyboard.IsKeyDown(Key.OemMinus))
                {
                    massSetting -= gProps.keyboardIncrement * (massSetting/gProps.smallestMass)/6.0d;  
                }
                if (Keyboard.IsKeyDown(Key.Up))
                {
                        MoveUser(new Vector(0,  10));
                }
                if (Keyboard.IsKeyDown(Key.Left))
                {
                        MoveUser(new Vector(10, 0));
                }
                if (Keyboard.IsKeyDown(Key.Right))
                {
                    MoveUser(new Vector(-10, 0));
                }
                if (Keyboard.IsKeyDown(Key.Down))
                {
                    MoveUser(new Vector(0,-10));
                } 
                if (Keyboard.IsKeyDown(Key.G))
                {
                    Random rnd = new Random();
                    if (chkOrbitAssistance.IsChecked == false)
                    {
                        AddParticle(new Particle((rnd.NextDouble() * mass_Setting * 2.0d) + gProps.smallestMass, new Vector(rnd.NextDouble() * 2.0d - 1.0d, rnd.NextDouble() * 2.0d - 1.0d), new Vector(rnd.NextDouble() * (double)gravityCanvas.Width, rnd.NextDouble() * (double)gravityCanvas.Height), gProps.currentID, ref gProps));
                    }
                    else
                    {
                        NewOrbitingParticle(new Vector(rnd.NextDouble() * (double)gravityCanvas.Width, rnd.NextDouble() * (double)gravityCanvas.Height));
                    }
                    RecalcConstants();
                }
            }
            Vector[] oldVelocity = new Vector[part.Count];
            Vector[] newVelocities = new Vector[part.Count];
            Vector[] a = new Vector[part.Count];  
            for (int i = 0; i < part.Count;i++)
            {
                a[i] = new Vector(0,0); 
                newVelocities[i] = new Vector(0,0); 
                
            }
            for (int i = 0; i < part.Count();i++)
            {
                oldVelocity[i] = part[i].vel;
                for (int o = i + 1; o < part.Count; o++)
                {
                    double angle;
                    double dX = part[i].pos.X - part[o].pos.X;
                    double dY = part[i].pos.Y - part[o].pos.Y;
                    angle = Math.Atan2(dY, dX);
                    double rSqr = dX * dX + dY * dY; // Finds distance in pixels
                    if (rSqr < 1600) { rSqr = 1600; }
                    //double r = Math.Sqrt(rSqr);
                    //// Change in Ek

                    //double dEk = (-(constants[i][(o - 1) - i] / r) + (constants[i][(o - 1) - i] / oldR[i][(o - 1) - i]))/*Now finds the distance in metres*/;

                    //double dYEk = Math.Sin(angle) * dEk;
                    //double dXEk = Math.Cos(angle) * dEkjerewee
                    double force = constants[i][(o - 1) - i] / (rSqr);
                    part[i].vel.Y -= Math.Sin(angle) * force / part[i].mass;
                    part[o].vel.Y += Math.Sin(angle) * force / part[o].mass;
                    part[i].vel.X -= Math.Cos(angle) * force / part[i].mass;
                    part[o].vel.X += Math.Cos(angle) * force / part[o].mass;

                    //oldR[i][(o - 1) - i] = r;
                }
            }
            for (int i = 0; i < part.Count;i++ )
            { 
                part[i].pos += 0.5D*(part[i].vel + oldVelocity[i])*seconds; 
                UpdatePosition(part[i]);
            }
                if (gProps.showADirection)
                {
                    for (int i = 0; i < part.Count; i++)
                    {

                        part[i].accelerationDirection.X1 = part[i].pos.X;
                        part[i].accelerationDirection.Y1 = part[i].pos.Y;
                        Vector acceleration = part[i].vel - oldVelocity[i];
                        Vector endPoint = part[i].pos + acceleration * 20.0D;

                        part[i].accelerationDirection.X2 = endPoint.X;
                        part[i].accelerationDirection.Y2 = endPoint.Y;
                    }
                }
                CollisionCheck();
            
            if (gProps.showVDirection)
            {
                for (int i = 0; i < part.Count; i++)
                {
                    part[i].velocityDirection.X1 = part[i].pos.X;
                    part[i].velocityDirection.Y1 = part[i].pos.Y;
                    Vector endPoint = part[i].pos + part[i].vel * 20D;
                    part[i].velocityDirection.X2 = endPoint.X;
                    part[i].velocityDirection.Y2 = endPoint.Y;
                }
            }
           if (trackedParticle != null)
           {
               Vector centre = new Vector(gravityCanvas.Width / 2.0D, gravityCanvas.Height / 2.0D);
               for (int i = 0; i < part.Count; i++)
               {
                   if (part[i].ID != trackedParticle.ID)
                   {
                       part[i].pos -= (trackedParticle.pos - centre);
                   }
               }
               trackedParticle.pos = centre;
               } 
        }
        public void CollisionCheck(bool fracturing = true)
        {
            if (chkStickyCollisions.IsChecked == true)
            {
                for (int i = 0; i < part.Count; i++)
                {
                for (int o = i + 1; o < part.Count; o++)
                {
                    // Find distance 
                    double dX = part[i].pos.X - part[o].pos.X;
                    double dY = part[i].pos.Y - part[o].pos.Y;

                    double r = Math.Sqrt(dX * dX + dY * dY);
                    if (r < (part[i].shape.Width + part[o].shape.Width) / 2.0d)
                    {
                        if (gProps.showVDirection) { gravityCanvas.Children.Remove(part[i].velocityDirection); }
                        if (gProps.showVDirection) { gravityCanvas.Children.Remove(part[o].velocityDirection); }
                        if (gProps.showADirection) { gravityCanvas.Children.Remove(part[i].accelerationDirection); }
                        if (gProps.showADirection) { gravityCanvas.Children.Remove(part[o].accelerationDirection); }   
                        smallestParticle[] collapseParts01 =  null;
                        smallestParticle[] collapseParts02 = null;
                        if ((chkFracturing.IsChecked == true && chkFracturing.IsEnabled == true)&&fracturing == true)
                        {
                            collapseParts01 = part[i].Collapse(part[o], gProps);
                           collapseParts02 = part[o].Collapse(part[i], gProps);
                        }
                        if (collapseParts01 != null)
                        {
                            AddParticleMultiple(collapseParts01);
                            for (int q = 0; q < collapseParts01.Length; q++)
                            {
                                UpdatePosition(collapseParts01[q]);
                            }
                        }
                        if (collapseParts02 != null)
                        {
                            AddParticleMultiple(collapseParts02);
                            for (int q = 0; q < collapseParts02.Length; q++)
                            {
                                UpdatePosition(collapseParts02[q]);
                            }
                        }
                        if (collapseParts01 == null || collapseParts02 == null)
                        { 
                            double newMass = part[i].mass + part[o].mass;
                            Vector newVel = new Vector((part[o].vel.X * part[o].mass + part[i].vel.X * part[i].mass) / newMass, (part[o].vel.Y * part[o].mass + part[i].vel.Y * part[i].mass) / newMass) ;

                            double r1 = part[i].shape.Width / 2;
                            double r2 = part[o].shape.Width / 2;

                            double newDiametre = Math.Sqrt(r1*r1 + r2*r2)*2;
                            double percentMass = part[i].mass / newMass;
                            Vector newPos = part[i].pos * percentMass + part[o].pos*(1 - percentMass); 
                            if (part[i].mass > part[o].mass)
                            {
                                if (part[o] is smallestParticle)
                                {
                                    AddParticle(new smallestParticle(newMass, newVel, newPos, gProps.currentID, gProps, newDiametre, part[i].shape));
                                }
                                else
                                {
                                    AddParticle(new Particle(newMass, newVel, newPos, gProps.currentID, gProps,newDiametre,part[i].shape));
                                }
                            }
                            else
                            {
                                if (part[i] is smallestParticle )
                                {
                                    AddParticle(new smallestParticle(newMass, newVel, newPos, gProps.currentID, gProps, newDiametre, part[i].shape));    
                                }
                                else
                                {
                                    AddParticle(new Particle(newMass, newVel, newPos, gProps.currentID, gProps, newDiametre, part[i].shape));
                                }
                            } 
                            UpdatePosition(part[part.Count - 1]); 
                        }
                        else
                        {
                            gravityCanvas.Children.Remove(part[i].shape);
                        }
                        gravityCanvas.Children.Remove(part[o].shape);
                        RemoveParticle(o);
                        RemoveParticle(i); 
                        break;

                    }
                }
            }
                for (int i = 0; i < part.Count; i++)
                {
                    for (int o = i + 1; o < part.Count; o++)
                    {
                        // Find the verticle and horizontal component of the distance in pixels
                        double dX = part[i].pos.X - part[o].pos.X;
                        double dY = part[i].pos.Y - part[o].pos.Y;

                        double r = Math.Sqrt(dX * dX + dY * dY); // Finds distance in pixels
                        if (r < (part[i].shape.Width + part[o].shape.Width) / 2.0d)
                        {
                            if (gProps.showVDirection) { gravityCanvas.Children.Remove(part[i].velocityDirection); }
                            if (gProps.showVDirection) { gravityCanvas.Children.Remove(part[o].velocityDirection); }
                            if (gProps.showADirection) { gravityCanvas.Children.Remove(part[i].accelerationDirection); }
                            if (gProps.showADirection) { gravityCanvas.Children.Remove(part[o].accelerationDirection); }
                             
                                double newMass = part[i].mass + part[o].mass;
                                Vector newVel = new Vector((part[o].vel.X * part[o].mass + part[i].vel.X * part[i].mass) / newMass, (part[o].vel.Y * part[o].mass + part[i].vel.Y * part[i].mass) / newMass);

                                double r1 = part[i].shape.Width / 2;
                                double r2 = part[o].shape.Width / 2;
                                double newDiametre = Math.Sqrt(r1 * r1 + r2 * r2) * 2;
                                double percentMass = part[i].mass / newMass;
                                Vector newPos = part[i].pos * percentMass + part[o].pos * (1 - percentMass);
                                Ellipse newEllipse = new Ellipse();
                                Random rnd = new Random();
                                if (part[i].mass > part[o].mass)
                                {
                                    if (part[o] is smallestParticle)
                                    {
                                        AddParticle(new smallestParticle(newMass, newVel, newPos, gProps.currentID, gProps,newDiametre));
                                    }
                                    else
                                    {
                                        AddParticle(new Particle(newMass, newVel, newPos, gProps.currentID, gProps,newDiametre));
                                    }

                                }
                                else
                                {
                                    if (part[i] is smallestParticle)
                                    {
                                        AddParticle(new smallestParticle(newMass, newVel, newPos, gProps.currentID, gProps,newDiametre));
                                    }
                                    else
                                    {
                                        AddParticle(new Particle(newMass, newVel, newPos, gProps.currentID, gProps,newDiametre));
                                    }
                                }
                                gravityCanvas.Children.Add(newEllipse);
                                UpdatePosition(part[part.Count - 1]); 
        
                            gravityCanvas.Children.Remove(part[i].shape);
                            gravityCanvas.Children.Remove(part[o].shape);
                            RemoveParticle(o);
                            RemoveParticle(i);
                        }



                    }
                }
                RecalcConstants();
            }

        }
        public void RemoveParticle(int i)
        {
            part.RemoveAt(i);
        }
        public void AddParticleMultiple(smallestParticle[] p)
        {
            for (int i = 0; i < p.Length ; i ++)
            {
                part.Add(p[i]);
                part[part.Count - 1].shape.MouseRightButtonUp += (s, e) => TrackParticle(p[i]);
            }
        }
        public void AddParticle(Particle p)
        {
            part.Add(p);
            part[part.Count - 1].shape.MouseRightButtonUp += (s, e) => TrackParticle(p);
        }
        public void TrackParticle( Particle p)
        {
            try
            {
                trackedParticle.shape.Fill = Brushes.White;
            }
            catch { }
            try
            {

                for (int i = 0; i < part.Count; i++)
                {
                    if (part[i].ID == p.ID)
                    {
                        trackedParticle = part[i];
                    }
                }
                trackedParticle.shape.Fill = Brushes.Yellow;
            }
            catch
            {

            }
        }
        // Update position on gravityCanvas
        public void UpdatePosition( Particle particle)
        {
            Canvas.SetLeft(particle.shape, Centre(particle.pos.X, particle));
            Canvas.SetTop(particle.shape, Centre(particle.pos.Y, particle));
        }
        public static double Centre(double length,Particle particle)
        {
            return length - particle.shape.Width / 2.0;
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        private static Point ConvertPixelsToUnits(int x, int y)
        {
            // get the system DPI
            IntPtr dDC = GetDC(IntPtr.Zero); // Get desktop DC
            int dpi = GetDeviceCaps(dDC, 88);
            bool rv = ReleaseDC(IntPtr.Zero, dDC);

            // WPF's physical unit size is calculated by taking the 
            // "Device-Independant Unit Size" (always 1/96)
            // and scaling it by the system DPI
            double physicalUnitSize = (1d / 96d) * (double)dpi;
            Point wpfUnits = new Point(physicalUnitSize * (double)x,
                physicalUnitSize * (double)y);

            return wpfUnits;
        }




        private   void ChangeAim()
        {
            aim.X2 = mousePosNOW.X;
            aim.Y2 = mousePosNOW.Y;
            Vector difference = mouseDownPos - mousePosNOW;
            Vector velocityAmount = difference / 20.0d;
            double speed = Math.Sqrt(velocityAmount.X *velocityAmount.X + velocityAmount.Y * velocityAmount.Y);
            aimSpeed.Content = (speed * gProps.pixelPerDist / 1000000.0d).ToString() +" Gm/s";
            Canvas.SetLeft(aimSpeed, aimSpeed.Width + difference.X+ mouseDownPos.X );
            Canvas.SetTop(aimSpeed, aimSpeed.Height+difference.Y + mouseDownPos.Y );
        }

        private void canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                // Good luck trying to understand this, even I can't anymore :P
                Vector oldDimensions = new Vector(gravityCanvas.Width,gravityCanvas.Height);
                Point mouse = Mouse.GetPosition(gravityCanvas);
                Vector ratio = new Vector((double)mouse.X/gravityCanvas.Width,(double)mouse.Y/gravityCanvas.Height);

                Vector origin = new Vector(gravityCanvas.Width * 0.5d, gravityCanvas.Height * 0.5d);
                if (gravityCanvas.Width + e.Delta * 1000.0d / SystemParameters.PrimaryScreenHeight > 10 && gravityCanvas.Height + e.Delta * 1000.0d / SystemParameters.PrimaryScreenWidth > 10)
                {
                    gravityCanvas.Width += e.Delta * 1000.0d / SystemParameters.PrimaryScreenHeight;
                    gravityCanvas.Height += e.Delta * 1000.0d / SystemParameters.PrimaryScreenWidth;
                    //double factor = gravityCanvas.Width / SystemParameters.PrimaryScreenWidth;
                }
                Vector delta = -origin + new Vector(gravityCanvas.Width * 0.5d, gravityCanvas.Height * 0.5d);
                Move(delta );
                
                Vector movedPixels = new Vector(oldDimensions.X - gravityCanvas.Width,oldDimensions.Y - gravityCanvas.Height);
                if (e.Delta < 0)
                {
                    Vector deltaMouseMove = new Vector(movedPixels.X * ratio.X - movedPixels.X / 2.0d, movedPixels.Y * ratio.Y - movedPixels.Y / 2.0d);
                    Move(-deltaMouseMove);
                }
                
                RecalcScale();
            }
            else
            {
                massSetting += e.Delta * gProps.scrollIncrement;
                if (massSetting < gProps.smallestMass) { massSetting = gProps.smallestMass; }
                
            }

        }
        private void Move(Vector moveAmount)
        {
            for (int i = 0; i < part.Count; i++)
                part[i].pos += moveAmount;
        }
        private void MoveUser(Vector moveAmount)
        {
            for (int i = 0; i < part.Count; i++)
                part[i].pos += moveAmount * (gravityCanvas.Width / gProps.sWidth);
        }
       
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        public void SaveSaveFile(string path)
        {
            try
            {
                List<string> sPList = new List<string>();
                sPList.Add(gravityCanvas.Width.ToString());
                sPList.Add(gravityCanvas.Height.ToString());
                for (int i = 0; i < part.Count; i ++ )
                {
                    Color colourT = ((SolidColorBrush)part[i].shape.Fill).Color;
                    sPList.Add(part[i].mass + "," + part[i].pos.X + "," + part[i].pos.Y + "," + part[i].vel.X + "," + part[i].vel.Y + "," + part[i].ID + "," + colourT.R + "," + colourT.G + "," + colourT.B+","+part[i].shape.Width);
                }
                    using (FileStream fs = File.Create(path))
                    {
                    }
                    File.AppendAllLines(path,sPList.ToArray());
            }
            catch
            {
                MessageBox.Show("Couldn't save file.");
            }
        }
        public void LoadSaveFile(string path)
        {
            try{
                            gravityCanvas.Children.Clear();
            part.Clear();
            string[] saveLines = File.ReadAllLines(path);
            gravityCanvas.Width = Convert.ToDouble(saveLines[0]);
            gravityCanvas.Height = Convert.ToDouble(saveLines[1]);
            for(int i = 2; i < saveLines.Count();i++)
            {
                //Order: Mass - > Position.x -> Position.y -> Velocity.x - Velocity.y -> ID
                string[] stringWords = saveLines[i].Split(',');
                double massT = Convert.ToDouble(stringWords[0]);
                Vector posT = new Vector(Convert.ToDouble(stringWords[1]), Convert.ToDouble(stringWords[2]));
                Vector velT = new Vector(Convert.ToDouble(stringWords[3]), Convert.ToDouble(stringWords[4]));
                int idT = Convert.ToInt16(stringWords[5]);
                byte r = Convert.ToByte(stringWords[6]);
                byte g = Convert.ToByte(stringWords[7]);
                byte b = Convert.ToByte(stringWords[8]);
                double diametreT = Convert.ToDouble(stringWords[9]);
                Particle pT = new Particle(massT, velT, posT, idT, gProps,diametreT);
                pT.shape.Fill = new SolidColorBrush(Color.FromRgb(r,g,b));
                AddParticle(pT);
                
            }
            }
            catch{            gravityCanvas.Children.Clear();
            part.Clear();
                MessageBox.Show("Could not load save file. The file could be corrupted");
            }
            RecalcConstants();
        }
        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (chkOrbitAssistance.IsChecked == true) { return; }
            if (aim != null) { return; }
            mouseDownPos = mousePosNOW;
            aim = new Line();
            aim.Stroke = Brushes.White;
            aim.StrokeThickness = 10;

            //aim.StrokeEndLineCap = PenLineCap.Triangle;
            aim.StrokeStartLineCap = PenLineCap.Triangle;
            aim.X1 = mouseDownPos.X;
            aim.Y1 = mouseDownPos.Y;
            aim.X2 = mouseDownPos.X; // Yes, it's meant to be like that
            aim.Y2 = mouseDownPos.Y;
            if (aim != null) { gravityCanvas.Children.Remove(aim); }
            if (aimSpeed != null) { gravityCanvas.Children.Remove(aimSpeed); }
            gravityCanvas.Children.Add(aim);

            aimSpeed = new Label();
            gravityCanvas.Children.Add(aimSpeed);
            ChangeAim();
        }

        public void window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(chkOrbitAssistance.IsChecked == false)
            { 
            Vector difference = mouseDownPos - mousePosNOW;
            difference /= 20.0d;
            Ellipse shapeT = new Ellipse();
            AddParticle(new Particle(massSetting, new Vector(difference.X, difference.Y), new Vector(mouseDownPos.X, mouseDownPos.Y), gProps.currentID, ref gProps));
                
            gravityCanvas.Children.Add(shapeT);
            gravityCanvas.Children.Remove(aim);
            gravityCanvas.Children.Remove(aimSpeed);
            aim = null;
            aimSpeed = null;
            }
            else
            {
                NewOrbitingParticle(new Vector(mousePosNOW.X,mousePosNOW.Y));
            } UpdatePosition(part[part.Count - 1]);
            CollisionCheck(false);
            RecalcConstants();
        }
    Random rd = new Random();

  List<TouchInfo> touchInfos = new List<TouchInfo>();
        private void workGrid_TouchDown(object sender, TouchEventArgs e)
  {
      TouchInfo infoT = new TouchInfo();
      infoT.touchDevice = e.TouchDevice;
      infoT.start = e.GetTouchPoint(gravityCanvas).Position;
      infoT.now = infoT.start;
      if (chkOrbitAssistance.IsChecked == true) 
      {
          infoT.orbitAssistance = true;
          touchInfos.Add(infoT);          
          return; }
            infoT.aim = new Line();
            infoT.aimSpeed = new Label();
            infoT.aim.X1 = infoT.start.X;
            infoT.aim.Y1 = infoT.start.Y;
            infoT.aim.X2 = infoT.now.X;
            infoT.aim.Y2 = infoT.now.Y;
            infoT.aim.Stroke = Brushes.White;
            infoT.aim.StrokeThickness = 10;
            infoT.aim.StrokeStartLineCap = PenLineCap.Triangle;
            touchInfos.Add(infoT);
            gravityCanvas.Children.Add(infoT.aim);
            //CaptureTouch(infoT.touchDevice);
            infoT.touchDevice.Capture(gravityCanvas,CaptureMode.Element);
        }

        private void workGrid_TouchUp(object sender, TouchEventArgs e)
        {
            int o = 0;
            try
            {

                Parallel.For(0, touchInfos.Count, (i, loopState) =>
                {
                    if (touchInfos[i].touchDevice.Id == e.TouchDevice.Id)
                    {
                        o = i;
                        loopState.Break();
                    }
                });
                if (chkOrbitAssistance.IsChecked == false)
                {
                    Vector difference = touchInfos[o].start - touchInfos[o].now;
                    difference /= 20.0d;
                    Ellipse shapeT = new Ellipse();
                    AddParticle(new Particle(massSetting, new Vector(difference.X, difference.Y), new Vector(touchInfos[o].start.X, touchInfos[o].start.Y), gProps.currentID, ref gProps));
                    gravityCanvas.Children.Add(shapeT);
                    gravityCanvas.Children.Remove(touchInfos[o].aim);
                    gravityCanvas.Children.Remove(touchInfos[o].aimSpeed);
                    ReleaseTouchCapture(touchInfos[o].touchDevice);
                }
                else
                {
                        NewOrbitingParticle(new Vector(touchInfos[o].now.X, touchInfos[o].now.Y));
                }
            }
            catch { }
            
                touchInfos.RemoveAt(o);
                UpdatePosition(part[part.Count - 1]);
                CollisionCheck(false);
            

        }
        private void CheckIsNumeric(TextBox sender, TextCompositionEventArgs e)
        {
            decimal result;
            bool dot = sender.Text.IndexOf(".") < 0 && e.Text.Equals(".") && sender.Text.Length > 0;
            if (!(Decimal.TryParse(e.Text, out result) || dot))
            {
                e.Handled = true;
            }
        }
        private void gravityCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice != null) // prevents touch from starting the mouse event
                e.Handled = true;
        }

        private void gravityCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice != null) // prevents touch from starting the mouse event
                e.Handled = true;
        }


        private void txtMass_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CheckIsNumeric((TextBox)sender, e);
        }

        private void txtMass_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtMass.Text == "") { return; } mass_Setting = Convert.ToDouble(txtMass.Text) * Math.Pow(10, Convert.ToDouble(txtPowerMass.Text));
            }
            catch { }
        }

        private void chkStickyCollisions_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                chkFracturing.IsEnabled = true;
            }
            catch { }
        }

        private void chkStickyCollisions_Unchecked(object sender, RoutedEventArgs e)
        {
            chkFracturing.IsEnabled = false;
        }
        private void chkShowVelocityDirection_Checked(object sender, RoutedEventArgs e)
        {
            gProps.showVDirection = true;
            for (int i = 0; i < part.Count; i++)
            {
                part[i].TurnOnVelocityDirection(gProps);
            }
        }
        private void chkShowVelocityDirection_Unchecked(object sender, RoutedEventArgs e)
        {
            gProps.showVDirection = false;
            for (int i = 0; i <part.Count; i++)
            {
                gravityCanvas.Children.Remove(part[i].velocityDirection);
            }
        }
        private void chkShowAccelerationDirection_Checked(object sender, RoutedEventArgs e)
        {
            gProps.showADirection = true;
            for (int i = 0; i < part.Count; i++)
            {
                part[i].TurnOnAccelerationDirection(gProps);
            }
        }

        private void chkShowAccelerationDirection_Unchecked(object sender, RoutedEventArgs e)
        {
            gProps.showADirection = false; 
            for (int i = 0; i < part.Count; i++)
            {
                gravityCanvas.Children.Remove(part[i].accelerationDirection);
            }
        }

        System.Windows.Forms.OpenFileDialog loadDialog = new System.Windows.Forms.OpenFileDialog();
        System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            try
            {
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) { dispatcherTimer.Start(); return; }

                if (saveDialog.FileName == "" || saveDialog.FileName == null) { dispatcherTimer.Start(); return; }
                SaveSaveFile(saveDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            dispatcherTimer.Start();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            try
            {
                if (loadDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) { dispatcherTimer.Start(); return; }
                if (loadDialog.FileName == "" || loadDialog.FileName == null) { dispatcherTimer.Start(); return; }
                LoadSaveFile(loadDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            for (int i = 0; i < lineMarks.Count; i++)
            {
                gravityCanvas.Children.Add(lineMarks[i]);
            }
            for (int i = 0; i < labelMarks.Count; i++)
            {
                gravityCanvas.Children.Add(labelMarks[i]);
            }
            gravityCanvas.Children.Add(scaleLine);
                RecalcScale();
            dispatcherTimer.Start();

        }
        private void txtPowerMass_TextInputPreview(object sender, TextCompositionEventArgs e)
        {
            int numberofminus = 0;
            int numberofdot = 0;
            foreach (char letter in txtPowerMass.Text)
            {
                if (letter == '.') { numberofdot += 1; continue; }
                if(letter == '-'){numberofminus += 1;continue;}
                if (!char.IsNumber(letter)) { e.Handled = true; }
            }
            if(numberofdot > 1){e.Handled = true;}
            if (numberofminus > 1) { e.Handled = true ;}
            double massT = recalcMass();
            if (gProps.smallestMass < massT) { e.Handled = true; }
        }
        private void txtPowerMass_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtMass.Text == "") { return; } mass_Setting = Convert.ToDouble(txtMass.Text) * Math.Pow(10, Convert.ToDouble(txtPowerMass.Text));
            }
            catch { }
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {

            WebBrowser2 wb = new WebBrowser2(new Uri(System.IO.Path.GetFullPath(@".\MySci User Documentation.htm#_Universal_Law_of")));
            UndockedModule ud = new UndockedModule(wb, "Help", 0);
            ud.Show();
        }
         
        private void txtSeconds_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt64(txtSeconds.Text) >= 100) { txtSeconds.Text = "100"; }
                if (Convert.ToInt64(txtSeconds.Text) <= 0) { txtSeconds.Text = "0"; }
            }
            catch
            {

            }
            try
            {

                seconds = Convert.ToDouble(txtSeconds.Text);
            }
            catch { }
        }

        private void btnUntrack_Click(object sender, RoutedEventArgs e)
        {
            trackedParticle = null;
        }

        private void txtSeconds_PreviewTextInput(object sender, TextCompositionEventArgs e)
        { 
        }
         
         
    }
}
