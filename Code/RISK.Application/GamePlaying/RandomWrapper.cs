using System;

namespace RISK.Domain.GamePlaying
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