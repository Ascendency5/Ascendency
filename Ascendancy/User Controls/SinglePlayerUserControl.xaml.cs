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

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for SinglePlayerUserControl.xaml
    /// </summary>
    public partial class SinglePlayerUserControl : UserControl
    {
        public SinglePlayerUserControl()
        {
            InitializeComponent();
        }

        private void CancelIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(CancelHover, false);

            ContentControlActionsWrapper.FadeOut();
        }

        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //currentUserControlAnimation.MenuButtonSound.Open(new Uri(@"/Resources/Audio/hover.wav", UriKind.Relative));
            //currentUserControlAnimation.MenuButtonSound.Volume = 0.97;
            //currentUserControlAnimation.MenuButtonSound.Play();
        }

        private void UserControlButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, true);
        }

        private void UserControlButton_MouseLeave(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, false);
        }

        //Click to Play the Game
        private void PlaySinglePlayer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActionsWrapper.FadeOut();

            // Set up game engine
            // todo Add code for easy/hard AI
            HumanPlayer humanPlayer = new HumanPlayer(GameBoardUserControl.human_move_handler);
            AIPlayer aiPlayer = new AIPlayer();

            Board board = BoardSetup.board_team1;

            GameEngine engine = new GameEngine(board, humanPlayer, aiPlayer, PieceType.Red);

            ContentControlActionsWrapper.setUpControl(new GameBoardUserControl(engine));
        }
    }
}
