using Sudoku_Solver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace TestSudokuSolver
{
    [TestClass()]
    public class ProgramTest
    {
        private TestContext testContextInstance;
        private ArrayList rows;
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
            rows = new ArrayList();
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
