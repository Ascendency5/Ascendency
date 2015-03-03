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
using Ascendancy.Game_Engine;

namespace Ascendancy.User_Controls.Multiplayer
{
    /// <summary>
    /// Interaction logic for LocalMultiplayerUserControl.xaml
    /// </summary>
    public partial class LocalMultiplayerUserControl : UserControl
    {
        public LocalMultiplayerUserControl()
        {
            InitializeComponent();
        }

        private void PlayIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActionsWrapper.FadeOut();

            // Set up game engine
            // todo Add code for easy/hard AI
            HumanPlayer humanPlayerRed = new HumanPlayer(GameBoardUserControl.human_move_handler);
            HumanPlayer humanPlayerBlack = new HumanPlayer(GameBoardUserControl.human_move_handler);

            //HumanPlayer aiPlayer = new HumanPlayer(GameBoardUserControl.human_move_handler);
            //AIPlayer aiPlayer = new AIPlayer();

            Board board = BoardSetup.board_team5;

            GameEngine engine = new GameEngine(board, humanPlayerRed, humanPlayerBlack, PieceType.Red);

            ContentControlActionsWrapper.setUpControl(new GameBoardUserControl(engine));
        }

        private void CancelIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(CancelHover, false);

            //ContentControlActionsWrapper.FadeOut();
            ContentControlActionsWrapper.setUpControl(new MultiplayerStarterUserControl());
        }

        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender == PlayIdle)
                UserControlAnimation.FadeInUserControlButton(PlayHover, false);
            else if (sender == CancelIdle)
                UserControlAnimation.FadeInUserControlButton(CancelHover, false);

            //added sound effect for the button
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Resources/Audio/UserControlButtonHover.wav");
            player.Play();
        }

        private void UserControlButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, false);
        }

        private void UserControlButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (PlayHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(PlayHover, true);
            if (CancelHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(CancelHover, true);

            UserControlAnimation.FadeInUserControlButton(sender, true);
        }
    }
}
