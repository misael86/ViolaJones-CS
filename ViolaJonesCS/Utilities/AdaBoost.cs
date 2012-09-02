using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViolaJonesCS.Utilities
{
    //public class WeakClassifier
    //{
    //    public double theta { get; set; }
    //    public double p { get; set; }
    //    public double err { get; set; }

    //    public WeakClassifier(double theta, double p, double err)
    //    {
    //        this.theta = theta;
    //        this.p = p;
    //        this.err = err;
    //    }
    //}

    public class WeakClassifier
    {
        public int parity { get; set; }
        public double error { get; set; }
        public double threshold { get; set; }

        public WeakClassifier(int parity, double error, double threshold)
        {
            this.parity = parity;
            this.error = error;
            this.threshold = threshold;
        }
    }

    public class Classifier
    {
        public double[] alphas { get; set; }
        public double[] thetas { get; set; }
        public Feature[] features { get; set; }
        public Feature[] all_features { get; set; }

        public Classifier(double[] alphas, double[] thetas, Feature[] features, Feature[] all_features)
        {
            this.alphas = alphas;
            this.thetas = thetas;
            this.features = features;
            this.all_features = all_features;
        }
    }

    public class AdaBoostRespons
    {
        public WeakClassifier weakClassifier { get; set; }
        public double alpha { get; set; }
        public FeatureIndex featureIndex { get; set; }

        public AdaBoostRespons(WeakClassifier weakClassifier, double alpha, FeatureIndex featureIndex)
        {
            this.weakClassifier = weakClassifier;
            this.alpha = alpha;
            this.featureIndex = featureIndex;
        }
    }

    public class FeatureIndex
    {
        public FeatureType featureType { get; set; }
        public int j { get; set; }

        public FeatureIndex(FeatureType featureType, int j)
        {
            this.featureType = featureType;
            this.j = j;
        }
    }

    public static class AdaBoost
    {
        public static int getClassification(Matrix featureValues, AdaBoostRespons[] adaBoostResponses)
        {
            double sum = 0;
            double alphaSum = 0;
            foreach (AdaBoostRespons adaBoostRespons in adaBoostResponses)
            {
                sum += adaBoostRespons.alpha * getWeakClassification(featureValues[adaBoostRespons.featureIndex.j], adaBoostRespons.weakClassifier.parity, adaBoostRespons.weakClassifier.threshold);
                alphaSum += adaBoostRespons.alpha;
            }

            return sum > (alphaSum / 2) ? 1 : 0;
        }

        public static int getWeakClassification(double featureValue, double parity, double threshold)
        {
            return parity * featureValue < parity * threshold ? 1 : 0;
        }

        public static Matrix getWeakClassifications(Matrix featureValues, double parity, double threshold)
        {
            double[] data = (from f in featureValues.data
                             select (double)getWeakClassification(f, parity, threshold)).ToArray();

            return new Matrix(data.Length, 1, data);
        }

        public static WeakClassifier learnWeakClassifier(Matrix allFeatureValues, Matrix weights, Matrix isFaceList, int j)
        {
            Matrix featureRow = allFeatureValues.getRow(j);

            return learnWeakClassifier(featureRow, weights, isFaceList);
        }

        public static WeakClassifier learnWeakClassifier(Matrix featureValues, Matrix weights, Matrix isFaceList)
        {
            if (featureValues.nr_vals != weights.nr_vals || featureValues.nr_vals != isFaceList.nr_vals) { throw new Exception("Error input"); }

            Matrix mu_p_temp = weights.dotMultiplication(isFaceList);
            double mu_p = mu_p_temp.dotMultiplication(featureValues).getSum() / mu_p_temp.getSum();

            Matrix mu_n_temp = weights - mu_p_temp;
            double mu_n = mu_n_temp.dotMultiplication(featureValues).getSum() / mu_n_temp.getSum();

            double threshold = (mu_p + mu_n) / 2.0;

            double e_n = (isFaceList - getWeakClassifications(featureValues, -1, threshold)).getAbsMatrix().dotMultiplication(weights).getSum();
            double e_p = (isFaceList - getWeakClassifications(featureValues, 1, threshold)).getAbsMatrix().dotMultiplication(weights).getSum();

            double error = Math.Min(e_n, e_p);
            int p_star = error == e_n ? -1 : 1;

            return new WeakClassifier(p_star, error, threshold);
        }

        public static AdaBoostRespons[] executeAdaBoost(Matrix[] integralImages, Feature[] allFeatures, Matrix isFaceList, int nrNegative, int nrWeakClassifiers)
        {
            FeatureType[] featureTypes =
                (from feature in allFeatures
                 select feature.type).Distinct().ToArray();

            InitiateFeatureValues(integralImages, allFeatures, featureTypes);

            AdaBoostRespons[] adaBoostRespons = new AdaBoostRespons[nrWeakClassifiers];

            Matrix weights = getInitializedWeights(isFaceList.nr_vals - nrNegative, nrNegative);

            for (int t = 0; t < nrWeakClassifiers; t++)
            {
                weights = weights / weights.getSum();

                FeatureIndex bestFeatureIndex = new FeatureIndex(FeatureType.type1, int.MinValue);
                WeakClassifier bestClassifier = new WeakClassifier(int.MaxValue, double.MaxValue, double.MaxValue);

                foreach (FeatureType featureType in featureTypes)
                {
                    Matrix allFeatureValuesOfSameType = Matrix.DeserializeFromXML("FeatureValues_" + featureType.ToString() + ".xml");

                    for (int j = 1; j <= allFeatureValuesOfSameType.nr_rows; j++)
                    {
                        WeakClassifier weakClassifier = learnWeakClassifier(allFeatureValuesOfSameType, weights, isFaceList, j);
                        if (weakClassifier.error < bestClassifier.error)
                        {
                            bestClassifier = weakClassifier;
                            bestFeatureIndex.featureType = featureType;
                            bestFeatureIndex.j = j;
                        }
                    }
                }


                double beta = bestClassifier.error / (1 - bestClassifier.error);
                double alpha = Math.Log(1 / beta);

                Matrix featureValuesOfSameType = Matrix.DeserializeFromXML("FeatureValues_" + bestFeatureIndex.featureType.ToString() + ".xml");
                for (int i = 1; i <= featureValuesOfSameType.nr_cols; i++)
                {
                    int classification = getWeakClassification(featureValuesOfSameType[i, bestFeatureIndex.j], bestClassifier.parity, bestClassifier.threshold);
                    weights[i] = Math.Abs(classification - isFaceList[i]) == 0 ? weights[i] * beta : weights[i];
                }

                adaBoostRespons[t] = new AdaBoostRespons(bestClassifier, alpha, bestFeatureIndex);
            }

            return adaBoostRespons;
        }

        private static void InitiateFeatureValues(Matrix[] integralImages, Feature[] allFeatures, FeatureType[] featureTypes)
        {
            foreach (FeatureType featureType in featureTypes)
            {
                Feature[] allFeaturesOfSameType = (from feature in allFeatures
                                                   where feature.type.Equals(featureType)
                                                   select feature).ToArray();

                Matrix allFeatureValuesOfSameType = Feature.getAllFeatureValues(integralImages, allFeaturesOfSameType);

                allFeatureValuesOfSameType.saveToXML("FeatureValues_" + featureType.ToString());
            }
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nr_p"></param>
        /// <param name="nr_n"></param>
        /// <returns></returns>
        public static Matrix getInitializedWeights(int nr_p, int nr_n)
        {
            double val_p = 1.0 / (double)(2 * nr_p);
            double val_n = 1.0 / (double)(2 * nr_n);

            double[] weights = new double[nr_n + nr_p];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = i < nr_p ? val_p : val_n;
            }

            return new Matrix(weights.Length, 1, weights);
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nr_p"></param>
        /// <param name="nr_n"></param>
        /// <returns></returns>
        public static Matrix getInitializedIsFaceList(int nr_p, int nr_n)
        {
            double[] isFaceList = new double[nr_n + nr_p];

            for (int i = 0; i < nr_p; i++)
            {
                isFaceList[i] = 1;
            }

            return new Matrix(isFaceList.Length, 1, isFaceList);
        }


    }
}
