using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine
{
    public class Sequence<T> : IEnumerable<T>
    {
        private readonly List<T> _items;
        private int _index;

        public Sequence(params T[] items)
        {
            _items = items.ToList();
        }

        public T Next()
        {
            if (_index >= _items.Count)
            {
                _index = 0;
            }

            return _items[_index++];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}