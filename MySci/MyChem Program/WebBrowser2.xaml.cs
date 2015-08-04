using System;
using System.Collections.Generic;
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

namespace MyChem_Program
{
    /// <summary>
    /// Interaction logic for WebBrowser2.xaml
    /// </summary>
    public partial class WebBrowser2 : UserControl
    {
        public WebBrowser2(Uri uri)
        {

            InitializeComponent();
            webB.Source = uri;
        }
    }
}
