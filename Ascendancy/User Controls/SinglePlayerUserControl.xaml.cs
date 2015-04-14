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
using Panel = System.Windows.Controls.Panel;

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for SinglePlayerUserControl.xaml
    /// </summary>
    public partial class SinglePlayerUserControl : UserControl
    {
        private bool gameModeIsHard;
        private bool playerGoesFirst;
        private Storyboard fadeInSprite;
        private Storyboard fadeOutSprite;

        public SinglePlayerUserControl()
        {
            InitializeComponent();

            //set the default difficulty and playerGoesFirst/second state
            gameModeIsHard = false;
            playerGoesFirst = true;
            InsertSprites();
            fadeOutSprite = FindResource("FadeOutSpriteStoryboard") as Storyboard;
            fadeInSprite = FindResource("FadeInSpriteStoryboard") as Storyboard;
        }

        private void startGame()
        {
            ContentControlActions.FadeOut();

            // Set up game engine
            // todo Add code for gameModeIsHard/hard AI
            HumanPlayer humanPlayer = new HumanPlayer(GameBoardUserControl.human_move_handler);
            AIPlayer aiPlayer = new AIPlayer();

            Board board = BoardSetup.Random();

            GameEngine engine = new GameEngine(board, humanPlayer, aiPlayer,
                playerGoesFirst ? PieceType.Red : PieceType.Black
                );

            ContentControlActions.setBaseContentControl(new GameBoardUserControl(engine, gameModeIsHard));
        }

        private void Play_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            startGame();
        }

        private void Cancel_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
        }

        //todo Nik added a few lines for the storyboards

        private void Easy_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (gameModeIsHard)
            {
                transition(EasyRobotCanvas, HardRobotCanvas);
            }
            gameModeIsHard = false;
            Easy.Selected = true;
            Hard.Selected = false;
        }

        private void Hard_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!gameModeIsHard)
            {
                transition(HardRobotCanvas, EasyRobotCanvas);
            }
            gameModeIsHard = true;
            Easy.Selected = false;
            Hard.Selected = true;
        }

        private void GoFirst_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!playerGoesFirst)
            {
                transition(GoFirstCanvas, GoSecondCanvas);
            }
            playerGoesFirst = true;
            GoFirst.Selected = true;
            GoSecond.Selected = false;
        }

        private void GoSecond_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (playerGoesFirst)
            {
                transition(GoSecondCanvas, GoFirstCanvas);
            }
            playerGoesFirst = false;
            GoFirst.Selected = false;
            GoSecond.Selected = true;
        }

        private void transition(Canvas fadeInCanvas, Canvas fadeOutCanvas)
        {
            Storyboard.SetTarget(fadeInSprite, fadeInCanvas);
            Storyboard.SetTarget(fadeOutSprite, fadeOutCanvas);
            fadeOutSprite.Begin();
            fadeInSprite.Begin();
        }

        #region Local Sprites

        private void InsertSprites()
        {
            string easyRobotFile;
            string hardRobotFile;
            string goFirstFile;
            string goSecondFile;
            string sound;

            easyRobotFile = "EasyRobotIdle";
            hardRobotFile = "HardRobotIdle";
            sound = "Resources/Audio/RobotPodDown.wav";

            goFirstFile = "GoFirstSprite";
            goSecondFile = "GoSecondSprite";

            // Sprite resource name and width
            Sprite easyRobotSprite = new Sprite(easyRobotFile, 311, AnimationType.AnimateForever)
            {
                Width = 310,
                Height = 310,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stretch = Stretch.Fill,
                Name = easyRobotFile
            };
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
            // Sprite resource name and width
            Sprite goFirstSprite = new Sprite(goFirstFile, 311, AnimationType.AnimateForever)
            {
                Width = 310,
                Height = 310,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stretch = Stretch.Fill,
                Name = goFirstFile
            };
            // Sprite resource name and width
            Sprite goSecondSprite = new Sprite(goSecondFile, 311, AnimationType.AnimateForever)
            {
                Width = 310,
                Height = 310,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stretch = Stretch.Fill,
                Name = goSecondFile
            };

            //Easy or Hard AI
            easyRobotSprite.Margin = new Thickness(0, 0, 0, 0);
            hardRobotSprite.Margin = new Thickness(0, 0, 0, 0);

            EasyRobotCanvas.Children.Add(easyRobotSprite);
            HardRobotCanvas.Children.Add(hardRobotSprite);

            //Going First or Second 
            goFirstSprite.Margin = new Thickness(0, 0, 0, 0);
            goSecondSprite.Margin = new Thickness(0, 0, 0, 0);

            GoFirstCanvas.Children.Add(goFirstSprite);
            GoSecondCanvas.Children.Add(goSecondSprite);


            Panel.SetZIndex(easyRobotSprite, 3);
            Panel.SetZIndex(hardRobotSprite, 3);

            Panel.SetZIndex(goFirstSprite, 3);
            Panel.SetZIndex(goFirstSprite, 3);

            VolumeManager.play(sound);
        }
        #endregion
    }
}
