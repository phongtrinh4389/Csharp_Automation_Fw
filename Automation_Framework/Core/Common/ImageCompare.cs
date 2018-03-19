using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Framework.Core.Common
{
    public class ImageCompare
    {

        /// <summary>
        /// To compare 2 images and return the different image
        /// </summary>
        /// <param name="actualImage">The string path to the actual image</param>
        /// <param name="expectedImage">The string path to the expected image</param>
        /// <param name="threshold">The threshold of comparision</param>
        /// <returns>The Bitmap of the different image</returns>
        public static Bitmap DiffImage(string actualImage, string expectedImage, int threshold)
        {
            int isDiff = 0;

            Bitmap bitmap1 = new Bitmap(Image.FromFile(actualImage));
            Bitmap bitmap2 = new Bitmap(Image.FromFile(expectedImage));
            Bitmap diff = null;

            if (bitmap1.Size != bitmap2.Size)
            {
                return null;
            }

            if (bitmap1.PixelFormat != bitmap2.PixelFormat)
            {
                return null;
            }

            int minWidth = Math.Min(bitmap1.Width, bitmap2.Width);
            int minHeight = Math.Min(bitmap1.Height, bitmap2.Height);
            int maxWidth = Math.Max(bitmap1.Width, bitmap2.Width);
            int maxHeight = Math.Max(bitmap1.Height, bitmap2.Height);

            diff = new Bitmap(maxWidth, maxHeight);

            for (int x = 0; x < minWidth; ++x)
            {
                for (int y = 0; y < minHeight; ++y)
                {
                    int rgb1 = bitmap1.GetPixel(x, y).ToArgb();
                    int rgb2 = bitmap2.GetPixel(x, y).ToArgb();
                    if (rgb1 != rgb2
                        && (Math.Abs((rgb1 & 0xFF) - (rgb2 & 0xFF)) > threshold
                        || Math.Abs(((rgb1 >> 8) & 0xFF) - ((rgb2 >> 8) & 0xFF)) > threshold
                        || Math.Abs(((rgb1 >> 16) & 0xFF) - ((rgb2 >> 16) & 0xFF)) > threshold))
                    {

                        diff.SetPixel(x, y, Color.Yellow);
                        isDiff++;
                    }
                    else
                    {
                        if (diff != null)
                        {
                            diff.SetPixel(x, y, bitmap1.GetPixel(x, y));
                        }
                    }
                }
            }

            if (isDiff > 0)
            {
                return diff;
            }
            else
            {
                return null;
            }
        }

    }
}
