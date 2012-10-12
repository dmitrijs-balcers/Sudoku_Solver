using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku_Solver
{
    class ImStuckException : Exception
    {
        public override string ToString()
        {

            return "[EXCEPTION] Im Stuck";
        }
    }
}
