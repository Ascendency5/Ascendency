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
            this.callback = callback;

            InitializeComponent();
            UserControlAnimation.StartButtonGradientSpin(Buttons);
        }

        private void EventFilter(object sender)
        {
            if (sender == LeaveGame)
            {
                callback(this, new MenuOptionEventArgs(MenuOption.Exit));
            }
            else if (sender == Resume)
            {
                callback(this, new MenuOptionEventArgs(MenuOption.Resume));
            }
            else if (sender == Restart)
            {
                callback(this, new MenuOptionEventArgs(MenuOption.Restart));
            }
        }

        private void InGameMenuButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonUpStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

            EventFilter(sender);
        }

        private void InGameMenuButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonDownStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();
        }

        private void InGameMenuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], true);
            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void InGameMenuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //todo get mouse down working with this
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], false);
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
