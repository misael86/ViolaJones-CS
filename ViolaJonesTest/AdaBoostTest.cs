using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViolaJonesCS.Utilities;

namespace TestProject3
{
    [TestClass]
    public class AdaBoostTest
    {
        [TestMethod]
        public void TestGetInitializedWeights()
        {
            int nr_n = 5000;
            int nr_p = 3000;

            Matrix weights = AdaBoost.getInitializedWeights(nr_p, nr_n);

            Assert.IsTrue(Math.Abs(weights.getSum() - 1) < 0.0001);
        }

        [TestMethod]
        public void TestGetInitializedIsFaceList()
        {
            int nr_n = 5000;
            int nr_p = 3000;

            Matrix isFaceList = AdaBoost.getInitializedIsFaceList(nr_p, nr_n);

            Assert.AreEqual(isFaceList.getSum(), nr_p);
        }

        [TestMethod]
        public void TestLearnWeakClassifier()
        {
            int nr_n = 5000;
            int nr_p = 3000;

            Feature ftype = Feature.getAllFeatures(19, 19)[12028];

            Matrix weights = AdaBoost.getInitializedWeights(nr_p, nr_n);
            Matrix isFaceList = AdaBoost.getInitializedIsFaceList(nr_p, nr_n);

            string[] face_n = Data.getImageList(Data.DataSet.nFace).Take(nr_n).ToArray();
            string[] face_p = Data.getImageList(Data.DataSet.pFace).Take(nr_p).ToArray();

            Matrix[] face_n_integrals = Image.getIntegralImages(Data.getNormalisedImageMatrixList(face_n));
            Matrix[] face_p_integrals = Image.getIntegralImages(Data.getNormalisedImageMatrixList(face_p));

            Matrix featureValues_n = Feature.getFeatureValues(face_n_integrals, ftype);
            Matrix featureValues_p = Feature.getFeatureValues(face_p_integrals, ftype);
            Matrix featureValues_all = new Matrix(featureValues_p.nr_vals + featureValues_n.nr_vals, 1, featureValues_p.data.Concat(featureValues_n.data).ToArray());

            string fn = featureValues_n.ToString();
            string fp = featureValues_p.ToString();

            WeakClassifier weakClassifier = AdaBoost.learnWeakClassifier(featureValues_all, weights, isFaceList);

            double mean_p = featureValues_p.getSum() / (double)nr_p;
            double mean_n = featureValues_n.getSum() / (double)nr_n;
            double mean_t = (mean_p + mean_n) / 2.0;

            Assert.IsTrue(Math.Abs(weakClassifier.threshold - mean_t) < 0.001);
            //Assert.IsTrue(Math.Abs(weakClassifier.threshold - (-3.6453)) < 0.001);
            Assert.IsTrue(weakClassifier.parity == 1);
        }

        [TestMethod]
        public void TestAdaBoost()
        {
            int T = 10;
            Matrix Thetas = new Matrix(3, 3, new double[9] {9.980000000000000, -0.079477305003370,  0.010000000000000,
                                                            4.770000000000000,  0.031892257638319, -0.010000000000000,
                                                            8.490000000000000, -0.055953021563651,  0.010000000000000});
            double[] alphas = new double[] { 1.694004004780836, 1.188907795146310, 0.982307485873930 };

            int featureSize = 19;

            Feature[] allFeatures = Feature.getAllFeatures(featureSize, featureSize).ToArray();//.GetRange(0, 1000).ToArray();

            string[] negFaceList = Data.getImageList(Data.DataSet.nFace);
            string[] posFaceList = Data.getImageList(Data.DataSet.pFace);

            Matrix[] negImages = Data.getNormalisedImageMatrixList(negFaceList);
            Matrix[] posImages = Data.getNormalisedImageMatrixList(posFaceList);

            Matrix[] negIntegral_images = Image.getIntegralImages(negImages);
            Matrix[] posIntegral_images = Image.getIntegralImages(posImages);

            Matrix[] integral_images = posIntegral_images.Union(negIntegral_images).ToArray();

            Matrix isFaceList = AdaBoost.getInitializedIsFaceList(posIntegral_images.Length, negIntegral_images.Length);
            //Matrix allFeatureValues = Feature.getAllFeatureValues(integral_images, allFeatures);

            AdaBoostRespons[] adaBoostResponses = AdaBoost.executeAdaBoost(integral_images, allFeatures, isFaceList, negIntegral_images.Length, T);

            for (int i = 0; i < adaBoostResponses.Length; i++)
            {
                Feature.saveFeatureImage(Feature.getFeature(adaBoostResponses[i].featureIndex.j, adaBoostResponses[i].featureIndex.featureType,  featureSize, featureSize), featureSize, featureSize, "feature" + i + ".jpg");
            }

            double sumAlpha = 0;
            for (int i = 0; i < adaBoostResponses.Length; i++)
            {
                sumAlpha += Math.Abs(adaBoostResponses[i].alpha - alphas[i]);
            }

            Assert.IsFalse(false);

            ///double eps = 0.000001;
            //Assert.IsTrue(sumAlpha < eps);
        }
    }
}
