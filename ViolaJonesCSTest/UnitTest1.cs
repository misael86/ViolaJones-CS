using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViolaJonesCS.Utilities;

namespace ViolaJonesCSTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Matrix m001 = new Matrix(2, 3);
            Matrix m002 = new Matrix(2, 3, new double[6] { 0, 0, 0, 0, 0, 0 });
            Matrix m003 = new Matrix(2, 3, new double[6] { 0, 0, 0, 0, 0, 1 });

            Assert.AreEqual(m001, m002);
            Assert.AreNotEqual(m001, m003);
        }
    }
}
