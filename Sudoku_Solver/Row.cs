using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Sudoku_Solver
{
    class Row
    {
        public Row(int y) 
        {
            array = new ArrayList();
            this.y = y;
        }

        public ArrayList array { get; set; }
        public int y { get; set; }
    }
}
