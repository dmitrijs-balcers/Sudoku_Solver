using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Sudoku_Solver
{
    class Number : ICloneable
    {
        public Number(int x, int y, int number, bool initial, bool isEmpty) 
        {
            buffer = new ArrayList();
            this.x = x;
            this.y = y;
            this.number = number;
            this.initial = initial;
            this.isEmpty = isEmpty;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int number { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool initial { get; set; }
        public bool isEmpty { get; set; }
        public ArrayList buffer { get; set; }

        public String toString() {
            string ret = "|num: " + number + " x|y - " + x + "|" + y + ". " + " isEmpty: " + isEmpty + ". Buffer: ";
            int[] array = buffer.ToArray(typeof(int)) as int[];
            foreach (int value in array)
            {
                ret += value + ", ";
            }

            return ret;
        }
    }
}
