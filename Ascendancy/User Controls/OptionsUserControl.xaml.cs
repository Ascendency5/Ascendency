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

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for ExitConfirmationUserControl.xaml
    /// </summary>
    public partial class OptionsUserControl : UserControl
    {

        public OptionsUserControl()
        {
            InitializeComponent();

            updateMusicSlider(VolumeManager.MusicVolume);
            updateSoundSlider(VolumeManager.SoundVolume);
        }

        private void updateSoundSlider(double volume)
        {
            SoundSlider.Value = volume;
            SoundPercentageLabel.Content = volume*100 + "%";
        }

        private void updateMusicSlider(double volume)
        {
            MusicSlider.Value = volume;
            MusicPercentageLabel.Content = volume*100 + "%";
        }

        private void SoundSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            updateSoundSlider(SoundSlider.Value);
            VolumeManager.SoundVolume = SoundSlider.Value;
            
        }

        private void MusicSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            updateMusicSlider(MusicSlider.Value);
            VolumeManager.MusicVolume = MusicSlider.Value;
        }

        private void Okay_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Panel.SetZIndex(Okay, 0);
            ContentControlActions.FadeOut();
        }
    }
}
