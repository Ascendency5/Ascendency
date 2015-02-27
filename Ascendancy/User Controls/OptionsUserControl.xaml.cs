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
    public partial class OptionsUserControl : UserControl
    {

        public OptionsUserControl()
        {
            InitializeComponent();
        }

        private void OkayIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(OkayHover, false);

            ContentControlActionsWrapper.FadeOut();
        }

        /**
        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (sender == QuitIdle)
              //  currentUserControlAnimation.FadeInUserControlButton(QuitHover, true);
            //else
            //    currentUserControlAnimation.FadeInUserControlButton(OkayHover, true);


            currentUserControlAnimation.MenuButtonSound.Open(new Uri(@"/Resources/Audio/hover.wav", UriKind.Relative));
            currentUserControlAnimation.MenuButtonSound.Volume = 0.67;
            currentUserControlAnimation.MenuButtonSound.Play();
        }

        private void UserControlButton_MouseEnter(object sender, MouseEventArgs e)
        {
            currentUserControlAnimation.FadeInUserControlButton(sender, true);
            currentUserControlAnimation = new UserControlAnimation();
        }

        private void UserControlButton_MouseLeave(object sender, MouseEventArgs e)
        {
            currentUserControlAnimation.FadeInUserControlButton(sender, false);
            currentUserControlAnimation = new UserControlAnimation();
        }
        */
    }
}
