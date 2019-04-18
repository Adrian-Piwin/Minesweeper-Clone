using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Playback;
using Windows.Media.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Minesweeper_Term_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainGame : Page
    {
        MediaPlayer player;
        bool playing;
        
        public MainGame()
        {
            this.InitializeComponent();
            player = new MediaPlayer();
            playing = true;

            player.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/bensound-erf.mp3"));

            player.AutoPlay = true;
        }

        private void Play_music_Tapped(object sender, TappedRoutedEventArgs e)
        {

            if (playing)
            {
                player.Source = null;
                background_music.Content = "Play Music";
                playing = false;
            }
            else
            {
                player.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/bensound-erf.mp3"));
                playing = true;
            }
        }
    }
}
