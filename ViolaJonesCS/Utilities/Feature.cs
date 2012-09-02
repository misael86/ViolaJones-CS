using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace ViolaJonesCS.Utilities
{
    public enum FeatureType
    {
        type1,
        type2,
        type3,
        type4
    }

    public class Feature
    {
      
        public FeatureType type;
        public int x, y, w, h;

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public Feature(FeatureType type, int x, int y, int w, int h)
        {
            this.type = type;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="integral_image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static double getBoxSum(Matrix integral_image, int x, int y, int w, int h)
        {
            if (h < 1 || w < 1 || integral_image.nr_cols < x + w - 1 || integral_image.nr_rows < y + h - 1)
            {
                throw new Exception("Invalid input - width, height, x, y, w, h " +
                    integral_image.nr_cols + ", " + integral_image.nr_rows + ", " +
                    x + ", " + y + ", " + w + ", " + h);
            }

            double sum = integral_image[x + w - 1, y + h - 1];
            sum -= y > 1 ? integral_image[x + w - 1, y - 1] : 0;
            sum -= x > 1 ? integral_image[x - 1, y + h - 1] : 0;
            sum += x > 1 && y > 1 ? integral_image[x - 1, y - 1] : 0;

            return sum;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="W"></param>
        /// <param name="featureHeight"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Feature> getFeatures(int featureWidth, int featureHeight, FeatureType type)
        {
            int x, y, w, h;

            List<Feature> all_features = new List<Feature>();

            switch (type)
            {
                case FeatureType.type1:
                    for (w = 1; w <= featureWidth - 2; w++)
                        for (h = 1; h <= Math.Floor(featureHeight / 2.0) - 2; h++)
                            for (x = 1; x < featureWidth - w; x++)
                                for (y = 1; y < featureHeight - 2 * h; y++)
                                    all_features.Add(new Feature(FeatureType.type1, x, y, w, h ));
                    break;

                case FeatureType.type2:
                    for (w = 1; w <= Math.Floor(featureWidth / 2.0) - 2; w++)
                        for (h = 1; h <= featureHeight - 2; h++)
                            for (x = 1; x <= featureWidth - 2 * w - 1; x++)
                                for (y = 1; y <= featureHeight - h - 1; y++)
                                    all_features.Add(new Feature(FeatureType.type2, x, y, w, h));
                    break;

                case FeatureType.type3:
                    for (w = 1; w <= Math.Floor(featureWidth / 3.0) - 2; w++)
                        for (h = 1; h <= featureHeight - 2; h++)
                            for (x = 1; x <= featureWidth - 3 * w - 1; x++)
                                for (y = 1; y <= featureHeight - h - 1; y++)
                                    all_features.Add(new Feature(FeatureType.type3, x, y, w, h));
                    break;

                case FeatureType.type4:
                    for (w = 1; w <= Math.Floor(featureWidth / 2.0) - 2; w++)
                        for (h = 1; h <= Math.Floor(featureHeight / 2.0) - 2; h++)
                            for (x = 1; x <= featureWidth - 2 * w - 1; x++)
                                for (y = 1; y <= featureHeight - 2 * h - 1; y++)
                                    all_features.Add(new Feature(FeatureType.type4, x, y, w, h));

                    break;
            }

            return all_features;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureWidth"></param>
        /// <param name="featureHeight"></param>
        /// <returns></returns>
        public static List<Feature> getAllFeatures(int featureWidth, int featureHeight)
        {
            List<Feature> allFeatures = new List<Feature>();

            allFeatures.AddRange(getFeatures(featureWidth, featureHeight, FeatureType.type1));
            allFeatures.AddRange(getFeatures(featureWidth, featureHeight, FeatureType.type2));
            allFeatures.AddRange(getFeatures(featureWidth, featureHeight, FeatureType.type3));
            allFeatures.AddRange(getFeatures(featureWidth, featureHeight, FeatureType.type4));

            return allFeatures;
        }

        public static Feature getFeature(int index, int featureWidth, int featureHeight)
        {
            List<Feature> allFeatures = getAllFeatures(featureWidth, featureHeight);
            if (index > allFeatures.Count || index < 1) { throw new Exception("Feature index is out of bounds"); }
            return allFeatures[index-1];
        }

        public static Feature getFeature(int index, FeatureType featureType, int featureWidth, int featureHeight)
        {
            List<Feature> allFeatures = getFeatures(featureWidth, featureHeight, featureType);
            if (index > allFeatures.Count || index < 1) { throw new Exception("Feature index is out of bounds"); }
            return allFeatures[index - 1];
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="integral_image"></param>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static double getFeatureValue(Matrix integral_image, Feature feature)
        {

            double val = 0;

            int x = feature.x;
            int y = feature.y;
            int w = feature.w;
            int h = feature.h;

            switch (feature.type)
            {
                case FeatureType.type1:
                    val = getBoxSum(integral_image, x, y, w, h);
                    val -= getBoxSum(integral_image, x, y + h, w, h);
                    break;

                case FeatureType.type2:
                    val = getBoxSum(integral_image, x + w, y, w, h);
                    val -= getBoxSum(integral_image, x, y, w, h);
                    break;

                case FeatureType.type3:
                    val = getBoxSum(integral_image, x + w, y, w, h);
                    val -= getBoxSum(integral_image, x, y, w, h);
                    val -= getBoxSum(integral_image, x + 2 * w, y, w, h);
                    break;

                case FeatureType.type4:
                    val = getBoxSum(integral_image, x + w, y, w, h);
                    val += getBoxSum(integral_image, x, y + h, w, h);
                    val -= getBoxSum(integral_image, x, y, w, h);
                    val -= getBoxSum(integral_image, x + w, y + h, w, h);
                    break;
            }

            return val;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="integral_images"></param>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static Matrix getFeatureValues(Matrix[] integral_images, Feature feature)
        {
            double[] values = (from integral_image in integral_images
                               select getFeatureValue(integral_image, feature)).ToArray();

            return new Matrix(values.Length, 1, values);
        }

        public static Matrix getAllFeatureValues(Matrix[] integral_images, Feature[] allFeatures)
        {
            Matrix result = new Matrix(integral_images.Length, allFeatures.Length);

            for (int i = 1; i <= allFeatures.Length; i++)
            { 
                result.setRow(i, getFeatureValues(integral_images, allFeatures[i-1]));
            }

            return result;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="featureWidth"></param>
        /// <param name="featureHeight"></param>
        /// <param name="filename"></param>
        public static void saveFeatureImage(Feature feature, int featureWidth, int featureHeight, string filename)
        {
            int k = 5;
            int x = feature.x * k;
            int y = feature.y * k;
            int w = feature.w * k;
            int h = feature.h * k;
            int width = featureWidth * k;
            int height = featureHeight * k;

            Bitmap bitMapImage = new Bitmap(width, height);
            switch (feature.type)
            { 
                case FeatureType.type1:
                    fillRectangle(ref bitMapImage, x, y, w, h, Color.Red);
                    fillRectangle(ref bitMapImage, x, y + h, w, h, Color.Green);
                    break;

                case FeatureType.type2:
                    fillRectangle(ref bitMapImage, x, y, w, h, Color.Green);
                    fillRectangle(ref bitMapImage, x + w, y, w, h, Color.Red);
                    break;

                case FeatureType.type3:
                    fillRectangle(ref bitMapImage, x, y, w, h, Color.Green);
                    fillRectangle(ref bitMapImage, x + w, y, w, h, Color.Red);
                    fillRectangle(ref bitMapImage, x + w + w, y, w, h, Color.Green);
                    break;

                case FeatureType.type4:
                    fillRectangle(ref bitMapImage, x, y, w, h, Color.Green);
                    fillRectangle(ref bitMapImage, x + w, y, w, h, Color.Red);
                    fillRectangle(ref bitMapImage, x, y + h, w, h, Color.Red);
                    fillRectangle(ref bitMapImage, x + w, y + h, w, h, Color.Green);
                    break;
            }

            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            bitMapImage.Save(filename, GetEncoder(ImageFormat.Jpeg), encoderParameters);
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid) { return codec; }
            }
            return null;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="color"></param>
        private static void fillRectangle(ref Bitmap bitmap, int x, int y, int w, int h, Color color)
        {
            for (int xi = x; xi < x + w; xi++)
            {
                for (int yi = y; yi < y + h; yi++)
                {
                    bitmap.SetPixel(xi, yi, color);
                }
            }
        }
    }
}
