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

        private void QuitIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //todo: fix exit animation
            //anim.animateWindow(Application.Current.MainWindow, false);
            //homeScreen.Close();
            Application.Current.Shutdown();
        }

        private void CancelIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(CancelHover, false);

            ContentControlActionsWrapper.FadeOut();
        }

        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender == QuitIdle)
                UserControlAnimation.FadeInUserControlButton(QuitHover, true);
            else
                UserControlAnimation.FadeInUserControlButton(CancelHover, true);
            
            MediaPlayer player = new MediaPlayer();
            player.Open(new Uri(@"/Resources/Audio/hover.wav", UriKind.Relative));
            player.Volume = 0.67;
            player.Play();
        }

        private void UserControlButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, true);
        }

        private void UserControlButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //if (sender == QuitIdle)
            //    currentUserControlAnimation.FadeInUserControlButton(QuitHover, false);
            //else// if(sender == CancelIdle)
            //    currentUserControlAnimation.FadeInUserControlButton(CancelHover, false);

            UserControlAnimation.FadeInUserControlButton(sender, false);
        }

    }
}
