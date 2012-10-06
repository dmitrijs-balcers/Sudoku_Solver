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
            Console.WriteLine("Welkome to Sudou Solver");

            bool exit = false;
            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1 - Load Numbers");
                Console.WriteLine("2 - Check Each 3x3 array");
                Console.WriteLine("3 - Print Grid");
                Console.WriteLine("4 - See info of Number x|y");
                Console.WriteLine("5 - CheckForCorrectNumber");
                Console.WriteLine("9 - Exit programm");
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                string input = Console.ReadLine();
                int opt;
                
                
                Int32.TryParse(input.Substring(0, 1), out opt);
                switch (opt)
                {
                    case 1:
                        LoadNumbers();
                        break;
                    case 2:
                        CheckEach3x3ArrayForDuplicates();
                        break;
                    case 3:
                        PrintGrid();
                        break;
                    case 4:
                        int x;
                        int y;
                        Console.WriteLine("Insert x");
                        string inp = Console.ReadLine();
                        Int32.TryParse(inp.Substring(0, 1), out x);
                        Console.WriteLine("Insert y");
                        inp = Console.ReadLine();
                        Int32.TryParse(inp.Substring(0, 1), out y);
                        foreach (Number item in array)
                        {
                            if (item.x == x && item.y == y)
                            {
                                Console.WriteLine(item.toString());
                            }
                        }
                        break;
                    case 5:
                        CheckForCorrectNumber();
                        break;
                    case 9:
                        exit = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void LoadNumbers()
        {
            array = new ArrayList();
            for (int y = 1; y <= A; y++)
            {
                for (int x = 1; x <= A; x++)
                {
                    Number num = new Number();
                    num.y = y;
                    num.x = x;

                    int number;
                    string input = Console.ReadLine();
                    if (input != "" && Int32.TryParse(input.Substring(0, 1), out number))
                    {
                        num.number = number;
                        num.initial = true;
                    }
                    else
                        num.isEmpty = true;
                    array.Add(num);

                    Console.CursorTop--;
                    Console.CursorLeft = x;
                }
                Console.WriteLine();
            }
        }

        private static void CheckForCorrectNumber()
        {
            for (int y = 1; y <= A; y++)
            {
                ArrayList rowArray = new ArrayList();
                for (int x = 1; x <= A; x++)
                {
                    foreach (Number item in array)
	                {
                        if (item.x == x && item.y == y)
                        {
                            rowArray.Add(item);
                        }
	                }
                }
                Console.WriteLine("Row " + y + ". Contains Duplicates: " + ContainsDuplicates(rowArray));
            }
        }

        private static void CheckEach3x3ArrayForDuplicates()
        {
            ArrayList tempArray = new ArrayList();
            for (int y1 = 1; y1 < 3; y1++)
            {
                for (int x1 = 1; x1 < 3; x1++)
                {
                    foreach (Number item in array)
                    {
                        if ((item.x <= x1 * 3 && item.x > (x1 - 1) * 3) && (item.y <= y1 * 3 && item.y > (y1 - 1) * 3))
                        {
                            tempArray.Add(item);
                        }
                    }
                    Console.WriteLine("Array(x|y) " + x1 + "|" + y1 + ". Contains Duplicates: " + ContainsDuplicates(tempArray));
                    tempArray = new ArrayList();
                }
            }
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
