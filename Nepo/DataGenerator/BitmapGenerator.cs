using System;
using System.Collections.Generic;
using System.Drawing;

namespace Nepo.DataGenerator
{
    public class BitmapGenerator
    {
        /// <summary>
        /// Adds random noise to bitmap.
        /// </summary>
        /// <param name="bitmap">Bitmap to add noise to.</param>
        /// <param name="rng">Random number generator.</param>
        /// <returns>Bitmap with noise.</returns>
        public static Bitmap AddRandomNoise(Bitmap bitmap, Random rng)
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

        public static Bitmap AddBlobs(Bitmap bitmap, Random rng, MapGenerationConstraints constraints)
        {
            var blobCount = rng.Next(constraints.LayerConstraints.MinDeadzoneCount, constraints.LayerConstraints.MaxDeadzoneCount);
            var blobDeviation = (int) (constraints.LayerConstraints.DeadzoneBaseSize * 1.4);
            var blobMin = constraints.LayerConstraints.DeadzoneBaseSize - blobDeviation;
            var blobMax = constraints.LayerConstraints.DeadzoneBaseSize + blobDeviation;

            for (var i = 0; i < blobCount; ++i)
            {
                BitmapGenerator.DrawEllipse(bitmap, new Point(rng.NextSize(new Size(0, 0), bitmap.Size)), rng.Next(blobMin, blobMax), rng.Next(blobMin, blobMax));
            }            

            return bitmap;
        }

        private static void DrawEllipse(Bitmap bitmap, Point center, int width, int height)
        {
            System.Drawing.Pen myPen = new System.Drawing.Pen(Color.FromArgb(255, Color.Black));
            var formGraphics = Graphics.FromImage(bitmap); 
            formGraphics.DrawEllipse(myPen, center.X, center.Y, width, height);
            myPen.Dispose();
            formGraphics.Dispose();
        }
    }
}