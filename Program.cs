using System;
using System.Collections.Generic;

namespace terrain.gen
{
    class MainClass
    {
        static List<double> seeds, heightMaps;
        static int length = 200;

        static ImprovedNoise noise;

        public static void Main(string[] args)
        {
            noise = new ImprovedNoise();

            seeds = new List<double>();
            heightMaps = new List<double>();

            double finalHeight = 0;
            double amplitude = 10.8;

            for (int i = 0; i < 4; i++)
            {
                seeds.Add(new Random().Next(1, 10000000));
            }

            double frequency = 50; // size of the terrain generation

            for (double x = 0; x < length; x++) {
                for (double z = 0; z < length; z++) {

                    int c = 1;
                    foreach (int seed in seeds)
                        finalHeight += noise.noise(x/length* (frequency/(2*c)), z/length* (frequency/(2*c)), seed)*amplitude;
                        c++;

                    heightMaps.Add(finalHeight);
                    Console.WriteLine(heightMaps.Count);

                    finalHeight = 0;
                }
            }

            new Game(heightMaps, 1000, 720);
        }
    }
}
