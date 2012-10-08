using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Sudoku_Solver
{
    class Column
    {
        public Column(int x) 
        {
            array = new ArrayList();
            this.x = x;
        }

        public ArrayList array { get; set; }
        public int x { get; set; }
    }
}
