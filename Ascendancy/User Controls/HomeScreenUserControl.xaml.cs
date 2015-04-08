using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

            Load3DModel();
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
            ContentControlActions.setUpControl(new HelpPopUpUserControl());
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

            VolumeManager.play(@"Resources/Audio/LogoHover.wav");
        }

        private static void gradientKeyFrame(DoubleAnimationUsingKeyFrames epicAnimation, double gradientPosition, int milliseconds)
        {
            epicAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(gradientPosition,
                KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(milliseconds))));
        }

        private void HomeScreen_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, true);

            //sound effect style 1
            VolumeManager.play(@"Resources/Audio/HomeScreenButtonHover.wav");
        }

        private void HomeScreen_MouseLeave(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, false);
        }
        #endregion

        private void StartMusic()
        {
            //start up the looped media
            VolumeManager.play(@"Resources/Audio/ThemeSong.wav", SoundType.Music, SoundLoop.Loop);
        }

        private void Load3DModel()
        {
            HomeScreenContentControl.Content = new Earth3DModelUserControl();
        }
    }
}
