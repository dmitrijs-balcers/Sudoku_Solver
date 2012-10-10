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
        private static int attempt = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Welkome to Sudou Solver");
            while (true)
            {
                PrintMenu();

                string input = Console.ReadLine();
                int opt;
                
                Int32.TryParse(input.Substring(0, 1), out opt);
                switch (opt)
                {
                    case 1:
                        readMatrix();
                        break;
                    case 2:
                        PrintGrid();
                        break;
                    case 3:
                        setPossibleNumbersInBuffer();
                        break;
                    case 9:
                        return;
                    default:
                        break;
                }
            }
        }

        private static void PrintMenu()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1 - Read Matrix");
            Console.WriteLine("2 - Print Grid");
            Console.WriteLine("3 - set Possible Numebrs + Print()");
            Console.WriteLine("9 - Exit programm");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }

        public static void readMatrix() 
        {
            string matrix = "";

            using (StreamReader sr =new StreamReader("matrix.txt"))
            {
                String line = sr.ReadToEnd();
                line = line.Replace("\r\n", string.Empty);
                matrix = line;
            }

            int x = 0;
            int y = 1;
            foreach (char item in matrix)
            {
                x++;
                int number = 0;
                Number num = new Number(x, y, number, false, true);

                if (!item.Equals("n") && Int32.TryParse(item.ToString(), out number))
                    SetInitialNumber(number, num);

                if (x == A)
                {
                    y++; 
                    x = 0;
                }
                
                array.Add(num);
            }
        }

        private static void SetInitialNumber(int number, Number num)
        {
            num.number = number;
            num.initial = true;
            num.isEmpty = false;
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
                        if (isNumberInParticularBlock(x1, y1, item)) block.al.Add(item);
                    blocks3x3.Add(block);
                }
            }
        }

        public static void setPossibleNumbersInBuffer() 
        {
            DivideArray();

            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("<<<<<<<<<<<<<===>>>>>>>>>>>>>>>>>");
            Console.BackgroundColor = ConsoleColor.Black;
            foreach (Number item in array)
            {
                if (item.isEmpty) { 
                    FillBuffer(item);
                    Console.WriteLine(item.toString());
                }
                
            }

            if (attempt > 15)
            {
                foreach (Block item in blocks3x3)
                {
                    Number tmp = FindUniqueNumberInBlockNumbersBuffer(item);
                    if (tmp != null)
                    {
                        foreach (Number n in array)
                        {
                            if (n.x == tmp.x && n.y == tmp.y)
                            {
                                n.number = tmp.number;
                                n.isEmpty = false;
                            }
                        }
                    }
                }
            }
            attempt++;


            foreach (Number item in array)
            {
                ClearBuffferAndSetNumberIfPossible(item);
            }

            var temp = from Number n in array where n.isEmpty select n;
            if (temp.Count() > 0) setPossibleNumbersInBuffer();

        }

        private static void FillBuffer(Number item)
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

        private static void ClearBuffferAndSetNumberIfPossible(Number item)
        {
            if (item.buffer.Count == 1)
            {
                int[] bn = item.buffer.ToArray(typeof(int)) as int[];
                item.number = bn[0];
                item.isEmpty = false;
            }
            item.buffer = new ArrayList();
        }

        // TODO: I should use array (probably simply divide it here)
        private static bool CheckAllNumebrs() 
        {
            return (!CheckEachRowForDuplicates(rows) && !CheckEachColumnForDuplicates() && !CheckEachBlockForDuplicates()) ? true : false;
        }

        public static bool CheckEachRowForDuplicates(ArrayList rows) 
        {
            foreach (Row row in rows)
            {
                var tempArray = from Number item in row.array select item;
                if (ContainsDuplicates(tempArray)) return true;
            }
            return false;
        }

        private static bool CheckEachColumnForDuplicates()
        {
            foreach (Column column in columns)
            {
                var tempArray = from Number item in column.array select item;
                if (ContainsDuplicates(tempArray)) return true;
            }
            return false;
        }

        private static bool CheckEachBlockForDuplicates()
        {
            foreach (Block block in blocks3x3)
            {
                var tempArray = from Number item in block.al where isNumberInParticularBlock(block.x, block.y, item) select item;
                if (ContainsDuplicates(tempArray)) return true;
            }
            return false;
        }

        private static bool isNumberInParticularBlock(int x, int y, Number item)
        {
            return (item.x <= x * 3 && item.x > (x - 1) * 3) && (item.y <= y * 3 && item.y > (y - 1) * 3);
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

        public static bool ContainsDuplicates(IEnumerable arrayToCheck) 
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

        internal static Number FindUniqueNumberInBlockNumbersBuffer(Block block)
        {
            ArrayList possibleNumbers = new ArrayList();
            ArrayList blackList = new ArrayList();
            foreach (Number item in block.al)
            {
                foreach (int n in item.buffer)
                {
                    if (!searchInPossibleNumbersAndRemove(possibleNumbers, blackList, n) && !searchInBlackList(blackList, n))
                    {
                        Number tempN = (Number) item.Clone();
                        tempN.number = n;
                        possibleNumbers.Add(new PossibleNumber(tempN, n));
                    }
                }
            }
            if (possibleNumbers.Count == 1)
                return ((PossibleNumber)possibleNumbers[0]).number;
            else
                return null;
        }

        private static bool searchInPossibleNumbersAndRemove(ArrayList possibleNumbers, ArrayList blackList, int integer)
        {
            foreach (PossibleNumber item in possibleNumbers)
            {
                if (item.integer == integer)
                {
                    possibleNumbers.Remove(item);
                    blackList.Add(item);
                    return true;
                }
            }
            return false;
        }

        private static bool searchInBlackList(ArrayList blackList, int integer) 
        {
            foreach (PossibleNumber item in blackList)
            {
                if (item.integer == integer)
                    return true;
            }
            return false;
        }

    }
    //matrix += "nnnn2345n";
    //matrix += "n1n9563nn";
    //matrix += "nnnnnn1nn";
    //matrix += "nnn837nnn";
    //matrix += "nnnnnnnnn";
    //matrix += "n8nn6n7nn";
    //matrix += "nn7nnnn4n";
    //matrix += "23nn91n6n";
    //matrix += "5nnnnnn2n";

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
}
