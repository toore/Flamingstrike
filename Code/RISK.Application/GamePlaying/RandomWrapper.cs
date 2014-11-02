using System;

namespace RISK.Application.GamePlaying
{
    public class RandomWrapper : IRandomWrapper 
    {
        private readonly Random _random = new Random();

        public int Next(int maxValue)
        {
            return _random.Next(maxValue);
        }
    }
}