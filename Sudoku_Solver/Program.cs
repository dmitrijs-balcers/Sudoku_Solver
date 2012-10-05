using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Sudoku_Solver
{
    class Program
    {
        private static int[,] array = new int[3,3];

        static void Main(string[] args)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int y = 0; y < 3; y++)
                {
                    int number;
                    string input = Console.ReadLine();
                    if (Int32.TryParse(input, out number))
                    {
                        array[i, y] = number;
                    }
                    else 
                    {
                        array[i, y] = 0;
                    }
                    
                    Console.CursorTop--;
                    Console.CursorLeft = y + 1;
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            int count = 0;
            foreach (int item in array)
            {
                if (count == 3)
                {
                    Console.WriteLine();
                    count = 0;
                }
                Console.Write(item + "\t");
                count++;
            }

            Console.WriteLine();
           
            Console.WriteLine("Does not Contain Duplicates: " + ContainsDuplicates(array));

            Console.ReadLine();
        }

        public static bool ContainsDuplicates(int[,] arrayToCheck) 
        {
            ArrayList arrayList = new ArrayList();
            foreach (int item in array)
            {
                if (item != 0)
                {
                    if (arrayList.Contains(item))
                    {
                        return false;
                    }
                    arrayList.Add(item);
                }
            }
            return true;
        }
    }
}
