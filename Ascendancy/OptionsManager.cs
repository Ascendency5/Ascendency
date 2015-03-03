using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ascendancy.User_Controls;

namespace Ascendancy
{
    public static class OptionsManager
    {
        public static double SoundVolume { get; set; }
        public static double MusicVolume { get; set; }
           public static OptionsUserControl GlobalOptions;

        //todo: get justin;s opinion on passsing options
        static OptionsManager()
        {
            GlobalOptions = new OptionsUserControl();

        }
        //put "sound effects" info here if/when we need it
        //todo: try to get "home screen theme song" + "gameboard theme song" synced and awesome

    }
}
