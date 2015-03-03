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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ascendancy.Game_Engine;

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for SinglePlayerUserControl.xaml
    /// </summary>
    public partial class SinglePlayerUserControl : UserControl
    {
        private bool easy;
        private bool first;
        public SinglePlayerUserControl()
        {
            InitializeComponent();
            easy = true;
            first = true;
            System.Windows.Controls.Panel.SetZIndex(EasyClick, 3);
            System.Windows.Controls.Panel.SetZIndex(GoFirstClick, 3);
        }

        private void CancelIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(CancelHover, false);

            ContentControlActionsWrapper.FadeOut();
        }

        //Click to Play the Game
        private void PlaySinglePlayerIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActionsWrapper.FadeOut();

            // Set up game engine
            // todo Add code for easy/hard AI
            HumanPlayer humanPlayer = new HumanPlayer(GameBoardUserControl.human_move_handler);
            //HumanPlayer aiPlayer = new HumanPlayer(GameBoardUserControl.human_move_handler);
            AIPlayer aiPlayer = new AIPlayer();

            Board board = BoardSetup.board_team5;

            GameEngine engine = new GameEngine(board, humanPlayer, aiPlayer,
                first ? PieceType.Red : PieceType.Black
                );

            ContentControlActionsWrapper.setUpControl(new GameBoardUserControl(engine));
        }



        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Displays whether the Easy AI is selected, or the Hard AI
            if (easy)
            {
                if (sender == HardIdle)
                {
                    easy = false;
                    System.Windows.Controls.Panel.SetZIndex(HardClick, 3);
                    System.Windows.Controls.Panel.SetZIndex(EasyClick, 2);
                }
            }
            else if (easy == false)
            {
                if (sender == EasyIdle)
                {
                    easy = true;
                    System.Windows.Controls.Panel.SetZIndex(EasyClick, 3);
                    System.Windows.Controls.Panel.SetZIndex(HardClick, 2);
                }
            }

            //Displays whether the Human Player plays first or second
            if (first)
            {
                if (sender == GoSecondIdle)
                {
                    first = false;
                    System.Windows.Controls.Panel.SetZIndex(GoSecondClick, 3);
                    System.Windows.Controls.Panel.SetZIndex(GoFirstClick, 2);
                }
            }
            else if (first == false)
            {
                if (sender == GoFirstIdle)
                {
                    first = true;
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
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Resources/Audio/UserControlButtonHover.wav");
            player.Play();
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
    }
}
