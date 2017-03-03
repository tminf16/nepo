using System;
using System.Collections.Generic;
using System.Drawing;
using LibNoise;
using LibNoise.Builder;
using LibNoise.Filter;
using LibNoise.Model;
using LibNoise.Primitive;

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
                BitmapGenerator.FillEllipse(bitmap, new Point(rng.NextSize(new Size(0, 0), bitmap.Size)), rng.Next(blobMin, blobMax), rng.Next(blobMin, blobMax));
            }            

            return bitmap;
        }

        private static void FillEllipse(Bitmap bitmap, Point center, int width, int height)
        {
            System.Drawing.Pen myPen = new System.Drawing.Pen(Color.FromArgb(255, Color.Blue));
            var formGraphics = Graphics.FromImage(bitmap); 
            formGraphics.DrawEllipse(myPen, center.X, center.Y, width, height);
            myPen.Dispose();
            formGraphics.Dispose();
        }

        public static Bitmap AddHeightNoise(Bitmap bitmap, Random rng)
        {
            var noiseMap = GenerateNoiseMap(rng, bitmap.Size);
            for (var i = 0; i < bitmap.Size.Height; ++i)
            {
                for (var j = 0; j < bitmap.Size.Width; ++j)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    var diff = (int)(127 * (noiseMap.GetValue(i, j)));

                    int old = 127;
                    if (i > 0)
                    {
                        if (j > 0)
                        {
                            old = (bitmap.GetPixel(i - 1, j).A + bitmap.GetPixel(i, j - 1).A) / 2;
                        }
                        else
                        {
                            old = bitmap.GetPixel(i - 1, j).A;
                        }       
                    }
                    else
                    {
                        if (j > 0)
                        {
                            old = bitmap.GetPixel(i, j - 1).A;
                        }                       
                    }

                    var newA = Math.Min(Math.Max(0, diff + old), 255);                                      

                    bitmap.SetPixel(i, j, Color.FromArgb(newA, pixel));
                }
            }
            
            return bitmap;
        }

        public static IMap2D<float> GenerateNoiseMap(Random rng, Size size)
        {
            var builder = new LibNoise.Builder.NoiseMapBuilderPlane(1, size.Width, 1, size.Height, false);
            

            var source = new SimplexPerlin(rng.Next(), NoiseQuality.Standard);
            builder.SourceModule = source;

            builder.SetSize(size.Width, size.Height);
            var baseMap = new NoiseMap(size.Width, size.Height);

            builder.NoiseMap = baseMap;
            
            builder.Build();

            var map = builder.NoiseMap;
            return builder.NoiseMap;
        }
    }
}