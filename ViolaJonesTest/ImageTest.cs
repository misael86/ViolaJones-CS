using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViolaJonesCS.Utilities;

namespace TestProject3
{
    [TestClass]
    public class ImageTest
    {
        [TestMethod]
        public void TestgetIntegralImage1()
        {
            Matrix m = new Matrix(4, 4, new double[16] { 5, 2, 5, 2, 3, 6, 3, 6, 5, 2, 5, 2, 3, 6, 3, 6 });
            Matrix i1 = new Matrix(4, 4, new double[16] { 5, 7, 12, 14, 8, 16, 24, 32, 13, 23, 36, 46, 16, 32, 48, 64 });
            Matrix i2 = Image.getIntegralImage(m);
            Assert.AreEqual(i1, i2);
        }

        [TestMethod]
        public void TestgetIntegralImage2()
        {
            double eps = 0.000001;

            Matrix face1 = Data.getImageMatrix("face00001.bmp", Data.DataSet.nFace).getNormal();
            Matrix integral1 = Image.getIntegralImage(face1);
            Matrix integral1ref = DebugInfo1.ii_mm;

            Assert.IsTrue((integral1 - integral1ref).getAbsMatrix().getSum() < eps); 
        }
    }
}
