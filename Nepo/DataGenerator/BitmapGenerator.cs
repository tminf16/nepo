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
            System.Drawing.Brush myPen = new System.Drawing.SolidBrush(Color.FromArgb(255, Color.Black));
            var formGraphics = Graphics.FromImage(bitmap); 
            formGraphics.FillEllipse(myPen, center.X, center.Y, width, height);
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
                    bitmap.SetPixel(i, j, Color.FromArgb((int)(127*noiseMap.GetValue(i,j)+127), pixel));
                }
            }
            
            return bitmap;

        }

        public static IMap2D<float> GenerateNoiseMap(Random rng, Size size)
        {
            var builder = new LibNoise.Builder.NoiseMapBuilderPlane(1, size.Width, 1, size.Height, false);
            

            var source = new SimplexPerlin();
            builder.SourceModule = source;

            builder.SetSize(size.Width, size.Height);
            var baseMap = new NoiseMap(size.Width, size.Height);

            builder.NoiseMap = baseMap;
            
            builder.Build();

            var map = builder.NoiseMap;
            map.MinMax(out float min, out float max);
            Console.WriteLine(map.Width);
            Console.WriteLine(map.GetValue(200, 300));
            Console.WriteLine($"Min: {min}, Max: {max}");
            return builder.NoiseMap;
        }
    }
}