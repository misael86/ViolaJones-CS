using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace ViolaJonesCS.Utilities
{
    public class Data
    {
        public enum DataSet { pFace, 
                              nFace }

        public static string[] DataURL = new string[2] { @"C:\Users\Misse\Documents\Exjobb\ViolaJonesCS\ViolaJonesCS\Images\PFACES\", 
                                                         @"C:\Users\Misse\Documents\Exjobb\ViolaJonesCS\ViolaJonesCS\Images\NFACES\" };

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static string[] getImageList(DataSet dataSet)
        {
            return Directory.GetFiles(DataURL[dataSet.GetHashCode()]);
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageList"></param>
        /// <param name="nrImages"></param>
        /// <returns></returns>
        public static string[] getRandomImageList(string[] imageList, int nrImages)
        {
            string[] randomImages = new string[nrImages];
            Random random = new Random();
            int maxInt = imageList.Count() - 1;
            for (int i = 0; i < nrImages; )
            {
                string nextImage = imageList[random.Next(maxInt)];
                if (!randomImages.Contains(nextImage))
                {
                    randomImages[i++] = nextImage;
                }
            }

            return randomImages;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageList"></param>
        /// <returns></returns>
        public static Matrix[] getImageMatrixList(string[] imageList)
        {
            Matrix[] imageMatrixList = new Matrix[imageList.Count()];
            for(int i = 0; i < imageList.Count(); i++)
            {
                Bitmap bitMapImage = new Bitmap(imageList[i]);
                Matrix matrixImage = new Matrix(bitMapImage.Width, bitMapImage.Height);

                for (int col = 1; col <= matrixImage.nr_cols; col++)
                {
                    for (int row = 1; row <= matrixImage.nr_rows; row++)
                    {
                        matrixImage[col, row] = bitMapImage.GetPixel(col-1, row-1).ToArgb();
                    }
                }
                imageMatrixList[i] = matrixImage;
            }
            return imageMatrixList;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageList"></param>
        /// <returns></returns>
        public static Matrix[] getNormalisedImageMatrixList(string[] imageList)
        {
            return getNormalisedImageMatrixList(getImageMatrixList(imageList));
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrixList"></param>
        /// <returns></returns>
        public static Matrix[] getNormalisedImageMatrixList(Matrix[] matrixList)
        {
            List<Matrix> result = new List<Matrix>();

            foreach (Matrix matrix in matrixList)
            {
                try
                {
                    result.Add(matrix.getNormal());
                }
                catch{ }
            }

            return result.ToArray();
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static Matrix getImageMatrix(string image, DataSet dataSet)
        {
            Bitmap bitMapImage = new Bitmap(DataURL[(int)dataSet-1] + image);
            Matrix matrixImage = new Matrix(bitMapImage.Width, bitMapImage.Height);

            for (int col = 1; col <= matrixImage.nr_cols; col++)
            {
                for (int row = 1; row <= matrixImage.nr_rows; row++)
                {
                    matrixImage[col, row] = bitMapImage.GetPixel(col - 1, row - 1).ToArgb();
                }
            }

            return matrixImage;
        }

    }
}
