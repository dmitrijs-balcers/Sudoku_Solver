using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Sudoku_Solver
{
    class Program
    {
        private static ArrayList array = new ArrayList();
        private const short A = 6;

        static void Main(string[] args)
        {

            for (int y = 0; y < A; y++)
            {
                for (int x = 0; x < A; x++)
                {
                    Number num = new Number();
                    num.y = y;
                    num.x = x;

                    int number;
                    string input = Console.ReadLine();
                    if (input != "" && Int32.TryParse(input, out number))
                    {
                        num.number = number;
                        num.initial = true;
                    }
                    else
                        num.isEmpty = true;
                    array.Add(num);
                    
                    Console.CursorTop--;
                    Console.CursorLeft = x + 1;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            PrintGrid();
            Console.WriteLine("Contains Duplicates: " + ContainsDuplicates(array));

            CountFilledNumbersIn3x3();

            ArrayList tempArray = new ArrayList();
            for (int y1 = 1; y1 < 3; y1++)
            {
                for (int x1 = 1; x1 < 3; x1++)
                {
                    foreach (Number item in array)
                    {
                        if ((item.x < x1 * 3 && item.x > (x1 - 1) * 3) && (item.y < y1 * 3 && item.y > (y1 - 1) * 3))
                        {
                            Console.Write(x1 + ", " + y1 + "n: " + item.number +"| ");
                            tempArray.Add(item);
                        }
                    }
                    Console.WriteLine("Temp Array " + y1 + "," + x1 +"Contains Duplicates: " + ContainsDuplicates(tempArray));
                    tempArray = new ArrayList();
                }
            }

            foreach (Number item in array)
            {
                if (item.isEmpty == true)
                {
                    for (int i = 1; i < 10; i++)
                    {
                        item.number = i;
                        if (!ContainsDuplicates(array))
                        {
                            PrintGrid();
                            Console.WriteLine("NumberFound!!!" + item.number);
                            break;
                        }
                    }
                }
            }
            Console.ReadLine();
        }

        private static void CountFilledNumbersIn3x3()
        {
            int numbersFilled = 0;
            if (!ContainsDuplicates(array))
            {
                foreach (Number item in array)
                    if (item.number != 0)
                        numbersFilled++;
            }
        }

        private static void PrintGrid()
        {
            int count = 0;
            foreach (Number item in array)
            {
                if (count == A)
                {
                    Console.WriteLine();
                    count = 0;
                }
                Console.Write(item.number + "\t");
                count++;
            }
            Console.WriteLine();
        }

        public static bool ContainsDuplicates(ArrayList arrayToCheck) 
        {
            ArrayList arrayList = new ArrayList();
            foreach (Number item in arrayToCheck)
            {
                if (item.number != 0)
                {
                    if (arrayList.Contains(item.number))
                        return true;
                    arrayList.Add(item.number);
                }
            }
            return false;
        }
    }
}
