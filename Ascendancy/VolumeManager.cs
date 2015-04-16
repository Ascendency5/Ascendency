using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Ascendancy.User_Controls;

namespace Ascendancy
{
    public static class VolumeManager
    {
        private static double _soundVolume = .60;
        private static double _musicVolume = .60;
        private static bool _battleThemeIsPlaying;

        public static double SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                _soundVolume = value;
                if (OnVolumeChanged != null)
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
                    OnVolumeChanged(null, new VolumeChangeEventArgs(SoundType.MenuMusic, value));
            }
        }

        public static bool PlayBattleTheme
        {
            get { return _battleThemeIsPlaying; }
            set
            {
                //swap the values of the battle music and the menu music
                //should swap a zero with the current global volume

                _battleThemeIsPlaying = value;
                if (_battleThemeIsPlaying)
                {
                    TransitionAudio(SoundType.MenuMusic, SoundType.BattleMusic);
                }
                else
                {
                    TransitionAudio(SoundType.BattleMusic, SoundType.MenuMusic);
                }
            }
        }

        private static void TransitionAudio(SoundType fromType, SoundType toType)
        {
            // Create a storyboard
            //Storyboard storyboard = new Storyboard();
            // Reset all audio of the toType to the start

            // Transition from MusicVolume to 0 for fromType
            var fromPlayers = currentMediaPlayers.Where(x => x.Value.Type == fromType).ToArray();

            // Transition from 0 to MusicVolume for toType
            var toPlayers = currentMediaPlayers.Where(x => x.Value.Type == toType).ToArray();

            SetUpAnimation(fromPlayers, MusicVolume, 0);
            SetUpAnimation(toPlayers, 0, MusicVolume);

            foreach (var player in fromPlayers)
            {
                player.Key.Position = TimeSpan.FromMilliseconds(1);
                player.Key.Play();
            }

            //storyboard.Begin();
        }

        private static void SetUpAnimation(
            KeyValuePair<MediaPlayer, SoundConfiguration>[] fromPlayers,
            double fromVolume,
            double toVolume)
        {
            foreach (var player in fromPlayers)
            {
                //var animation = new DoubleAnimation(fromVolume, toVolume, TimeSpan.FromMilliseconds(400));
                //Storyboard.SetTarget(animation, player.Key);
                //Storyboard.SetTargetProperty(animation, new PropertyPath(MediaPlayerVolumeProperty));
                //storyboard.Children.Add(animation);
                player.Key.Volume = toVolume;
            }
        }

        private static readonly EventHandler<VolumeChangeEventArgs> OnVolumeChanged = on_volume_changed;

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
                //todo make music transition flow correctly
                case SoundType.BattleMusic:
                    player.Volume = PlayBattleTheme ? MusicVolume : 0;
                    break;
                case SoundType.MenuMusic:
                    player.Volume = PlayBattleTheme ? 0 : MusicVolume;
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
        BattleMusic,
        MenuMusic,
        SoundEffect
    }

    public enum SoundLoop
    {
        None,
        Loop
    }
}
