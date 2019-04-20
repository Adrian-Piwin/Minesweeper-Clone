using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper_Term_Project
{
    class Number : Tile
    {

        private List<byte> _numberList;
        public Number(TileType tileType): base(tileType)
        {

        }
    }
}
