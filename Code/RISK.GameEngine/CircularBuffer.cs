using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine
{
    public class CircularBuffer<T> : IEnumerable<T>
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

        public IEnumerator<T> GetEnumerator()
        {
            return GetStartingBuffer()
                .Concat(GetEndingBuffer())
                .GetEnumerator();
        }

        private IEnumerable<T> GetStartingBuffer()
        {
            return _items
                .GetRange(_index, _items.Count - _index);
        }

        private IEnumerable<T> GetEndingBuffer()
        {
            return _items.GetRange(0, _index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}