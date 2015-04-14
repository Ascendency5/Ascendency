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

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for IntroUserControl.xaml
    /// </summary>
    public partial class IntroUserControl : UserControl
    {
        public EventHandler OnComplete;

        public IntroUserControl()
        {
            InitializeComponent();
        }

        private void IntroStoryboard_OnCompleted(object sender, EventArgs e)
        {
            stopIntro();
        }

        public void stopIntro()
        {
            HomeScreenIntro.Stop();
            // Todo refactor this
            HomeScreenIntro.Volume = .7;
            HomeScreenIntro.Close(); 

            ContentControlActions.FadeOut();
            OnComplete(this, new EventArgs());
        }
    }
}
