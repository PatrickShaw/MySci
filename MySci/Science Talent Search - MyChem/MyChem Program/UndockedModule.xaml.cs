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
using System.Windows.Shapes;

namespace MyChem_Program
{
    /// <summary>
    /// Interaction logic for UndockedModule.xaml
    /// </summary>
    public partial class UndockedModule : Window
    {
        public UndockedModule(UserControl userControl)
        {
            InitializeComponent();
            theWindow.Content = userControl;
        }
    }
}
