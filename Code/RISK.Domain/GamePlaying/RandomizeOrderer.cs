﻿using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.GamePlaying
{
    public class RandomizeOrderer : IRandomizeOrderer
    {
        private readonly IRandomWrapper _randomWrapper;

        public RandomizeOrderer(IRandomWrapper randomWrapper)
        {
            _randomWrapper = randomWrapper;
        }

        public IEnumerable<T> OrderByRandomOrder<T>(IEnumerable<T> collection)
        {
            var originalCollection = collection.ToList();

            while(originalCollection.Any())
            {
                var randomIndex = _randomWrapper.Next(originalCollection.Count);
                var randomElement = originalCollection.ElementAt(randomIndex);
                originalCollection.Remove(randomElement);

                yield return randomElement;
            }
        }
    }
}