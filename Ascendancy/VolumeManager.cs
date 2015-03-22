using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Ascendancy.User_Controls;

namespace Ascendancy
{
    public static class VolumeManager
    {
        public static double SoundVolume { get; set; }
        public static double MusicVolume { get; set; }

        private static readonly ConcurrentSet<MediaPlayer> currentMediaPlayers = new ConcurrentSet<MediaPlayer>();

        public static void play(MediaPlayer player, SoundType type = SoundType.SoundEffect)
        {
            currentMediaPlayers.Add(player);
            switch (type)
            {
                case SoundType.Music:
                    player.Volume = MusicVolume;
                    break;
                case SoundType.SoundEffect:
                    player.Volume = SoundVolume;
                    break;
            }
            player.MediaEnded += on_player_end;
            player.Play();
        }

        private static void on_player_end(object sender, EventArgs e)
        {
            MediaPlayer player = (MediaPlayer) sender;
            if (currentMediaPlayers.Contains(player))
            {
                currentMediaPlayers.Remove(player);
            }
        }
    }

    public enum SoundType
    {
        Music,
        SoundEffect
    }
}
