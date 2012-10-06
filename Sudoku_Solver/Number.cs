using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Sudoku_Solver
{
    class Number
    {
        public int number { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool initial { get; set; }
        public bool isEmpty { get; set; }
        public ArrayList buffer { get; set; }
    }
}
