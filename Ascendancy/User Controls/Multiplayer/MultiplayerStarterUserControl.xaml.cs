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
    /// Interaction logic for MultiplayerStarterUserControl.xaml
    /// </summary>
    public partial class MultiplayerStarterUserControl : UserControl
    {
        public MultiplayerStarterUserControl()
        {
            InitializeComponent();
        }
        
        private void CancelIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(CancelHover, false);

            ContentControlActionsWrapper.FadeOut();
        }

        private void OnlineIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActionsWrapper.setUpControl(new OnlineNamePromptUserControl());
        }

        private void LocalIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActionsWrapper.setUpControl(new LocalMultiplayerUserControl());
        }

        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender == LocalIdle)
                UserControlAnimation.FadeInUserControlButton(LocalHover, false);
            else if (sender == OnlineIdle)
                UserControlAnimation.FadeInUserControlButton(OnlineHover, false);
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
            if (LocalHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(LocalHover, true);
            if (OnlineHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(CancelHover, true);
            if (CancelHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(CancelHover, true);

            UserControlAnimation.FadeInUserControlButton(sender, true);
        }

    }
}
