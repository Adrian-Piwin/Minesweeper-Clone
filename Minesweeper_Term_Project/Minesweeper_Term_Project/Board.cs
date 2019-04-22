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
            int boardDifficulty = s_randomizer.Next(_difficulty, _difficulty + _difficultyModifier);

            int tempBomb = boardDifficulty / _boardSize;

            // Add tiles to board list
            for (int i = 0; i < _boardSize; i++)
            {
                List<Tile> subList = new List<Tile>();
                for (int ind = 0; ind < _boardSize; ind++)
                {
                    subList.Add(new Tile(TileType.empty, $"{ind},{i}"));
                }

                subList = RandomizeTiles(subList, tempBomb);
                boardList.Add(subList);
            }

            // Loop through each tile getting surrounding bombs and setting image
            for (int rowi = 0; rowi < boardList.Count; rowi++)
            {
                for (int coli = 0; coli < boardList[rowi].Count; coli++)
                {
                    boardList[rowi][coli].GetSurroundingBombs(boardList, rowi, coli);
                    boardList[rowi][coli].SetImage();
                }

            }
            Debug.WriteLine(boardList);
        }

        /// <summary>
        /// Set random tiles to bomb type
        /// </summary>
        /// <param name="tileList"></param>
        public List<Tile> RandomizeTiles(List<Tile> tileList, int bombCount)
        {

            while (bombCount > 0)
            {
                int index = s_randomizer.Next(tileList.Count);
                tileList[index]._tileType = TileType.bomb;

                bombCount -= 1;
            }

            return tileList;
        }

        public List<List<Tile>> BoardList { get { return boardList; } }
    }
}
