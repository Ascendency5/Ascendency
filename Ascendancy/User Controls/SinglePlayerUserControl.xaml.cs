using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using Ascendancy.Game_Engine;
using Panel = System.Windows.Forms.Panel;

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for SinglePlayerUserControl.xaml
    /// </summary>
    public partial class SinglePlayerUserControl : UserControl
    {
        private bool gameModeIsEasy;
        private bool playerGoesFirst;

        public SinglePlayerUserControl()
        {
            InitializeComponent();

            //set the default difficulty and playerGoesFirst/second state
            gameModeIsEasy = true;
            playerGoesFirst = true;
            GoFirstSelected.Opacity = 1;
            EasySelected.Opacity = 1;

            System.Windows.Controls.Panel.SetZIndex(EasyClick, 3);
            System.Windows.Controls.Panel.SetZIndex(GoFirstClick, 3);
            UserControlAnimation.StartButtonGradientSpin(Buttons);
        }

        //other functions
        private void EventFilter(object sender)
        {
            if (sender == Play)
            {
                startGame();
            }
            else if (sender == Cancel)
            {
                UserControlAnimation.FadeInUserControlButton(CancelHover, false);
                ContentControlActions.FadeOut();
            }

            if (gameModeIsEasy)
            {
                if (sender == Hard)
                {
                    gameModeIsEasy = false;
                    animateSelectedButton(Easy, false);
                    animateSelectedButton(Hard, true);
                }
            }
            else if (!gameModeIsEasy)
            {
                if (sender == Easy)
                {
                    gameModeIsEasy = true;
                    animateSelectedButton(Easy, true);
                    animateSelectedButton(Hard, false);
                }
            }

            if (playerGoesFirst)
            {
                if (sender == GoSecond)
                {
                    playerGoesFirst = false;
                    animateSelectedButton(GoFirst, false);
                    animateSelectedButton(GoSecond, true);
                }
            }
            else if (!playerGoesFirst)
            {
                if (sender == GoFirst)
                {
                    playerGoesFirst = true;
                    animateSelectedButton(GoFirst, true);
                    animateSelectedButton(GoSecond, false);
                }
            }
        }

        private void startGame()
        {
            ContentControlActions.FadeOut();

            // Set up game engine
            // todo Add code for gameModeIsEasy/hard AI
            HumanPlayer humanPlayer = new HumanPlayer(GameBoardUserControl.human_move_handler);
            //HumanPlayer aiPlayer = new HumanPlayer(GameBoardUserControl.human_move_handler);
            AIPlayer aiPlayer = new AIPlayer();

            Board board = BoardSetup.board_team5;

            GameEngine engine = new GameEngine(board, humanPlayer, aiPlayer,
                playerGoesFirst ? PieceType.Red : PieceType.Black
                );

            ContentControlActions.setUpControl(new GameBoardUserControl(engine));
        }

        private void animateSelectedButton(Canvas sender, bool fadeIn)
        {
            UserControlAnimation.FadeInUserControlButton(sender.Children[2], fadeIn);
        }

        #region Button Animations
        private void Cancel_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], true);
            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void Cancel_MouseLeave(object sender, MouseEventArgs e)
        {
            //todo get mouse down working with this
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], false);

        }

        private void Cancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonDownStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();
        }

        private void Cancel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonUpStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

            EventFilter(sender);
        }

        #endregion


        //todo re-implement logic to highlight "gameModeIsEasy/hard/playerGoesFirst second"
        #region Old Button Animations

        private void CancelIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(CancelHover, false);

            ContentControlActions.FadeOut();
        }

        //Click to Play the Game
        private void PlaySinglePlayerIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();

            // Set up game engine
            // todo Add code for gameModeIsEasy/hard AI
            HumanPlayer humanPlayer = new HumanPlayer(GameBoardUserControl.human_move_handler);
            //HumanPlayer aiPlayer = new HumanPlayer(GameBoardUserControl.human_move_handler);
            AIPlayer aiPlayer = new AIPlayer();

            Board board = BoardSetup.board_team5;

            GameEngine engine = new GameEngine(board, humanPlayer, aiPlayer,
                playerGoesFirst ? PieceType.Red : PieceType.Black
                );

            ContentControlActions.setUpControl(new GameBoardUserControl(engine));
        }

        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Displays whether the Easy AI is selected, or the Hard AI
            if (gameModeIsEasy)
            {
                if (sender == HardIdle)
                {
                    gameModeIsEasy = false;
                    System.Windows.Controls.Panel.SetZIndex(HardClick, 3);
                    System.Windows.Controls.Panel.SetZIndex(EasyClick, 2);
                }
            }
            else if (gameModeIsEasy == false)
            {
                if (sender == EasyIdle)
                {
                    gameModeIsEasy = true;
                    System.Windows.Controls.Panel.SetZIndex(EasyClick, 3);
                    System.Windows.Controls.Panel.SetZIndex(HardClick, 2);
                }
            }

            //Displays whether the Human Player plays playerGoesFirst or second
            if (playerGoesFirst)
            {
                if (sender == GoSecondIdle)
                {
                    playerGoesFirst = false;
                    System.Windows.Controls.Panel.SetZIndex(GoSecondClick, 3);
                    System.Windows.Controls.Panel.SetZIndex(GoFirstClick, 2);
                }
            }
            else if (playerGoesFirst == false)
            {
                if (sender == GoFirstIdle)
                {
                    playerGoesFirst = true;
                    System.Windows.Controls.Panel.SetZIndex(GoFirstClick, 3);
                    System.Windows.Controls.Panel.SetZIndex(GoSecondClick, 2);
                }
            }
            

            if (sender == EasyIdle)
                UserControlAnimation.FadeInUserControlButton(EasyHover, false);
            else if (sender == HardIdle)
                UserControlAnimation.FadeInUserControlButton(HardHover, false);
            else if (sender == GoFirstIdle)
                UserControlAnimation.FadeInUserControlButton(GoFirstHover, false);
            else if (sender == GoSecondIdle)
                UserControlAnimation.FadeInUserControlButton(GoSecondHover, false);
            else if (sender == CancelIdle)
                UserControlAnimation.FadeInUserControlButton(CancelHover, false);
            //else if (sender == playsafasdfasdfasdf)
            //    UserControlAnimation.FadeInUserControlButton(CancelHover, true);

            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void UserControlButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //UserControlButton_MouseLeave(sender, e);
            if (sender == EasyIdle)
            {
                UserControlAnimation.FadeInUserControlButton(EasyHover, true);
            }
            else if (sender == HardIdle)
            {
                UserControlAnimation.FadeInUserControlButton(HardHover, true);
            }
            else if (sender == GoFirstIdle)
            {
                UserControlAnimation.FadeInUserControlButton(GoFirstHover, true);
            }
            else if (sender == GoSecondIdle)
            {
                UserControlAnimation.FadeInUserControlButton(GoSecondHover, true);
            }
            else if (sender == CancelIdle)
            {
                UserControlAnimation.FadeInUserControlButton(CancelHover, true);
            }
        }

        private void UserControlButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, false);
            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void UserControlButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (EasyHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(EasyHover, true);
            if (HardHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(HardHover, true);
            if (GoFirstHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(GoFirstHover, true);
            if (GoSecondHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(GoSecondHover, true);
            if (CancelHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(CancelHover, true);

            UserControlAnimation.FadeInUserControlButton(sender, true);
        }
        #endregion

    }
}
