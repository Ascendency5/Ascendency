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
        private InGameMenuButtonHandler callback;

        public delegate void InGameMenuButtonHandler(object sender, MenuOptionEventArgs eventArgs);

        public InGameMenuUserControl(InGameMenuButtonHandler callback)
        {
            InitializeComponent();
            this.callback = callback;
        }

        private void Resume_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            callback(this, new MenuOptionEventArgs(MenuOption.Resume));
        }

        private void Options_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.setPopup(new OptionsUserControl());
        }

        private void Help_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // todo This is a control, but controls don't actually work.
            // ContentControlActions.setUpControl(new HelpPopUpUserControl());
            throw new NotImplementedException();
        }

        private void Restart_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            callback(this, new MenuOptionEventArgs(MenuOption.Restart));
        }

        private void LeaveGame_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            callback(this, new MenuOptionEventArgs(MenuOption.Exit));
        }
    }

    public class MenuOptionEventArgs : EventArgs
    {
        public readonly MenuOption Option;

        public MenuOptionEventArgs(MenuOption option)
        {
            this.Option = option;
        }
    }

    public enum MenuOption
    {
        Resume,
        Restart,
        Exit
    }
}
