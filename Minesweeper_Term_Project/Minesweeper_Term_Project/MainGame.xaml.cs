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

        private List<Image> _imgControlList;

        private bool flaggedToggle = true;

        public MainGame()
        {
            this.InitializeComponent();

            // Initialize the image control list in XML when images are created.
            _imgControlList = new List<Image>();

            // XAML Create images for grid
            for (int rowi = 1; rowi < SIZE_OF_BOARD+1; rowi++)
            {
                for (int coli = 1; coli < SIZE_OF_BOARD + 1; coli++) {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri("ms-appx:///Assets/brick.png"));
                    img.Stretch = Stretch.Uniform;
                    img.Tapped += new TappedEventHandler(OnTap);
                    img.RightTapped += new RightTappedEventHandler(OnRightTap);
                    img.Tag = $"{rowi-1},{coli-1}";
                    Grid.SetColumn(img, coli);
                    Grid.SetRow(img, rowi);

                    
                    gameGrid.Children.Add(img);
                    _imgControlList.Add(img);
                }
            }

            // Create board
            _board = new Board(SIZE_OF_BOARD, DIFFICULTY, DIFFICULTY_MODIFIER);

            player = new MediaPlayer();
            playing = true;

            player.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/bgm2.mp3"));

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
                player.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/bgm2.mp3"));
                background_music.Content = "Mute Music";
                playing = true;
            }
        }

        private void OnRightTap(object sender, RightTappedRoutedEventArgs e)
        {
            Image tileObj = sender as Image;
            if (flaggedToggle == true)
            {
                tileObj.Source = new BitmapImage(new Uri("ms-appx:///Assets/flagged.png"));
                flaggedToggle = false;
            }
            else if (flaggedToggle == false)
            {
                tileObj.Source = new BitmapImage(new Uri("ms-appx:///Assets/brick.png"));
                flaggedToggle = true;
            }


        }
        private void OnTap(object sender, TappedRoutedEventArgs e)
        {
            // Get object of selected tile
            Image tileObj = sender as Image;
            string _tag = Convert.ToString(tileObj.Tag);
            // Position of tile that has been tapped (x,y)
            string[] pos = _tag.Split(',');

            Tile selectedTile = _board.BoardList[int.Parse(pos[0])][int.Parse(pos[1])];

            tileObj.Source = selectedTile.TileSourceImage;
            selectedTile.TileStatus = TileStatus.Revealed;

            // Empty space reveal TODO
            if (selectedTile.TileType == TileType.empty)
            {
                List<Tile> tempList = new List<Tile>();
                tempList = selectedTile.GetTouchingEmptySpaces(_board.BoardList);
                

                for (int item = 0; item < tempList.Count; item++)
                {
                    tempList[item].TileStatus = TileStatus.Revealed;
                }

                for (int item = 0; item < tempList.Count; item++)
                {
                    tempList.Concat(tempList[item].GetTouchingEmptySpaces(_board.BoardList));
                    
                    for (int imgItem = 0; imgItem < _imgControlList.Count; imgItem++)
                    {
                        if (_imgControlList[imgItem].Tag.ToString() == $"{tempList[item].GetPosX-1},{tempList[item].GetPosY-1}")
                        {
                            _imgControlList[imgItem].Source = tempList[item].TileSourceImage;
                        }
                    }
                    




                }
            }
        }

    }
}
