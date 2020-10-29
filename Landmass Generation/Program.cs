using System;
using System.Collections.Generic;

namespace terrain.gen
{
    class MainClass
    {
        static List<double> seeds, heightMaps;
        static int length = 300;

        static ImprovedNoise noise;

        public static void Main(string[] args)
        {
            noise = new ImprovedNoise();

            seeds = new List<double>();
            heightMaps = new List<double>();

            double finalHeight = 0;
            double lacunarity = 2;
            double persistance = .5;

            for (int i = 0; i < 25; i++)
            {
                seeds.Add(new Random().Next(1, 10000000));
            }
            Console.WriteLine(seeds[0]);

            for (double x = 0; x < length; x++) {
                for (double z = 0; z < length; z++) {

                    double frequency = 1;
                    double amplitude = 1;

                    int c = 0;
                    for (int oct = 0; oct < 10; oct++) {

                        double n = noise.noise(x/20 * frequency, z/20 * frequency, seeds[0]) * 2 - 1;
                        finalHeight += (n * amplitude*40)+15;

                        frequency *= Math.Pow(lacunarity, oct);
                        amplitude *= Math.Pow(persistance, oct);
                    }

                    heightMaps.Add(finalHeight);
                    finalHeight = 0;
                }
            }

            new terrain.gen.GameModules.Game(heightMaps, 1000, 720);
        }
    }
}
