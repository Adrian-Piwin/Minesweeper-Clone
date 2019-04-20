using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Minesweeper_Term_Project
{

    class Board
    {
        // 2D array to repersent the board
        private List<List<Tile>> boardList;

        private static Random s_randomizer;

        private int _boardSize;

        private int _difficulty;

        private int _difficultyModifier;

        /// <summary>
        /// Constructor for board class
        /// </summary>
        public Board(int boardSize, int difficulty, int difficultyModifier)
        {
            _boardSize = boardSize;

            _difficulty = difficulty;

            _difficultyModifier = difficultyModifier;

            boardList = new List<List<Tile>>();

            s_randomizer = new Random();

            generateBoard();
        }

        /// <summary>
        /// Generate board for 2d array
        /// </summary>
        private void generateBoard()
        {
            int boardSize = _boardSize * _boardSize;

            int boardDifficulty = s_randomizer.Next(_difficulty, _difficultyModifier);

            int tempBomb = boardDifficulty / _boardSize;

            // Add tiles to board list
            for (int i = 0; i < _boardSize; i++)
            {
                List<Tile> subList = new List<Tile>();
                for (int ind = 0; ind < _boardSize; ind++)
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

                tempBomb = boardDifficulty / _boardSize;

                subList = RandomizeTiles(subList);
                boardList.Add(subList);
            }

            // Loop through each tile getting surrounding bombs and setting image
            for (int rowi = 0; rowi < boardList.Count; rowi++)
            {
                for (int coli = 0; coli < boardList[rowi].Count; coli++)
                {
                    boardList[rowi][coli].GetSurroundingBombs(boardList, rowi, coli);
                    //tile.SetImage();
                }

            }
            Debug.WriteLine(boardList);
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
