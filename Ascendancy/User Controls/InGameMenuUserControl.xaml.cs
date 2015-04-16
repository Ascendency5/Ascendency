using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using Ascendancy.User_Control;
using Ascendancy.User_Controls.Multiplayer;

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for InGameMenuUserControl.xaml
    /// </summary>
    public partial class InGameMenuUserControl : UserControl
    {
        private readonly VoidFunctionTemplate.Function Callback;

        public InGameMenuUserControl(VoidFunctionTemplate.Function callback)
        {
            InitializeComponent();
            this.Callback = callback;
        }

        private void Resume_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
        }

        private void Options_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.setPopup(new OptionsUserControl());
        }

        private void Help_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.setPopup(new HelpPopUpUserControl());
        }

        private void LeaveGame_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
            Callback();
        }
    }
}
