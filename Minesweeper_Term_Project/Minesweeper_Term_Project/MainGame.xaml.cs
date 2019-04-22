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

        private DispatcherTimer gameTimer;

        private int score = 0;

        private int highscore = 0;

        private int flaggedCount = 0;

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

            // Create game timer
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += MinesweeperTimer;
            gameTimer.Start();

            // Bomb remaining count
            MineSweeperBombs();
        }

        /// <summary>
        /// Event handler if the user clicks on the play/pause music button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Event handler if the user right clicks on a tile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRightTap(object sender, RightTappedRoutedEventArgs e)
        {
            Image tileObj = sender as Image;
            if (((tileObj).Source as BitmapImage).UriSource.ToString() == "ms-appx:///Assets/brick.png")
            {
                tileObj.Source = new BitmapImage(new Uri("ms-appx:///Assets/flagged.png"));
                flaggedCount += 1;
            }
            else if (((tileObj).Source as BitmapImage).UriSource.ToString() == "ms-appx:///Assets/flagged.png")
            {
                tileObj.Source = new BitmapImage(new Uri("ms-appx:///Assets/brick.png"));
                flaggedCount -= 1;
            }

            MineSweeperBombs();
        }

        /// <summary>
        /// Event handler if the user clicks on a tile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTap(object sender, TappedRoutedEventArgs e)
        {
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
                    //RevealBombs();
                    EndGame(false);

                }

                // Check if the game is won
                if (CheckWinner())
                {
                    EndGame(true);
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

        /// <summary>
        /// Reveals all bombs on the board
        /// </summary>
        private void RevealBombs()
        {
            foreach (List<Tile> itemList in _board.BoardList)
            {
                foreach (Tile item in itemList)
                {
                    if (item.TileType == TileType.bomb)
                    {
                        item.TileStatus = TileStatus.Revealed;
                        foreach (Image img in _imgControlList)
                        {
                            if (img.Tag.ToString() == $"{item.GetPosX},{item.GetPosY}")
                            {
                                img.Source = item.TileSourceImage;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the game is won
        /// </summary>
        private bool CheckWinner()
        {
            for (int row = 0; row < _board.BoardList.Count; row++)
            {
                for (int col = 0; col < _board.BoardList[row].Count; col++)
                {
                    Tile currentTile = _board.BoardList[row][col];
                    if (currentTile.TileStatus == TileStatus.Hidden && currentTile._tileType != TileType.bomb)
                    {
                        // Something that is not a bomb is still hidden - game not won/lost yet
                        return false;
                    }
                }
            }

            // Everything that is not a bomb is revealed - game won
            return true;
        }

        /// <summary>
        /// Show result of game, ask to play again
        /// </summary>
        /// <param name="result"></param>
        private async void EndGame(bool winner)
        {
            gameTimer.Stop();

            string message;
            string message2;

            // Game is won
            if (winner)
            {
                message = "You win!";
                message2 = $"Highscore: {highscore}\nYour Score: {score}";
            }
            // Game is lost
            else
            {
                message = "You lost!";
                message2 = $"Highscore: {highscore}";

            }

            // Update high score
            if (score < highscore)
            {
                highscore = score;
            }

            ContentDialog gameResult = new ContentDialog
            {
                Title = message,
                Content = $"{message2}\n\nWould you like to play again?",
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            };

            ContentDialogResult result = await gameResult.ShowAsync();

            player.Source = null;

            if (result == ContentDialogResult.Primary)
            {
                this.Frame.Navigate(typeof(MainGame));
            }
            else
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        /// <summary>
        /// Every second, add to timer and display it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinesweeperTimer(object sender, object e)
        {
            score += 1;
            timer_display.Text = $"Time: {score}";
        }

        private void MineSweeperBombs()
        {
            bomb_display.Text = $"Bombs: {_board.BoardDifficulty - flaggedCount}";
        }

    }
}
