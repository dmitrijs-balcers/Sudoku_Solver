﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku_Solver
{
    class EmptyBufferForEmptyNumberException : Exception
    {
        public override string ToString()
        {
            return "Empty Buffer For Empty Number Exception";
        }
    }
}
