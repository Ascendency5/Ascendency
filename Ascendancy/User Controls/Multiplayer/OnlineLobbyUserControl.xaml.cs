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
    /// Interaction logic for OnlineLobbyUserControl.xaml
    /// </summary>
    public partial class OnlineLobbyUserControl : UserControl
    {
        public OnlineLobbyUserControl()
        {
            InitializeComponent();
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
            if (sender == ChallengeIdle)
                UserControlAnimation.FadeInUserControlButton(ChallengeHover, false);
            else if (sender == ConnectIdle)
                UserControlAnimation.FadeInUserControlButton(ConnectHover, false);
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
            if (ChallengeHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(ChallengeHover, true);
            if (ConnectHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(ConnectHover, true);
            if (CancelHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(CancelHover, true);

            UserControlAnimation.FadeInUserControlButton(sender, true);
        }
    }
}
