﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku_Solver
{
    class SolvingFailedException : Exception
    {
        public override string ToString()
        {
            return "[EXCEPTION] Sudoku Solving Failed";
        }
    }
}
