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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for Earth3DModelUserControl.xaml
    /// </summary>
    public partial class Earth3DModelUserControl : UserControl
    {
        public Earth3DModelUserControl()
        {
            InitializeComponent();
        }

        private void EarthViewport_MouseEnter(object sender, MouseEventArgs e)
        {

            StreamResourceInfo info = Application.GetResourceStream(new Uri(@"Resources/Images/EarthCursor.cur", UriKind.Relative));
            Cursor = new Cursor(info.Stream);
        }

        private void EarthShadow_MouseEnter(object sender, MouseEventArgs e)
        {
            StreamResourceInfo info = Application.GetResourceStream(new Uri(@"Resources/Images/MainWindowCursor.cur", UriKind.Relative));
            Cursor = new Cursor(info.Stream);
        }
    }
}
