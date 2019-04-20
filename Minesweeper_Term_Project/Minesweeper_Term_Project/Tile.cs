using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Minesweeper_Term_Project
{
    /// <summary>
    /// enum for type of tile
    /// </summary>
    enum TileType
    {
        empty,
        number,
        bomb
    }

    enum TileStatus
    {
        Revealed,
        Hidden
    }

    class Tile
    {
        // Type of tile
        private TileType _tileType;

        // Image sourse
        private BitmapImage _tileSourceImage;

        // Number of bombs around this tile
        private byte _surroundingBombs;

        // Position of tile within the list
        private string[] _pos;
        private int _posX;
        private int _posY;

        // Status of tile
        private TileStatus _tileStatus;

        /// <summary>
        /// Constructor for tile
        /// </summary>
        /// <param name="tileType"></param>
        public Tile(TileType tileType, string pos)
        {
            _tileType = tileType;

            _pos = pos.Split(',');

            _posX = int.Parse(_pos[0]);

            _posY = int.Parse(_pos[1]);

            _tileStatus = TileStatus.Hidden;
        }

        /// <summary>
        /// TODO: Method that finds how many bombs are around the tile, then returning that number
        /// Also changing type depending if any bombs are or are not surrounding it
        /// </summary>
        /// <returns></returns>
        public void GetSurroundingBombs(List<List<Tile>> boardList, int rowPos, int colPos)
        {
            byte bombCount = 0;

            if (boardList[rowPos][colPos]._tileType != TileType.bomb)
            {
                // Column
                for (int ind = 0; ind < 3; ind++)
                {
                    // Check row for bombs
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            if (boardList[rowPos + 1 - i][colPos + 1 - ind]._tileType == TileType.bomb)
                            {
                                bombCount += 1;
                            }
                        }
                        // If checking for tile that does not exist: skip loop
                        catch (System.ArgumentOutOfRangeException)
                        {
                            continue;
                        }
                    }
                }

                if (bombCount > 0)
                {
                    _tileType = TileType.number;
                }

                _surroundingBombs = bombCount;
            }

        }

        public List<Tile> GetTouchingEmptySpaces(List<List<Tile>> boardList)
        {
            List<Tile> emptyTileList = new List<Tile>();

            // Column
            for (int ind = 0; ind < 3; ind++)
            {
                // Row
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        if (boardList[_posX + 1 - i][_posY + 1 - ind]._tileType == TileType.empty && boardList[_posX + 1 - i][_posY + 1 - ind].TileStatus == TileStatus.Hidden)
                        {
                            emptyTileList.Add(boardList[_posX + 1 - i][_posY + 1 - ind]);
                        }
                    }
                    // If checking for tile that does not exist: skip loop
                    catch (System.ArgumentOutOfRangeException)
                    {
                        continue;
                    }
                }
            }

            return emptyTileList;
        }

        /// <summary>
        /// Method to set image depending on what type it is
        /// </summary>
        public void SetImage()
        {
            switch (_tileType)
            {
                case TileType.empty:
                    _tileSourceImage = new BitmapImage(new Uri("ms-appx:///Assets/no0.png"));
                    break;

                case TileType.bomb:
                    _tileSourceImage = new BitmapImage(new Uri("ms-appx:///Assets/bomb.png"));
                    break;

                case TileType.number:
                    switch (_surroundingBombs)
                    {
                        case 1:
                            _tileSourceImage = new BitmapImage(new Uri("ms-appx:///Assets/no1.png"));
                            break;
                        case 2:
                            _tileSourceImage = new BitmapImage(new Uri("ms-appx:///Assets/no2.png"));
                            break;
                        case 3:
                            _tileSourceImage = new BitmapImage(new Uri("ms-appx:///Assets/no3.png"));
                            break;
                        case 4:
                            _tileSourceImage = new BitmapImage(new Uri("ms-appx:///Assets/no4.png"));
                            break;
                        case 5:
                            _tileSourceImage = new BitmapImage(new Uri("ms-appx:///Assets/no5.png"));
                            break;
                        case 6:
                            _tileSourceImage = new BitmapImage(new Uri("ms-appx:///Assets/no6.png"));
                            break;
                        case 7:
                            _tileSourceImage = new BitmapImage(new Uri("ms-appx:///Assets/no7.png"));
                            break;
                        case 8:
                            _tileSourceImage = new BitmapImage(new Uri("ms-appx:///Assets/no8.png"));
                            break;
                    }
                    break;
                    
            }
        }

        /// <summary>
        /// Get method for _tileType
        /// </summary>
        public TileType TileType { get { return _tileType; } }

        public BitmapImage TileSourceImage { get { return _tileSourceImage; } }

        public TileStatus TileStatus { get { return _tileStatus; } set { _tileStatus = TileStatus; } }
    }
}
