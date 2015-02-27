using System;

namespace Ascendancy.Game_Engine
{
    public enum PanelType
    {
        OneByTwo,
        TwoByOne,
        OneByThree,
        ThreeByOne,
        TwoByTwo,
        TwoByThree,
        ThreeByTwo
    }

    public static class PanelTypeExtensions
    {
        public static int GetRows(this PanelType type)
        {
            switch (type)
            {
                case PanelType.OneByTwo:
                    return 1;
                case PanelType.TwoByOne:
                    return 2;
                case PanelType.OneByThree:
                    return 1;
                case PanelType.ThreeByOne:
                    return 3;
                case PanelType.TwoByTwo:
                    return 2;
                case PanelType.TwoByThree:
                    return 2;
                case PanelType.ThreeByTwo:
                    return 3;
            }
            return 0;
        }

        public static int GetColumns(this PanelType type)
        {
            switch (type)
            {
                case PanelType.OneByTwo:
                    return 2;
                case PanelType.TwoByOne:
                    return 1;
                case PanelType.OneByThree:
                    return 3;
                case PanelType.ThreeByOne:
                    return 1;
                case PanelType.TwoByTwo:
                    return 2;
                case PanelType.TwoByThree:
                    return 3;
                case PanelType.ThreeByTwo:
                    return 2;
            }
            return 0;
        }
    }
}
