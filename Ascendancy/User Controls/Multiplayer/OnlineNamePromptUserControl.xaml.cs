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

namespace Ascendancy.User_Controls.Multiplayer
{
    /// <summary>
    /// Interaction logic for OnlineNamePromptUserControl.xaml
    /// </summary>
    public partial class OnlineNamePromptUserControl : UserControl
    {
        public OnlineNamePromptUserControl()
        {
            InitializeComponent();
        }
        
        private void EnterLobbyIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActionsWrapper.setUpControl(new OnlineLobbyUserControl());
        }

        private void CancelIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(CancelHover, false);

            //ContentControlActionsWrapper.FadeOut();
            ContentControlActionsWrapper.setUpControl(new MultiplayerStarterUserControl());
        }

        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender == EnterLobbyIdle)
                UserControlAnimation.FadeInUserControlButton(EnterLobbyHover, false);
            else if (sender == CancelIdle)
                UserControlAnimation.FadeInUserControlButton(CancelHover, false);

            //added sound effect for the button
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Resources/Audio/UserControlButtonHover.wav");
            player.Play();
        }

        private void UserControlButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, false);
        }

        private void UserControlButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (EnterLobbyHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(EnterLobbyHover, true);
            if (CancelHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(CancelHover, true);

            UserControlAnimation.FadeInUserControlButton(sender, true);
        }

    }
}
