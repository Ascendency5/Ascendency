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

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for GameCompleteUserControl.xaml
    /// </summary>
    public partial class GameCompleteUserControl : UserControl
    {
        private GameCompleteMenuButtonHandler callback;

        public delegate void GameCompleteMenuButtonHandler(object sender, GameCompleteMenuOptionEventArgs eventArgs);

        public GameCompleteUserControl(GameResult gameResult, GameCompleteMenuButtonHandler callback)
        {
            this.callback = callback;
            InitializeComponent();
            UserControlAnimation.StartButtonGradientSpin(Buttons);
            showWinner(gameResult);

        }

        private void showWinner(GameResult gameResult)
        {
            Storyboard gameComplete = FindResource("CompletionLogoStoryboard") as Storyboard;
            switch (gameResult)
            {
                case GameResult.Win:
                    Storyboard.SetTarget(gameComplete, WinCanvas);
                    //Storyboard.SetTargetProperty(WinCanvas, new PropertyPath("Opacity"));
                    VolumeManager.play(@"Resources/Audio/FanfareHuman.wav");
                    break;
                case GameResult.Loss:
                    Storyboard.SetTarget(gameComplete, LossCanvas);
                    //Storyboard.SetTargetProperty(LossCanvas, new PropertyPath("Opacity"));
                    VolumeManager.play(@"Resources/Audio/FanfareRobot.wav");
                    break;
                case GameResult.Tie:
                    Storyboard.SetTarget(gameComplete, TieCanvas);
                    //Storyboard.SetTargetProperty(TieCanvas, new PropertyPath("Opacity"));
                    VolumeManager.play(@"Resources/Audio/FanfareHuman.wav");
                    break;
            }
            gameComplete.Begin();
        }

        private void EventFilter(object sender)
        {
            if (sender == NewGame)
            {
                callback(this, new GameCompleteMenuOptionEventArgs(GameCompleteMenuOption.NewGame));
            }
            else if (sender == Restart)
            {
                callback(this, new GameCompleteMenuOptionEventArgs(GameCompleteMenuOption.Restart));
            }
            else if (sender == MainMenu)
            {
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
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], true);
            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void GameCompleteMenuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //todo get mouse down working with this
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], false);
        }
    }

    public class GameCompleteMenuOptionEventArgs : EventArgs
    {
        public readonly GameCompleteMenuOption Option;

        public GameCompleteMenuOptionEventArgs(GameCompleteMenuOption option)
        {
            this.Option = option;
        }
    }

    public enum GameCompleteMenuOption
    {
        NewGame,
        Restart,
        MainMenu
    }

    public enum GameResult
    {
        Win,
        Loss,
        Tie
    }
}


/*
 
 * node1 = 
 * 
 
 */