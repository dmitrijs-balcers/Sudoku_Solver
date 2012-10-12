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
        #region Arrays + initial data
        private static List<Number> array = new List<Number>();
        private static List<Row> rows = null;
        private static List<Column> columns = null;
        private static List<Block> blocks3x3 = null;

        private const short A = 9;
        private static int attempt = 0;
        private static int emptyNumbersBuffer = 0;
        private static int imStuckExceptionsCountGlobal = 0;

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Welkome to Sudou Solver");
            while (true)
            {
                PrintMenu();

                string input = Console.ReadLine();

                int opt = 0;
                if (!input.Equals("")) Int32.TryParse(input.Substring(0, 1), out opt);
                switch (opt)
                {
                    case 1:
                        readMatrix();
                        break;
                    case 2:
                        PrintGrid();
                        break;
                    case 3:
                        try { solveSudoku(); }
                        catch (SolvingFailedException e) { throwRedException(e.ToString()); }
                        catch (ImStuckException e) { throwRedException(e.ToString()); }
                        catch (EmptyBufferForEmptyNumberException e) { throwRedException(e.ToString()); }
                        break;
                    case 4:
                        removeBlackCanditatesFromWHiteBuffer();
                        forcingChain();
                        //foreach (Number number in array)
                          //  number.blackListBuffer = new List<int>();
                        break;
                    case 9:
                        return;
                    default:
                        break;
                }
            }
        }

        public static void removeBlackCanditatesFromWHiteBuffer() {
            foreach (Number n in array)
	        {
                foreach (int i in n.blackListBuffer)
                if (n.buffer.Contains(i)) n.buffer.Remove(i);
	        }
        }

        public static void throwRedException(string value) 
        {
            Console.Write("\t\t\t");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(value);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }

        private static void PrintMenu()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1 - Read Matrix");
            Console.WriteLine("2 - Print Grid");
            Console.WriteLine("3 - Solve Sudoku");
            Console.WriteLine("9 - Exit programm");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }

        public static void readMatrix()
        {
            string matrix = "";

            using (StreamReader sr = new StreamReader("matrix.txt"))
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

        public static bool solveSudoku() 
        {
            loadCandidatesInBuffer();
            if (!CheckAllNumebrsOnDuplicates()) throw new SolvingFailedException();

            foreach (Number item in array) setCandidateIfPossible(item);

            var emptyNumbers = from Number n in array where n.isEmpty select n;

            if (emptyNumbers.Count() > 0)
            {
                if (emptyNumbers.Count() == emptyNumbersBuffer)
                {
                    emptyNumbersBuffer = 0;
                    imStuckExceptionsCountGlobal++;
                    throw new ImStuckException();
                }

                emptyNumbersBuffer = emptyNumbers.Count();
                solveSudoku();
            }
            return true;
        }

        public static bool forcingChain() {
            foreach (Number n in array) {
                if (!n.isEmpty) continue;
                if (n.buffer.Count == 0) continue;
                foreach (int i in n.buffer) {
                    if (n.blackListBuffer.Contains(i)) continue;
                    n.number = i;
                    n.isEmpty = false;
                    try { solveSudoku(); }
                    catch (ImStuckException) {
                        if (imStuckExceptionsCountGlobal < 2)
                        {
                            n.number = 0;
                            n.isEmpty = true;
                            if (!n.blackListBuffer.Contains(i)) n.blackListBuffer.Add(i);
                        }
                        else
                        {
                            n.number = 0;
                            n.isEmpty = true;
                            if (!n.blackListBuffer.Contains(i)) n.blackListBuffer.Add(i);
                            continue;
                        }
                    }
                    catch (SolvingFailedException) {
                        n.number = 0;
                        n.isEmpty = true;
                        if (!n.blackListBuffer.Contains(i)) n.blackListBuffer.Add(i); 
                    }
                    catch (EmptyBufferForEmptyNumberException) {
                        n.number = 0;
                        n.isEmpty = true;
                        if (!n.blackListBuffer.Contains(i)) n.blackListBuffer.Add(i);
                        continue;
                    }
                    forcingChain();
                }
            }
            return false;
        }

        public static void loadCandidatesInBuffer()
        {
            DivideArray();

            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("<<<<<<<<<<<<<===>>>>>>>>>>>>>>>>>");
            Console.BackgroundColor = ConsoleColor.Black;

            foreach (Number item in array)
            {
                if (!item.isEmpty) continue;
                refillBuffer(item);
                Console.WriteLine(item.toString());
            }

            foreach (Block item in blocks3x3)
            {
                Number tmp = FindUniqueNumberInBlockNumbersBuffer(item);
                if (tmp == null) continue;
                foreach (Number n in array)
                {
                    if (!(n.x == tmp.x && n.y == tmp.y)) continue;
                    n.number = tmp.number;
                    n.isEmpty = false;
                }
            }

            attempt++;
        }

        private static void DivideArray()
        {
            rows = new List<Row>();
            columns = new List<Column>();
            blocks3x3 = new List<Block>();

            for (int y = 1; y <= A; y++)
            {
                Row row = new Row(y);
                for (int x = 1; x <= A; x++)
                    foreach (Number item in array)
                        if (item.x == x && item.y == y) row.array.Add(item);
                rows.Add(row);
            }

            for (int x = 1; x <= A; x++)
            {
                Column column = new Column(x);
                for (int y = 1; y <= A; y++)
                    foreach (Number item in array)
                        if (item.x == x && item.y == y) column.array.Add(item);
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

        private static void refillBuffer(Number item)
        {
            item.buffer = new List<int>();
            item.isEmpty = false;
            for (int current = 1; current <= 9; current++)
            {
                if(item.blackListBuffer.Count > 0)
                    if (item.blackListBuffer.Contains(current))
                        continue;
                item.number = current;
                DivideArray();
                if (CheckAllNumebrsOnDuplicates()) item.buffer.Add(current);
            }
            item.number = 0;
            item.isEmpty = true;
        }

        // TODO: I should use array (probably simply divide it here)
        private static bool CheckAllNumebrsOnDuplicates()
        {
            return (!CheckEachRowForDuplicates(rows) && !CheckEachColumnForDuplicates() && !CheckEachBlockForDuplicates()) ? true : false;
        }

        public static bool CheckEachRowForDuplicates(List<Row> rows)
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

        public static bool ContainsDuplicates(IEnumerable arrayToCheck)
        {
            List<int> arrayList = new List<int>();
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
            List<PossibleNumber> possibleNumbers = new List<PossibleNumber>();
            List<PossibleNumber> blackList = new List<PossibleNumber>();
            foreach (Number item in block.al)
            {
                if (item.isEmpty && item.buffer.Count == 0) throw new EmptyBufferForEmptyNumberException();
                else if (!item.isEmpty) continue;

                foreach (int n in item.buffer)
                {
                    if (!searchInPossibleNumbersAndRemove(possibleNumbers, blackList, n) && !searchInBlackList(blackList, n))
                    {
                        Number tempN = (Number)item.Clone();
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

        private static bool searchInPossibleNumbersAndRemove(List<PossibleNumber> possibleNumbers, List<PossibleNumber> blackList, int integer)
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

        private static bool searchInBlackList(List<PossibleNumber> blackList, int integer)
        {
            foreach (PossibleNumber item in blackList)
            {
                if (item.integer == integer)
                    return true;
            }
            return false;
        }

        private static void setCandidateIfPossible(Number item)
        {
            if (item.buffer.Count == 1)
            {
                int[] bn = item.buffer.ToArray();
                item.number = bn[0];
                item.isEmpty = false;
            }
        }

        private static bool isNumberInParticularBlock(int x, int y, Number item)
        {
            return (item.x <= x * 3 && item.x > (x - 1) * 3) && (item.y <= y * 3 && item.y > (y - 1) * 3);
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
