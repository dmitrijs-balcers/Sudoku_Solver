using Sudoku_Solver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TestSudokuSolver
{
    [TestClass()]
    public class ProgramTest
    {
        private TestContext testContextInstance;
        private List<Row> rows;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            rows = new List<Row>();
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void CheckBlockOnUniqueNumberInBuffer() 
        {
            Number num1 = new Number(1, 1, 1, true, false);
            num1.buffer.Add(2);
            num1.buffer.Add(5);
            num1.buffer.Add(6);
            num1.buffer.Add(8);
            num1.buffer.Add(9);
            Number num2 = new Number(2, 1, 2, true, false);
            num2.buffer.Add(2);
            num2.buffer.Add(6);
            num2.buffer.Add(8);
            Number num3 = new Number(2, 1, 2, true, false);
            num3.buffer.Add(2);
            num3.buffer.Add(5);
            num3.buffer.Add(8);
            num3.buffer.Add(9);
            Number num4 = new Number(2, 1, 2, true, false);
            num4.buffer.Add(1);
            num4.buffer.Add(2);
            num4.buffer.Add(6);
            num4.buffer.Add(8);
            Number num5 = new Number(2, 1, 2, true, false);
            num5.buffer.Add(2);
            num5.buffer.Add(5);
            num5.buffer.Add(8);
            num5.buffer.Add(9);
            Number num6 = new Number(2, 1, 2, true, false);
            num6.buffer.Add(2);
            num6.buffer.Add(5);
            num6.buffer.Add(6);
            num6.buffer.Add(8);
            num6.buffer.Add(9);
            Block block = new Block(1, 1);
            block.al.Add(num1);
            block.al.Add(num2);
            block.al.Add(num3);
            block.al.Add(num4);
            block.al.Add(num5);
            block.al.Add(num6);

            int expected = 1;
            Number actual = Program.FindUniqueNumberInBlockNumbersBuffer(block);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.number);

        }

        [TestMethod()]
        public void CheckEachRowForDuplicatesTest_Successfull()
        {
            Number num1 = new Number(1, 1, 1, true, false);
            Number num2 = new Number(2, 1, 2, true, false);
            Row row1 = new Row(1);
            row1.array.Add(num1);
            row1.array.Add(num2);
            rows.Add(row1);

            bool expected = false;
            bool actual = Program.CheckEachRowForDuplicates(rows);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CheckEachRowForDuplicatesTest_DuplicatesFound_OnOneRow()
        {
            Number num1 = new Number(1, 1, 1, true, false);
            Number num2 = new Number(2, 1, 1, true, false);
            Row row1 = new Row(1);
            row1.array.Add(num1);
            row1.array.Add(num2);
            rows.Add(row1);

            bool expected = true;
            bool actual = Program.CheckEachRowForDuplicates(rows);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CheckEachRowForDuplicatesTest_DuplicatesFound_OnSeveralRows()
        {
            Number num1 = new Number(1, 1, 1, true, false);
            Number num2 = new Number(2, 1, 1, true, false);
            Row row1 = new Row(1);
            row1.array.Add(num1);
            row1.array.Add(num2);
            rows.Add(row1);

            Row row2 = new Row(2);
            num1 = new Number(1, 2, 1, true, false);
            num2 = new Number(2, 2, 1, true, false);
            row2.array.Add(num1);
            row2.array.Add(num2);
            rows.Add(row2);

            bool expected = true;
            bool actual = Program.CheckEachRowForDuplicates(rows);
            Assert.AreEqual(expected, actual);
        }
    }
}
