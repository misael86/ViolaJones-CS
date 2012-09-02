using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViolaJonesCS.Utilities
{
    public class Image
    {
        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Matrix getIntegralImage(Matrix image)
        {
            Matrix integral_image = new Matrix(image.nr_cols, image.nr_rows);

            for (int y, x = 1; x <= image.nr_cols; x++)
            {
                for (y = 1; y <= image.nr_rows; y++)
                {
                    integral_image[x, y] = image[x, y];
                    integral_image[x, y] += x > 1 ? integral_image[x - 1, y] : 0;
                    integral_image[x, y] += y > 1 ? integral_image[x, y - 1] : 0;
                    integral_image[x, y] -= x > 1 && y > 1 ? integral_image[x - 1, y - 1] : 0;
                }
            }

            return integral_image;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public static Matrix[] getIntegralImages(Matrix[] images)
        {
            return (from image in images
                    select getIntegralImage(image)).ToArray();
        }

    }
}
