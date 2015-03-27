using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Ascendancy.User_Controls;
using Panel = System.Windows.Controls.Panel;
using UserControl = System.Windows.Controls.UserControl;

namespace Ascendancy
{
    public static class ContentControlActions
    {
        public static ContentControl baseContentControl { get; set; }
        public static ContentControl popupContentControl { get; set; }

        public static bool IsPopupVisible
        {
            get { return popupControls.Count != 0; }
        }

        private static readonly Stack<UserControl> popupControls = new Stack<UserControl>();
        private static HomeScreenUserControl menuUserControl;

        public static void setPopup(UserControl control)
        {
            popupControls.Push(control);
            popupContentControl.Content = control;

            FadeInControl(popupContentControl, 7);
        }

        public static void setUpControl(UserControl control)
        {
            // Kill the popups if any are in the way
            if (popupControls.Count != 0)
            {
                popupControls.Clear();
                FadeOutControl(popupContentControl);
            }

            /*
             * todo This is definitely a hack
             * It works by seeing if the current
             * content is the menu, and stores it
             * if it is to be restored on fade out.
             */

            HomeScreenUserControl homeScreen = baseContentControl.Content as HomeScreenUserControl;
            if (homeScreen != null)
            {
                menuUserControl = homeScreen;
            }

            baseContentControl.Content = control;

            FadeInControl(baseContentControl, 2);
        }

        public static void FadeOut()
        {
            /**
             * todo Possible hack
             * This code assumes that the base content control can't be changed if
             * there is a popup window above. In cases of moving to a game screen,
             * the popup window has to completely get rid of all previous popup
             * windows before changing the base control. Because of that, the popup
             * window stack is cleared whenever the base content is changed. When a
             * popup window 
             */
            if (popupControls.Count == 0)
            {
                // The current control is the base one
                FadeOutControl(baseContentControl);

                // Do we need to set the menu back up?
                if (menuUserControl == null) return;

                baseContentControl.Content = menuUserControl;
                FadeInControl(baseContentControl, 2);
                menuUserControl = null;
            }
            else
            {
                // Remove the top UserControl
                popupControls.Pop();

                FadeOutControl(popupContentControl);

                // Don't fade in the next control if we don't have one
                if (popupControls.Count == 0) return;

                // Get the previous one on the stack
                popupContentControl.Content = popupControls.Peek();
                FadeInControl(popupContentControl, 7);
            }
        }

        private static void FadeInControl(ContentControl control, int index)
        {
            // Bring the control up front
            Panel.SetZIndex(control, index);

            UserControlAnimation.FadeInContentControl(control);
        }

        private static void FadeOutControl(ContentControl control)
        {
            UserControlAnimation.FadeOutContentControl(control);

            //set the Content control to the back
            Panel.SetZIndex(control, 0);
        }
    }
}
