using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using data;
namespace unit_test
{
    [TestClass]
    public class JobTest
    {
        [TestMethod]
        public void TestJobMethods()
        {
            double[] input = new double[3] { 4, 2, 3 };

            Job j = new Job(3);
            j.Tasks = input;

            Assert.AreEqual(9, j.GetSumOfTasks());
            Assert.AreEqual(4, j.GetMaxTask());
        }
    }
    public class DataTest
    {
        [TestMethod]
        public void TestDataMethods()
        {
            double[] input = new double[3] { 4, 2, 3 };

            Job j = new Job(3);
            j.Tasks = input;

            Assert.AreEqual(9, j.GetSumOfTasks());
            Assert.AreEqual(4, j.GetMaxTask());
        }
    }
}
