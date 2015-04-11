using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ascendancy.Game_Engine;

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for ExitConfirmationUserControl.xaml
    /// </summary>
    public partial class ExitConfirmationUserControl : UserControl
    {
        public ExitConfirmationUserControl()
        {
            InitializeComponent();
        }

        private void Cancel_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
        }

        private void Quit_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
