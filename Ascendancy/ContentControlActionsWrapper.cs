using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ascendancy
{
    public static class ContentControlActionsWrapper
    {
        public static ContentControl baseContentControl { get; set; }
        private static UserControl baseControl;

        public static void setUpControl(UserControl control)
        {
            baseContentControl.Content = control;
            baseControl = control;

            Panel.SetZIndex(baseContentControl, 3);
            UserControlAnimation.FadeInContentControl(baseContentControl, true);
        }

        public static void FadeOut()
        {
            //animate the ContentControl from the HomeScreenWindow, then kill anim object
            UserControlAnimation.FadeInContentControl(baseContentControl, false);

            //set the HomeScreenContentControl to back, then empty it
            Panel.SetZIndex(baseContentControl, 2);
        }
    }
}
