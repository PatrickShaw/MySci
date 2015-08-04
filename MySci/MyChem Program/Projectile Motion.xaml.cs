using System;
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
using System.Windows.Threading;

namespace MyChem_Program
{
    /// <summary>
    /// Interaction logic for Projectile_Motion.xaml
    /// </summary>
    public class PBody
    {
        public decimal mass = 1;
        public Vector pos = new Vector(0, 0);
        public Vector vel = new Vector(0,0);
        public Vector acc = new Vector(0,0);
        public Vector jerk = new Vector(0, 0);
        public Canvas cvs;
        public void Update()
        {
            acc += jerk;
            vel += acc;
            pos += vel;
        }
    }
    public partial class Projectile_Motion : UserControl
    {
        public decimal time = 0;
        public decimal timeFactor = 1.0M;
        public PBody mainPart = new PBody();
        public DispatcherTimer timer = new DispatcherTimer();
        public StartMode sMode = StartMode.stopped;
      public  enum StartMode
    {
            started, paused, stopped
    }
        public Projectile_Motion()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(17);
        }
        public void Start()
        {
            sMode = StartMode.started;
        }
        public void Pause()
        {
            sMode = StartMode.paused;
        }
        public void Stop()
        {
            sMode = StartMode.stopped;
        }
        public void Update()
        {
            if (sMode == StartMode.paused || sMode == StartMode.stopped) { return; }
            GravityUpdate();

        }
        public void GravityUpdate()
        {
            mainPart.vel.Y += 10;
        }
    }
}
