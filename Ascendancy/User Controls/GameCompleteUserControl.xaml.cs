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
using Ascendancy.Networking;

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for GameCompleteUserControl.xaml
    /// </summary>
    public partial class GameCompleteUserControl : UserControl
    {
        private double tempVolume;
        private MediaPlayer fanfareMediaPlayer;
        private GameCompleteMenuButtonHandler callback;

        public delegate void GameCompleteMenuButtonHandler(object sender, GameCompleteMenuOptionEventArgs eventArgs);

        public GameCompleteUserControl(GameResult gameResult, GameCompleteMenuButtonHandler callback)
        {
            this.callback = callback;
            InitializeComponent();
            UserControlAnimation.StartButtonGradientSpin(Buttons);
            showWinner(gameResult);

            //keep the music down while the fanfare plays
            VolumeManager.BattleThemeTransition = false;
            tempVolume = VolumeManager.GetMusicVolume;
            VolumeManager.MainThemeVolume = 0;

        }

        private void showWinner(GameResult gameResult)
        {
            string victorySpriteFile = "";
            string uriString = "";
            Storyboard videoStoryboard;

            //set the file name of the sprite to be loaded
            switch (gameResult)
            {
                case GameResult.Win:
                    victorySpriteFile = "VictorySprite";
                    uriString = @"Resources/Audio/FanfareHuman.wav";
                    videoStoryboard = FindResource("AstroWinStoryboard") as Storyboard;
                    videoStoryboard.Begin();
                    break;
                case GameResult.Loss:
                    victorySpriteFile = "DefeatSprite";
                    uriString = @"Resources/Audio/FanfareRobot.wav";
                    videoStoryboard = FindResource("AstroLossStoryboard") as Storyboard;
                    videoStoryboard.Begin();
                    PageX.SetValue(Canvas.LeftProperty,-140.0);
                    break;
                case GameResult.Tie:
                    victorySpriteFile = "TieSprite";
                    uriString = @"Resources/Audio/FanfareHuman.wav";
                    videoStoryboard = FindResource("AstroTieStoryboard") as Storyboard;
                    videoStoryboard.Begin();
                    PageX.SetValue(Canvas.LeftProperty,-140.0);
                    break;
            }

            //play the correct fanfare
            fanfareMediaPlayer = new MediaPlayer();
            fanfareMediaPlayer.Open(new Uri(uriString, UriKind.Relative));
            fanfareMediaPlayer.Volume = VolumeManager.GetMusicVolume;
            fanfareMediaPlayer.Play();

            // Sprite resource name and width
            Sprite victorySprite = new Sprite(victorySpriteFile, 641, AnimationType.AnimateOnce)
            {
                Width = 640,
                Height = 360,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stretch = Stretch.Uniform,
                Name = victorySpriteFile
            };

            victorySprite.Margin = new Thickness(0, 0, 0, 0);
            SpriteCanvas.Children.Add(victorySprite);
            Panel.SetZIndex(victorySprite, 3);
        }

        private void EventFilter(object sender)
        {
            fanfareMediaPlayer.Volume = 0;
            if (sender == NewGame)
            {
                callback(this, new GameCompleteMenuOptionEventArgs(GameCompleteMenuOption.NewGame));
            }
            else if (sender == MainMenu)
            {
                //adjust the sound and kill the network manager if its running
                if(Networkmanager.IsRunning())
                    Networkmanager.Shutdown();
                VolumeManager.MainThemeVolume = tempVolume;
                callback(this, new GameCompleteMenuOptionEventArgs(GameCompleteMenuOption.MainMenu));
            }
        }

        private void GameCompleteMenuButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonUpStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

            EventFilter(sender);
        }

        private void GameCompleteMenuButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonDownStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();
        }

        private void GameCompleteMenuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInElement(animateThisCanvas.Children[0], true);
            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void GameCompleteMenuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //todo get mouse down working with this
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInElement(animateThisCanvas.Children[0], false);
        }
    }

    public class GameCompleteMenuOptionEventArgs : EventArgs
    {
        public readonly GameCompleteMenuOption Option;
        public GameCompleteMenuOptionEventArgs(GameCompleteMenuOption option)
        {
            Option = option;
        }
    }

    public enum GameCompleteMenuOption
    {
        NewGame,
        MainMenu
    }

    public enum GameResult
    {
        Win,
        Loss,
        Tie
    }
}