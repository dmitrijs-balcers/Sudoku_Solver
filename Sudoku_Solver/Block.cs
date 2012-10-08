using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Sudoku_Solver
{
    class Block
    {
        public Block(int x, int y) 
        {
            this.al = new ArrayList();
            this.x = x;
            this.y = y;
        }

        public ArrayList al { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}
