using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media;
namespace MyChem_Program
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            if (!File.Exists(@".\Settings.ini"))
            File.Create(@".\Settings.ini");
            Cache.defaultMoleculeFolder = Path.Combine(Directory.GetCurrentDirectory(), @"Molecules");
            Cache.defaultGravityFolder = Path.Combine(Directory.GetCurrentDirectory(), @"Gravity Simulator\Save Files");
            if (!Directory.Exists(Cache.defaultMoleculeFolder))
            {
                // This path is a directory
                Directory.CreateDirectory(Cache.defaultMoleculeFolder);
            }
        }

    }
}
