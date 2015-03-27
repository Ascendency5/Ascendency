using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Ascendancy.User_Controls;

namespace Ascendancy
{
    public static class VolumeManager
    {
        private static double _soundVolume = .5;
        private static double _musicVolume = .5;

        public static double SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                _soundVolume = value;
                if(OnVolumeChanged != null)
                    OnVolumeChanged(null, new VolumeChangeEventArgs(SoundType.SoundEffect, value));
            }
        }
        public static double MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                _musicVolume = value;
                if (OnVolumeChanged != null)
                    OnVolumeChanged(null, new VolumeChangeEventArgs(SoundType.Music, value));
            }
        }

        private static EventHandler<VolumeChangeEventArgs> OnVolumeChanged = on_volume_changed;

        private static readonly ConcurrentDictionary<MediaPlayer, SoundConfiguration> currentMediaPlayers = new ConcurrentDictionary<MediaPlayer, SoundConfiguration>();

        public static void play(string uriString, SoundType type = SoundType.SoundEffect, SoundLoop loop = SoundLoop.None)
        {
            play(new Uri(uriString, UriKind.Relative), type, loop);
        }

        public static void play(Uri uri, SoundType type = SoundType.SoundEffect, SoundLoop loop = SoundLoop.None)
        {
            MediaPlayer player = new MediaPlayer();
            player.Open(uri);
            play(player, type, loop);
        }

        public static void play(MediaPlayer player, SoundType type = SoundType.SoundEffect, SoundLoop loop = SoundLoop.None)
        {
            SoundConfiguration configuration = new SoundConfiguration(type, loop);
            currentMediaPlayers[player] = configuration;

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
            if (!currentMediaPlayers.ContainsKey(player)) return;

            SoundConfiguration configuration = currentMediaPlayers[player];
            if (configuration.Loop == SoundLoop.Loop)
            {
                player.Position = TimeSpan.FromMilliseconds(1);
                player.Play();
            }
            else
            {
                currentMediaPlayers.TryRemove(player, out configuration);
            }
        }

        private static void on_volume_changed(object sender, VolumeChangeEventArgs e)
        {
            currentMediaPlayers
                .Where(x => x.Value.Type == e.Type)
                .Select(x => x.Key)
                .ToList()
                .ForEach(x => x.Volume = e.Volume);
        }
    }

    class VolumeChangeEventArgs : EventArgs
    {
        public SoundType Type { get; private set; }
        public double Volume { get; private set; }

        public VolumeChangeEventArgs(SoundType type, double volume)
        {
            Type = type;
            Volume = volume;
        }
    }

    class SoundConfiguration
    {
        public SoundType Type { get; private set; }
        public SoundLoop Loop { get; private set; }

        public SoundConfiguration(SoundType soundType, SoundLoop soundLoop)
        {
            Type = soundType;
            Loop = soundLoop;
        }
    }

    public enum SoundType
    {
        Music,
        SoundEffect
    }

    public enum SoundLoop
    {
        None,
        Loop
    }
}
