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
            if (((tileObj).Source as BitmapImage).UriSource.ToString() == "ms-appx:///Assets/brick.png")
            {
                tileObj.Source = new BitmapImage(new Uri("ms-appx:///Assets/flagged.png"));
            }
            else if (((tileObj).Source as BitmapImage).UriSource.ToString() == "ms-appx:///Assets/flagged.png")
            {
                tileObj.Source = new BitmapImage(new Uri("ms-appx:///Assets/brick.png"));
            }


        }
        private void OnTap(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine(_board.BoardList.Count().ToString());
            // Get object of selected tile
            Image tileObj = sender as Image;
            string _tag = Convert.ToString(tileObj.Tag);
            // Position of tile that has been tapped (x,y)
            string[] pos = _tag.Split(',');

            // Check if tile is brick
            if (((tileObj).Source as BitmapImage).UriSource.ToString() == "ms-appx:///Assets/brick.png")
            {

                Tile selectedTile = _board.BoardList[int.Parse(pos[0])][int.Parse(pos[1])];

                tileObj.Source = selectedTile.TileSourceImage;
                selectedTile.TileStatus = TileStatus.Revealed;

                // If bomb is clicked, reveal all other bombs and end game
                if (selectedTile.TileType == TileType.bomb)
                {
                    RevealBombs();
                }


                // Empty space reveal
                
                if (selectedTile.TileType == TileType.empty)
                {
                    List<Tile> tempList = new List<Tile>();
                    List<Tile> tempNumList = new List<Tile>();
                    tempList.Add(selectedTile);
                    Tuple<List<Tile>,List<Tile>> listTuple = selectedTile.GetTouchingEmptySpaces(_board.BoardList, tempList);

                    tempList = listTuple.Item1;
                    tempNumList = listTuple.Item2;

                    // Empty spaces
                    for (int item = 1; item < tempList.Count; item++)
                    {
                        for (int imgItem = 0; imgItem < _imgControlList.Count; imgItem++)
                        {
                            if (_imgControlList[imgItem].Tag.ToString() == $"{tempList[item].GetPosY},{tempList[item].GetPosX}")
                            {
                                _imgControlList[imgItem].Source = tempList[item].TileSourceImage;
                            }
                        }
                    }

                    for (int item = 0; item < tempNumList.Count; item++)
                    {
                        for (int imgItem = 0; imgItem < _imgControlList.Count; imgItem++)
                        {
                            if (_imgControlList[imgItem].Tag.ToString() == $"{tempNumList[item].GetPosY},{tempNumList[item].GetPosX}")
                            {
                                _imgControlList[imgItem].Source = tempNumList[item].TileSourceImage;
                            }
                        }
                    }
                }
            }
        }

        private void RevealBombs()
        {

            for (int i =0; i< _board.BoardList.Count(); i++)
            {
                 for (int j =0; j < _board.BoardList[i].Count(); j++)
                {
                   if( _board.BoardList[i][j].TileType == TileType.bomb)
                    {
                        _board.BoardList[i][j].TileStatus = TileStatus.Revealed;
                        for (int k = 0; k< _imgControlList.Count; k++)
                        {
                            if (_imgControlList[k].Tag.ToString() == $"{ _board.BoardList[i][j].GetPosY},{ _board.BoardList[i][j].GetPosX}")
                            {
                                _imgControlList[k].Source = _board.BoardList[i][j].TileSourceImage;
                                Debug.WriteLine(_imgControlList[k].Tag.ToString());
                                break;
                            }
                        }
                    }
                }
            }
        }

    }
}
