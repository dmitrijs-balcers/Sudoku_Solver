using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace Sudoku_Solver
{
    class Program
    {
        private static ArrayList array = new ArrayList();
        private static ArrayList rows = null;
        private static ArrayList columns = null;
        private static ArrayList blocks3x3 = null;

        private const short A = 9;

        static void Main(string[] args)
        {
            Console.WriteLine("Welkome to Sudou Solver");

            bool exit = false;
            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1 - Read Matrix");
                Console.WriteLine("2 - Check Each 3x3 array");
                Console.WriteLine("3 - Print Grid");
                Console.WriteLine("4 - See info of Number x|y");
                Console.WriteLine("5 - DivideArray");
                Console.WriteLine("6 - CheckAllNumebrs");
                Console.WriteLine("7 - set Possible Numebrs and then output number.ToString()");
                Console.WriteLine("9 - Exit programm");
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                string input = Console.ReadLine();
                int opt;
                
                
                Int32.TryParse(input.Substring(0, 1), out opt);
                switch (opt)
                {
                    case 1:
                        readMatrix();
                        break;
                    case 2:
                        CheckEachBlockForDuplicates();
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
                            if (item.x == x && item.y == y)
                                Console.WriteLine(item.toString());
                        break;
                    case 5:
                        DivideArray();
                        break;
                    case 6:
                        CheckAllNumebrs();
                        break;
                    case 7:
                        setPossibleNumbersInBuffer();
                        break;
                    case 9:
                        exit = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public static void readMatrix() 
        {
            string matrix = "";
            matrix += "nnnn2345n";
            matrix += "n1n9563nn";
            matrix += "nnnnnnn1n";
            matrix += "nnn837nnn";
            matrix += "nnnnnnnnn";
            matrix += "n8nn6n7nn";
            matrix += "nn7nnnn4n";
            matrix += "23nn91n6n";
            matrix += "5nnnnnn2n";

            //matrix += "nnn815nn3";
            //matrix += "nnnnnnnnn";
            //matrix += "nnn294n5n";
            //matrix += "n81n7354n";
            //matrix += "nn7568n9n";
            //matrix += "n6n142nn8";
            //matrix += "nn835n7n2";
            //matrix += "nn3629n1n";
            //matrix += "nnnn8nnnn";

            //matrix += "n17n3458n";
            //matrix += "n54689nnn";
            //matrix += "nn37512nn";
            //matrix += "nnn178392";
            //matrix += "nnn34n7n6";
            //matrix += "nnn92nnnn";
            //matrix += "nnn413nnn";
            //matrix += "nn256n9n3";
            //matrix += "nnnnnnnnn";

            //matrix += "n17234589";
            //matrix += "254689137";
            //matrix += "893751264";
            //matrix += "546178392";
            //matrix += "928345716";
            //matrix += "371926458";
            //matrix += "769413825";
            //matrix += "182567943";
            //matrix += "435892671";
            int x = 0;
            int y = 1;
            foreach (char item in matrix)
            {
                x++;
                int number = 0;
                Number num = new Number(x, y, number, false, true);

                if (!item.Equals("n") && Int32.TryParse(item.ToString(), out number))
                {
                    num.number = number;
                    num.initial = true;
                    num.isEmpty = false;
                }
                if (x == A)
                {
                    y++;
                    x = 0;
                }
                
                array.Add(num);
            }
        }

        private static void LoadNumbers()
        {
            array = new ArrayList();
            for (int y = 1; y <= A; y++)
            {
                for (int x = 1; x <= A; x++)
                {
                    int number = 0;
                    Number num = new Number(x, y, number, false, true);

                    string input = Console.ReadLine();
                    if (input != "" && Int32.TryParse(input.Substring(0, 1), out number))
                    {
                        num.number = number;
                        num.initial = true;
                        num.isEmpty = false;
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

        private static void DivideArray()
        {
            rows = new ArrayList();
            columns = new ArrayList();
            blocks3x3 = new ArrayList();

            for (int y = 1; y <= A; y++)
            {
                Row row = new Row(y);
                for (int x = 1; x <= A; x++)
                    foreach (Number item in array)
                        if (item.x == x && item.y == y)
                            row.array.Add(item);
                rows.Add(row);
            }

            for (int x = 1; x <= A; x++)
            {
                Column column = new Column(x);
                for (int y = 1; y <= A; y++)
                    foreach (Number item in array)
                        if (item.x == x && item.y == y)
                            column.array.Add(item);
                columns.Add(column);
            }

            for (int y1 = 1; y1 <= 3; y1++)
            {
                for (int x1 = 1; x1 <= 3; x1++)
                {
                    Block block = new Block(x1, y1);
                    foreach (Number item in array)
                        if ((item.x <= x1 * 3 && item.x > (x1 - 1) * 3) && (item.y <= y1 * 3 && item.y > (y1 - 1) * 3))
                            block.al.Add(item);
                    blocks3x3.Add(block);
                }
            }
        }

        public static void setPossibleNumbersInBuffer() 
        {
            DivideArray();
            foreach (Number item in array)
            {
                if (item.isEmpty)
                {
                    item.isEmpty = false;
                    for (int current = 1; current <= 9; current++)
                    {
                        item.number = current;
                        DivideArray();
                        if (CheckAllNumebrs())
                            item.buffer.Add(current);
                    }
                    item.number = 0;
                    item.isEmpty = true;
                }
            }

            foreach (Number item in array)
            {
                if (item.buffer.Count == 1) 
                {
                    int[] bn = item.buffer.ToArray(typeof(int)) as int[];
                    item.number = bn[0];
                    item.isEmpty = false;
                }
                item.buffer = new ArrayList();
            }

            foreach (Number item in array)
            {
                if (item.isEmpty)
                    setPossibleNumbersInBuffer();
            }
        }

        // TODO: I should use array (probably simply divide it here)
        private static bool CheckAllNumebrs() 
        {
            bool isAllNumbersOk = false;
            if (!CheckEachRowForDuplicates(rows) && !CheckEachColumnForDuplicates() && !CheckEachBlockForDuplicates())
                isAllNumbersOk = true;
            return isAllNumbersOk;
        }

        public static bool CheckEachRowForDuplicates(ArrayList rows) 
        {
            bool containsDupl = true;
            foreach (Row row in rows)
            {
                ArrayList tempArray = new ArrayList();
                foreach (Number item in row.array)
                    tempArray.Add(item);
                containsDupl = ContainsDuplicates(tempArray);
                if (containsDupl)
                    return containsDupl;
            }
            return containsDupl;
        }

        private static bool CheckEachColumnForDuplicates()
        {
            bool containsDupl = true;
            foreach (Column column in columns)
            {
                ArrayList tempArray = new ArrayList();
                foreach (Number item in column.array)
                    tempArray.Add(item);
                containsDupl = ContainsDuplicates(tempArray);
                if (containsDupl)
                    return containsDupl;
            }
            return containsDupl;
        }

        private static bool CheckEachBlockForDuplicates()
        {
            bool containsDupl = true;
            foreach (Block block in blocks3x3)
            {
                ArrayList tempArray = new ArrayList();
                foreach (Number item in block.al)
                    if (isNumberInParticularBlock(block, item))
                        tempArray.Add(item);
                containsDupl = ContainsDuplicates(tempArray);
                if (containsDupl)
                    return containsDupl;
            }
            return containsDupl;
        }

        private static bool isNumberInParticularBlock(Block block, Number item)
        {
            return (item.x <= block.x * 3 && item.x > (block.x - 1) * 3) && (item.y <= block.y * 3 && item.y > (block.y - 1) * 3);
        }

        private static void CountFilledNumbersIn3x3()
        {
            int numbersFilled = 0;
            if (!ContainsDuplicates(array))
                foreach (Number item in array)
                    if (item.number != 0)
                        numbersFilled++;
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
