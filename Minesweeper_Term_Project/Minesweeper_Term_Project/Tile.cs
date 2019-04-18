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

    class Tile
    {
        // Type of tile
        private TileType _tileType;

        // Image sourse
        public BitmapImage _tileSourceImage;

        // Number of bombs around this tile
        public byte _surroundingBombs;

        /// <summary>
        /// Constructor for tile
        /// </summary>
        /// <param name="tileType"></param>
        public Tile(TileType tileType)
        {
            _tileType = tileType;
        }

        /// <summary>
        /// TODO: Method that finds how many bombs are around the tile, then returning that number
        /// Also changing type depending if any bombs are or are not surrounding it
        /// </summary>
        /// <returns></returns>
        public void GetSurroundingBombs(List<List<Tile>> boardList)
        {
            _surroundingBombs = 0;
        }

        /// <summary>
        /// Method to set image depending on what type it is
        /// </summary>
        public void SetImage()
        {
            switch (_tileType)
            {
                case TileType.empty:
                    _tileSourceImage = new BitmapImage();
                    break;

                case TileType.bomb:
                    _tileSourceImage = new BitmapImage();
                    break;

                case TileType.number:
                    switch (_surroundingBombs)
                    {
                        case 1:
                            _tileSourceImage = new BitmapImage();
                            break;
                        case 2:
                            _tileSourceImage = new BitmapImage();
                            break;
                        case 3:
                            _tileSourceImage = new BitmapImage();
                            break;
                        case 4:
                            _tileSourceImage = new BitmapImage();
                            break;
                        case 5:
                            _tileSourceImage = new BitmapImage();
                            break;
                        case 6:
                            _tileSourceImage = new BitmapImage();
                            break;
                        case 7:
                            _tileSourceImage = new BitmapImage();
                            break;
                        case 8:
                            _tileSourceImage = new BitmapImage();
                            break;
                    }
                    break;
                    
            }
        }

        /// <summary>
        /// Get method for _tileType
        /// </summary>
        public TileType TileType { get { return _tileType; } }
    }
}
