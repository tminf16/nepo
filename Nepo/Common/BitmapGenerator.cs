using System;
using System.Drawing;

namespace Nepo.Common
{
    public class BitmapGenerator
    {
        /// <summary>
        /// Adds random noise to bitmap.
        /// </summary>
        /// <param name="bitmap">Bitmap to add noise to.</param>
        /// <returns>Bitmap with noise.</returns>
        public Bitmap AddRandomNoise(Bitmap bitmap, Random rng)
        {
            for (var i = 0; i < bitmap.Size.Height; ++i)
            {
                for (var j = 0; j < bitmap.Size.Width; ++j)
                {
                    var pixel = bitmap.GetPixel(i, j);                    
                    bitmap.SetPixel(i,j, Color.FromArgb(rng.Next(255), pixel));
                }    
            }

            return bitmap;
        }
    }
}