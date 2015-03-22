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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ascendancy.User_Control;
using Ascendancy.User_Controls.Multiplayer;

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for HomeScreenUserControl.xaml
    /// </summary>
    public partial class HomeScreenUserControl : UserControl
    {
        public HomeScreenUserControl()
        {
            InitializeComponent();

            StartMusic();
        }

        #region mouse callbacks
        #region Popup controls

        private void SinglePlayerHover_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            popup(new SinglePlayerUserControl());
        }

        private void MultiplayerHover_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            popup(new MultiplayerStarterUserControl());
        }

        private void HelpHover_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            popup(new HelpPopUpUserControl());
        }

        private void OptionsHover_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            popup(new OptionsUserControl());
        }

        private void ExitHover_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            popup(new ExitConfirmationUserControl());
        }

        private void popup(UserControl control)
        {
            ContentControlActions.setPopup(control);
        }

        #endregion

        private void MenuLogo_MouseEnter(object sender, MouseEventArgs e)
        {
            Storyboard logoStoryboard = new Storyboard();

            var gradientAnimation = new DoubleAnimationUsingKeyFrames
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 900))
            };

            gradientKeyFrame(gradientAnimation, 0.5, 0);
            gradientKeyFrame(gradientAnimation, 1, 300);
            gradientKeyFrame(gradientAnimation, 0.1, 600);
            gradientKeyFrame(gradientAnimation, 0.5, 900);

            //Storyboard.SetTarget(gradientAnimation, LogoGradient);
            Storyboard.SetTargetName(gradientAnimation, "LogoGradient");
            Storyboard.SetTargetProperty(gradientAnimation, new PropertyPath(GradientStop.OffsetProperty));

            logoStoryboard.Children.Add(gradientAnimation);

            logoStoryboard.Begin(this);

            // todo figure out if we need to use soundplayer
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Resources/Audio/LogoHover.wav");
            player.Play();
        }

        private static void gradientKeyFrame(DoubleAnimationUsingKeyFrames epicAnimation, double gradientPosition, int milliseconds)
        {
            epicAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(gradientPosition,
                KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(milliseconds))));
        }

        private void HomeScreen_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, true);

            //todo: do all sound effects have to interrupt each other?

            //sound effect style 1
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Resources/Audio/HomeScreenButtonHover.wav");
            player.Play();
        }

        private void HomeScreen_MouseLeave(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, false);
        }
        #endregion

        private void StartMusic()
        {
            //start up the looped media
            Storyboard playThemeSong = FindResource("HomeScreenThemeSongStoryboard") as Storyboard;
            HomeScreenThemeSong.BeginStoryboard(playThemeSong);
            Storyboard playHomeScreenBackground = FindResource("HomeScreenBackgroundVideoStoryboard") as Storyboard;
            HomeScreenVideo.BeginStoryboard(playHomeScreenBackground);
        }
    }
}
