using System;

namespace Pong
{
    public static class RandomHelper
    {
        private static readonly Random Random = new Random();

        public static bool GetRandomBool()
        {
            return Random.NextDouble() > 0.5;
        }
    }
}