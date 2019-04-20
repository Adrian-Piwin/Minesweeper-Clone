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
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;

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

        // Size of the board numxnum
        private const int SIZE_OF_BOARD = 10;

        // Diffifculty of game (Percentage of bombs randomly between chosen difficulty + modifier)
        private const int DIFFICULTY = 10;

        private const int DIFFICULTY_MODIFIER = 10;

        private Board _board;

        public MainGame()
        {
            this.InitializeComponent();
            player = new MediaPlayer();
            playing = true;

            player.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/bensound-erf.mp3"));

            player.AutoPlay = true;

            // XAML Create images for grid
            for (int rowi = 1; rowi < SIZE_OF_BOARD+1; rowi++)
            {
                for (int coli = 1; coli < SIZE_OF_BOARD + 1; coli++) {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri("ms-appx:///Assets/brick.png"));
                    img.Stretch = Stretch.Uniform;
                    img.Tapped += new TappedEventHandler(OnTap);
                    img.Tag = $"{rowi-1}{coli-1}";
                    Grid.SetColumn(img, coli);
                    Grid.SetRow(img, rowi);

                    gameGrid.Children.Add(img);
                }
            }

            // Create board
            _board = new Board(SIZE_OF_BOARD, DIFFICULTY, DIFFICULTY_MODIFIER);
            
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

        private void OnTap(object sender, TappedRoutedEventArgs e)
        {
            // Get object of selected tile
            Image tileObj = sender as Image;
            byte _tag = Convert.ToByte(tileObj.Tag);
        }

    }
}
