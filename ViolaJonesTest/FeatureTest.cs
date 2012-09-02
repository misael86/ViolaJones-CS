using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViolaJonesCS.Utilities;

namespace TestProject3
{
    [TestClass]
    public class FeatureTest
    {
        [TestMethod]
        public void TestGetBoxSum()
        {
            Matrix m001 = new Matrix(3, 2, new double[6] { 1, 2, 3, 4, 5, 6 });
            Matrix i001 = Image.getIntegralImage(m001);
            Matrix m002 = m001.getSubMatrix(2, 1, 2, 2);
            Assert.AreEqual(Feature.getBoxSum(i001, 2, 1, 2, 2), m002.getSum());
        }

        [TestMethod]
        public void TestGetFeatures()
        {
            List<Feature> type1 = Feature.getFeatures(19, 19, FeatureType.type1);
            List<Feature> type2 = Feature.getFeatures(19, 19, FeatureType.type2);
            List<Feature> type3 = Feature.getFeatures(19, 19, FeatureType.type3);
            List<Feature> type4 = Feature.getFeatures(19, 19, FeatureType.type4);

            int nrFeatures = type1.Count + type2.Count + type3.Count + type4.Count;
            Assert.AreEqual(nrFeatures, 32746);
        }

        [TestMethod]
        public void TestGetFeatureValue1()
        {
            double eps = 0.000001;

            Feature feature1 = new Feature(FeatureType.type1, DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h);
            Feature feature2 = new Feature(FeatureType.type2, DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h);
            Feature feature3 = new Feature(FeatureType.type3, DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h);
            Feature feature4 = new Feature(FeatureType.type4, DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h);

            double f1 = Feature.getFeatureValue(DebugInfo1.ii_mm, feature1);
            double f2 = Feature.getFeatureValue(DebugInfo1.ii_mm, feature2);
            double f3 = Feature.getFeatureValue(DebugInfo1.ii_mm, feature3);
            double f4 = Feature.getFeatureValue(DebugInfo1.ii_mm, feature4);

            double f1ref = DebugInfo1.im.getSubMatrix(DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h).getSum() -
                           DebugInfo1.im.getSubMatrix(DebugInfo2.x, DebugInfo2.y + DebugInfo2.h, DebugInfo2.w, DebugInfo2.h).getSum();
            double f2ref = DebugInfo1.im.getSubMatrix(DebugInfo2.x + DebugInfo2.w, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h).getSum() -
                           DebugInfo1.im.getSubMatrix(DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h).getSum();
            double f3ref = DebugInfo1.im.getSubMatrix(DebugInfo2.x + DebugInfo2.w, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h).getSum() -
                           DebugInfo1.im.getSubMatrix(DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h).getSum() -
                           DebugInfo1.im.getSubMatrix(DebugInfo2.x + 2 * DebugInfo2.w, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h).getSum();
            double f4ref = DebugInfo1.im.getSubMatrix(DebugInfo2.x + DebugInfo2.w, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h).getSum() +
                           DebugInfo1.im.getSubMatrix(DebugInfo2.x, DebugInfo2.y + DebugInfo2.h, DebugInfo2.w, DebugInfo2.h).getSum() -
                           DebugInfo1.im.getSubMatrix(DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h).getSum() -
                           DebugInfo1.im.getSubMatrix(DebugInfo2.x + DebugInfo2.w, DebugInfo2.y + DebugInfo2.h, DebugInfo2.w, DebugInfo2.h).getSum();
            
            Assert.IsTrue(Math.Abs(f1 - f1ref) < eps);
            Assert.IsTrue(Math.Abs(f2 - f2ref) < eps);
            Assert.IsTrue(Math.Abs(f3 - f3ref) < eps);
            Assert.IsTrue(Math.Abs(f4 - f4ref) < eps);

        }

        [TestMethod]
        public void TestGetFeatureValue2()
        {
            double eps = 0.000001;

            Feature feature1 = new Feature(FeatureType.type1, DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h);
            Feature feature2 = new Feature(FeatureType.type2, DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h);
            Feature feature3 = new Feature(FeatureType.type3, DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h);
            Feature feature4 = new Feature(FeatureType.type4, DebugInfo2.x, DebugInfo2.y, DebugInfo2.w, DebugInfo2.h);

            double f1 = Feature.getFeatureValue(DebugInfo1.ii_mm, feature1);
            double f2 = Feature.getFeatureValue(DebugInfo1.ii_mm, feature2);
            double f3 = Feature.getFeatureValue(DebugInfo1.ii_mm, feature3);
            double f4 = Feature.getFeatureValue(DebugInfo1.ii_mm, feature4);

            Assert.IsTrue(Math.Abs(DebugInfo2.f1 - f1) < eps);
            Assert.IsTrue(Math.Abs(DebugInfo2.f2 - f2) < eps);
            Assert.IsTrue(Math.Abs(DebugInfo2.f3 - f3) < eps);
            Assert.IsTrue(Math.Abs(DebugInfo2.f4 - f4) < eps);
        }

        [TestMethod]
        public void TestGetFeatureValues()
        {
            double eps = 0.000001;

            string[] names = Data.getImageList(Data.DataSet.pFace).Take(100).ToArray();
            Matrix[] images = Data.getNormalisedImageMatrixList(names);
            Matrix[] integrals = Image.getIntegralImages(images);
            Matrix values = Feature.getFeatureValues(integrals, DebugInfo3.ftype);

            Assert.IsTrue((values - DebugInfo3.fs).getAbsMatrix().getSum() < eps);
        }

        [TestMethod]
        public void TestSaveFeatureImage()
        {
            Feature feature1 = new Feature(FeatureType.type1, 2, 2, 3, 4);
            Feature.saveFeatureImage(feature1, 19, 19, "test1.jpg");

            Feature feature2 = new Feature(FeatureType.type4, 5, 5, 5, 5);
            Feature.saveFeatureImage(feature2, 19, 19, "test2.jpg");
            
            Assert.IsFalse(false);
        }


    }
}
