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
        // Sprite resource name and width
        Sprite logoSprite = new Sprite("LogoSprite", 601, AnimationType.AnimateOnce)
        {
            Width = 600,
            Height = 250,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Stretch = Stretch.Fill,
            Name = "LogoSprite"
        };
        public HomeScreenUserControl()
        {
            InitializeComponent();

            StartMusic();
            Load3DModel();
            LoadLogoSprite();
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

        private void HomeScreen_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInElement(sender, true);

            //sound effect style 1
            VolumeManager.play(@"Resources/Audio/HomeScreenButtonHover.wav");
        }

        private void HomeScreen_MouseLeave(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInElement(sender, false);
        }

        //todo get this function working
        private void LogoCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            //animate the sprite
            //Sprite animateLogo = new Sprite(logoSprite);
            //LogoCanvas.Children.Add(animateLogo);
            //Panel.SetZIndex(animateLogo, 4);
            VolumeManager.play("Resources/Audio/LogoHover.wav");
            logoSprite.ResetAnimation();
        }
        #endregion

        private void StartMusic()
        {
            //start up the looped media
            VolumeManager.play(@"Resources/Audio/ThemeSong.wav", SoundType.MenuMusic, SoundLoop.Loop);
        }

        private void LoadLogoSprite()
        {
            //Easy or Hard AI
            logoSprite.Margin = new Thickness(0, 0, 0, 0);
            LogoCanvas.Children.Add(logoSprite);
            Panel.SetZIndex(logoSprite, 3);
        }

        private void Load3DModel()
        {
            HomeScreenContentControl.Content = new Earth3DModelUserControl();
        }


        private void HomeScreenUserControlView_Unloaded(object sender, RoutedEventArgs e)
        {

            VolumeManager.play(@"Resources/Audio/BattleTheme.wav", SoundType.BattleMusic, SoundLoop.Battle);
            //VolumeManager.BattleThemeTransition = true;
        }
    }
}
