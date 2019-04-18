using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper_Term_Project
{

    class Board
    {
        // 2D array to repersent the board
        private List<List<Tile>> boardList;

        private static Random s_randomizer;

        // Size of the board numxnum
        public static int SIZE_OF_BOARD = 10;

        // Diffifculty of game (Percentage of bombs randomly between chosen difficulty + modifier)
        public static int DIFFICULTY = 10;

        public static int DIFFICULTY_MODIFIER = 10;

        /// <summary>
        /// Constructor for board class
        /// </summary>
        public Board()
        {
            boardList = new List<List<Tile>>();

            s_randomizer = new Random();

            generateBoard();
        }

        /// <summary>
        /// Generate board for 2d array
        /// </summary>
        private void generateBoard()
        {
            int boardSize = SIZE_OF_BOARD * SIZE_OF_BOARD;

            int boardDifficulty = s_randomizer.Next(DIFFICULTY, DIFFICULTY_MODIFIER);

            int tempBomb = boardDifficulty / SIZE_OF_BOARD;

            // Add tiles to board list
            for (int i = 0; i < SIZE_OF_BOARD; i++)
            {
                List<Tile> subList = new List<Tile>();
                for (int ind = 0; ind < SIZE_OF_BOARD; i++)
                {
                    if (tempBomb != 0)
                    {
                        subList.Add(new Tile(TileType.bomb));
                        tempBomb -= 1;
                    }
                    else
                    {
                        subList.Add(new Tile(TileType.empty));
                    }
                }

                tempBomb = boardDifficulty / SIZE_OF_BOARD;

                subList = RandomizeTiles(subList);
                boardList.Add(subList);
            }

            // Loop through each tile getting surrounding bombs and setting image
            foreach (List<Tile> sublist in boardList)
            {
                foreach (Tile tile in sublist)
                {
                    tile.GetSurroundingBombs(boardList);
                    tile.SetImage();
                }

            }
        }

        /// <summary>
        /// Random tiles in list
        /// </summary>
        /// <param name="tileList"></param>
        public List<Tile> RandomizeTiles(List<Tile> tileList)
        {
            List<Tile> shuffledList = new List<Tile>();

            // randomly remove items from first list
            // and add them to the second list
            while (tileList.Count > 0)
            {
                int index = s_randomizer.Next(tileList.Count);
                shuffledList.Add(tileList[index]);
                tileList.RemoveAt(index);
            }

            return shuffledList;
        }

        /// <summary>
        /// Get method for 2D array of the board list
        /// </summary>
        public List<List<string>> BoardList { get; }
    }
}
