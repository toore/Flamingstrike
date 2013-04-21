﻿using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.GamePlaying
{
    public class RandomSorter : IRandomSorter
    {
        private readonly IRandomWrapper _randomWrapper;

        public RandomSorter(IRandomWrapper randomWrapper)
        {
            _randomWrapper = randomWrapper;
        }

        public IEnumerable<T> RandomSort<T>(IEnumerable<T> collection)
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