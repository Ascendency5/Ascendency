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
using Panel = System.Windows.Controls.Panel;

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
            InsertSprites();
        }
        private void InsertSprites()
        {
            string hardRobotFile;
            string sound;

            hardRobotFile = "HardRobotIdle";
            sound = "Resources/Audio/RobotPodDown.wav";


            // Sprite resource name and width
            Sprite hardRobotSprite = new Sprite(hardRobotFile, 311, AnimationType.AnimateForever)
            {
                Width = 310,
                Height = 310,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stretch = Stretch.Fill,
                Name = hardRobotFile
            };

            //Easy or Hard AI
            hardRobotSprite.Margin = new Thickness(0, 0, 0, 0);
            PlayerTwoSpriteCanvas.Children.Add(hardRobotSprite);
            Panel.SetZIndex(hardRobotSprite, 3);

            VolumeManager.play(sound);
        }

        private void Back_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
        }

        private void Play_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Set up game engine
            HumanPlayer humanPlayerRed = new HumanPlayer(GameBoardUserControl.human_move_handler);
            HumanPlayer humanPlayerBlack = new HumanPlayer(GameBoardUserControl.human_move_handler);

            Board board = BoardSetup.Random();

            GameEngine engine = new GameEngine(board, humanPlayerRed, humanPlayerBlack, PieceType.Red);

            //grab the content from the user input for the player names
            string playerOne = PlayerOneNamePrompt.Text;
            string playerTwo = PlayerTwoNamePrompt.Text;

            // todo Make this transition better
            ContentControlActions.setBaseContentControl(new GameBoardUserControl(engine, playerOne, playerTwo));
        }

        private void NamePromptText_Loaded(object sender, RoutedEventArgs e)
        {
            PlayerOneNamePrompt.Focus();
            PlayerOneNamePrompt.SelectAll();
        }

        private void NamePromptText_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender == PlayerOneNamePrompt)
            {
                if (PlayerOneNamePrompt.Text == "Player One")
                    PlayerOneNamePrompt.Text = "";
            }
            else
            {
                if (PlayerTwoNamePrompt.Text == "Player Two")
                    PlayerTwoNamePrompt.Text = "";
            }
        }
    }
}
