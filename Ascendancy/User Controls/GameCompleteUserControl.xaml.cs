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
        private readonly double tempVolume;
        private readonly VoidFunctionTemplate.Function callback;

        public GameCompleteUserControl(GameResult gameResult, VoidFunctionTemplate.Function callback)
        {
            this.callback = callback;
            InitializeComponent();
            showWinner(gameResult);

            //keep the music down while the fanfare plays
            tempVolume = VolumeManager.MusicVolume;
            VolumeManager.MusicVolume = 0;
        }

        private void showWinner(GameResult gameResult)
        {
            string victorySpriteFile = "";
            string uriString = "";
            Storyboard videoStoryboard = null;

            //set the file name of the sprite to be loaded
            switch (gameResult)
            {
                case GameResult.Win:
                    victorySpriteFile = "VictorySprite";
                    uriString = @"Resources/Audio/FanfareHuman.wav";
                    videoStoryboard = FindResource("AstroWinStoryboard") as Storyboard;
                    break;
                case GameResult.Loss:
                    victorySpriteFile = "DefeatSprite";
                    uriString = @"Resources/Audio/FanfareRobot.wav";
                    videoStoryboard = FindResource("AstroLossStoryboard") as Storyboard;
                    PageX.SetValue(Canvas.LeftProperty,-140.0);
                    break;
                case GameResult.Tie:
                    victorySpriteFile = "TieSprite";
                    uriString = @"Resources/Audio/FanfareHuman.wav";
                    videoStoryboard = FindResource("AstroTieStoryboard") as Storyboard;
                    PageX.SetValue(Canvas.LeftProperty,-140.0);
                    break;
            }
            if(videoStoryboard != null)
                videoStoryboard.Begin();

            //play the correct fanfare
            VolumeManager.play(uriString);

            // Sprite resource name and width
            Sprite victorySprite = new Sprite(victorySpriteFile, 641, AnimationType.AnimateOnce)
            {
                Width = 640,
                Height = 360,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stretch = Stretch.Uniform,
                Name = victorySpriteFile,
                Margin = new Thickness(0, 0, 0, 0)
            };

            SpriteCanvas.Children.Add(victorySprite);
            Panel.SetZIndex(victorySprite, 3);
        }

        private void MainMenu_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            VolumeManager.MusicVolume = tempVolume;
            ContentControlActions.FadeOut();

            callback();
        }
    }

    public enum GameResult
    {
        Win,
        Loss,
        Tie
    }
}