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
            UserControlAnimation.StartButtonGradientSpin(Buttons);
        }

        public void EventFilter(object sender)
        {
            if (sender == Cancel)
            {
                ContentControlActions.FadeOut();
            }
            else if (sender == Play)
            {
                // Set up game engine
                HumanPlayer humanPlayerRed = new HumanPlayer(GameBoardUserControl.human_move_handler);
                HumanPlayer humanPlayerBlack = new HumanPlayer(GameBoardUserControl.human_move_handler);

                Board board = BoardSetup.board_team5;

                GameEngine engine = new GameEngine(board, humanPlayerRed, humanPlayerBlack, PieceType.Red);

                // todo Make this transition better
                ContentControlActions.setUpControl(new GameBoardUserControl(engine));
            }
        }

        
        private void LocalMultiplayerButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonUpStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

            EventFilter(sender);
        }

        private void LocalMultiplayerButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonDownStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

        }

        private void LocalMultiplayerButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], true);
            
            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void LocalMultiplayerButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //todo get mouse down working with this
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], false);
        }


    }
}
