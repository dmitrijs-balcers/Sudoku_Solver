using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku_Solver
{
    class PossibleNumber
    {
        public PossibleNumber(Number number, int integer)
        {
            this.number = number;
            this.integer = integer;
        }

        public Number number { get; set; }
        public int integer { get; set; }
    }
}
