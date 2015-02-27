using System;

namespace Ascendancy.Game_Engine
{
    class BoardSetup
    {
        public static readonly Board board_team1 = setupBoard1();
        public static readonly Board board_team2 = setupBoard2();
        public static readonly Board board_team3 = setupBoard3();
        public static readonly Board board_team4 = setupBoard4();
        public static readonly Board board_team5 = setupBoard5();
        public static readonly Board board_team6 = setupBoard6();
        public static readonly Board board_team7 = setupBoard7();

        private static Random rnd = new Random();

        public static Board random()
        {
            switch (rnd.Next(1, 7))
            {
                case 1: return board_team1;
                case 2: return board_team2;
                case 3: return board_team3;
                case 4: return board_team4;
                case 5: return board_team5;
                case 6: return board_team6;
                case 7: return board_team7;
            }

            // This shouldn't ever be possible
            return board_team1;
        }

        private static Board setupBoard1()
        {
            Panel[] panels =
            {
                new Panel(0, 0, PanelType.OneByThree),
                new Panel(0, 3, PanelType.OneByThree),
                new Panel(0, 6, PanelType.ThreeByOne),
                new Panel(0, 7, PanelType.ThreeByOne),
                new Panel(1, 0, PanelType.TwoByTwo),
                new Panel(1, 2, PanelType.TwoByOne),
                new Panel(1, 3, PanelType.TwoByOne),
                new Panel(1, 4, PanelType.TwoByOne),
                new Panel(1, 5, PanelType.TwoByOne),
                new Panel(3, 0, PanelType.TwoByTwo),
                new Panel(3, 2, PanelType.TwoByTwo),
                new Panel(3, 4, PanelType.TwoByTwo),
                new Panel(3, 6, PanelType.TwoByTwo),
                new Panel(5, 0, PanelType.ThreeByTwo),
                new Panel(5, 2, PanelType.ThreeByTwo),
                new Panel(5, 4, PanelType.ThreeByTwo),
                new Panel(5, 6, PanelType.ThreeByTwo)
            };

            return new Board(panels);
        }

        private static Board setupBoard2()
        {
            Panel[] panels =
            {
                new Panel(0, 0, PanelType.OneByThree),
                new Panel(0, 3, PanelType.OneByTwo),
                new Panel(0, 5, PanelType.OneByThree),
                new Panel(1, 0, PanelType.ThreeByTwo),
                new Panel(1, 2, PanelType.ThreeByOne),
                new Panel(1, 3, PanelType.OneByThree),
                new Panel(1, 6, PanelType.ThreeByTwo),
                new Panel(2, 3, PanelType.TwoByOne),
                new Panel(2, 4, PanelType.TwoByOne),
                new Panel(2, 5, PanelType.TwoByOne),
                new Panel(4, 0, PanelType.TwoByTwo),
                new Panel(4, 2, PanelType.TwoByTwo),
                new Panel(4, 4, PanelType.TwoByTwo),
                new Panel(4, 6, PanelType.TwoByTwo),
                new Panel(6, 0, PanelType.TwoByTwo),
                new Panel(6, 2, PanelType.TwoByThree),
                new Panel(6, 5, PanelType.TwoByThree)
            };

            return new Board(panels);
        }

        private static Board setupBoard3()
        {
            Panel[] panels =
            {
                new Panel(0, 0, PanelType.ThreeByTwo),
                new Panel(0, 2, PanelType.ThreeByOne),
                new Panel(0, 3, PanelType.TwoByTwo),
                new Panel(0, 5, PanelType.ThreeByOne),
                new Panel(0, 6, PanelType.OneByTwo),
                new Panel(1, 6, PanelType.TwoByTwo),
                new Panel(2, 3, PanelType.ThreeByTwo),
                new Panel(3, 0, PanelType.TwoByTwo),
                new Panel(3, 2, PanelType.TwoByOne),
                new Panel(3, 5, PanelType.TwoByTwo),
                new Panel(3, 7, PanelType.TwoByOne),
                new Panel(5, 0, PanelType.OneByThree),
                new Panel(5, 3, PanelType.TwoByOne),
                new Panel(5, 4, PanelType.TwoByTwo),
                new Panel(5, 6, PanelType.ThreeByTwo),
                new Panel(6, 0, PanelType.TwoByThree),
                new Panel(7, 3, PanelType.OneByThree)
            };

            return new Board(panels);
        }

        private static Board setupBoard4()
        {
            Panel[] panels =
            {
                new Panel(0, 0, PanelType.TwoByThree),
                new Panel(0, 3, PanelType.ThreeByOne),
                new Panel(0, 4, PanelType.TwoByTwo),
                new Panel(0, 6, PanelType.ThreeByTwo),
                new Panel(2, 0, PanelType.TwoByOne),
                new Panel(2, 1, PanelType.TwoByTwo),
                new Panel(2, 4, PanelType.TwoByTwo),
                new Panel(3, 3, PanelType.TwoByOne),
                new Panel(3, 6, PanelType.ThreeByOne),
                new Panel(3, 7, PanelType.TwoByOne),
                new Panel(4, 0, PanelType.TwoByThree),
                new Panel(4, 4, PanelType.TwoByTwo),
                new Panel(5, 3, PanelType.ThreeByOne),
                new Panel(5, 7, PanelType.ThreeByOne),
                new Panel(6, 0, PanelType.TwoByTwo),
                new Panel(6, 2, PanelType.TwoByOne),
                new Panel(6, 4, PanelType.TwoByThree)
            };

            return new Board(panels);
        }

        private static Board setupBoard5()
        {
            Panel[] panels =
            {
                new Panel(0, 0, PanelType.OneByThree),
                new Panel(0, 3, PanelType.OneByThree),
                new Panel(0, 6, PanelType.TwoByTwo),
                new Panel(1, 0, PanelType.TwoByTwo),
                new Panel(1, 2, PanelType.OneByTwo),
                new Panel(1, 4, PanelType.TwoByTwo),
                new Panel(2, 2, PanelType.TwoByTwo),
                new Panel(2, 6, PanelType.ThreeByTwo),
                new Panel(3, 0, PanelType.OneByTwo),
                new Panel(3, 4, PanelType.ThreeByTwo),
                new Panel(4, 0, PanelType.TwoByThree),
                new Panel(4, 3, PanelType.ThreeByOne),
                new Panel(5, 6, PanelType.ThreeByTwo),
                new Panel(6, 0, PanelType.TwoByTwo),
                new Panel(6, 2, PanelType.TwoByOne),
                new Panel(6, 4, PanelType.OneByTwo),
                new Panel(7, 3, PanelType.OneByThree)
            };

            return new Board(panels);
        }

        private static Board setupBoard6()
        {
            Panel[] panels =
            {
                new Panel(0, 0, PanelType.OneByThree),
                new Panel(0, 3, PanelType.ThreeByTwo),
                new Panel(0, 5, PanelType.OneByTwo),
                new Panel(0, 7, PanelType.ThreeByOne),
                new Panel(1, 0, PanelType.TwoByOne),
                new Panel(1, 1, PanelType.TwoByTwo),
                new Panel(1, 5, PanelType.TwoByTwo),
                new Panel(3, 0, PanelType.TwoByThree),
                new Panel(3, 3, PanelType.TwoByTwo),
                new Panel(3, 5, PanelType.TwoByThree),
                new Panel(5, 0, PanelType.ThreeByOne),
                new Panel(5, 1, PanelType.TwoByTwo),
                new Panel(5, 3, PanelType.ThreeByTwo),
                new Panel(5, 5, PanelType.TwoByTwo),
                new Panel(5, 7, PanelType.TwoByOne),
                new Panel(7, 1, PanelType.OneByTwo),
                new Panel(7, 5, PanelType.OneByThree)
            };

            return new Board(panels);
        }

        private static Board setupBoard7()
        {
            Panel[] panels =
            {
                new Panel(0, 0, PanelType.TwoByTwo),
                new Panel(0, 2, PanelType.TwoByThree),
                new Panel(0, 5, PanelType.ThreeByOne),
                new Panel(0, 6, PanelType.OneByTwo),
                new Panel(1, 6, PanelType.TwoByTwo),
                new Panel(2, 0, PanelType.ThreeByTwo),
                new Panel(2, 2, PanelType.TwoByOne),
                new Panel(2, 3, PanelType.TwoByTwo),
                new Panel(3, 5, PanelType.TwoByThree),
                new Panel(4, 2, PanelType.OneByThree),
                new Panel(5, 0, PanelType.OneByTwo),
                new Panel(5, 2, PanelType.ThreeByTwo),
                new Panel(5, 4, PanelType.ThreeByOne),
                new Panel(5, 5, PanelType.ThreeByOne),
                new Panel(5, 6, PanelType.OneByTwo),
                new Panel(6, 0, PanelType.TwoByTwo),
                new Panel(6, 6, PanelType.TwoByTwo)
            };

            return new Board(panels);
        }
    }
}