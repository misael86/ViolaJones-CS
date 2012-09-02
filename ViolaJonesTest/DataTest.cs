using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViolaJonesCS.Utilities;

namespace TestProject3
{
    [TestClass]
    public class DataTest
    {
        [TestMethod]
        public void TestGetRandomImages()
        {
            string[] images = Data.getImageList(Data.DataSet.nFace);
            string[] randomImages1 = Data.getRandomImageList(images, 100);
            string[] randomImages2 = Data.getRandomImageList(images, 100);
            Assert.AreNotEqual(randomImages1, randomImages2);
        }

        [TestMethod]
        public void TestGetImage()
        {
            double eps = 0.000001;

            Matrix face1 = Data.getImageMatrix("face00001.bmp", Data.DataSet.nFace).getNormal();
            Matrix face1ref = DebugInfo1.im;

            Assert.IsTrue((face1 - face1ref).getAbsMatrix().getSum() < eps); 
        }


    }
}
