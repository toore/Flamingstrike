using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine
{
    public class CircularBuffer<T>
    {
        private readonly List<T> _items;
        private int _index;

        public CircularBuffer(params T[] items)
        {
            _items = items.ToList();
            _index = items.Length - 1;
        }

        public T Next()
        {
            _index++;
            if (_index >= _items.Count)
            {
                _index = 0;
            }

            return _items[_index];
        }
    }
}