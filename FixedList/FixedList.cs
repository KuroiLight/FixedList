using System;
using System.Collections.Generic;

namespace FixedList
{
    public class FixedList<T>
    {
        private readonly T[] _data;
        private readonly bool[] _values;
        public int Count { get; private set; }


        public readonly int Capacity;
        private readonly Stack<int> _emptyIndices;

        public FixedList(int capacity)
        {
            Capacity = capacity;
            _data = new T[capacity];
            _values = new bool[capacity];
            _emptyIndices = new Stack<int>(capacity);
            for (var i = 0; i <= capacity - 1; i++) {
                _values[i] = false;
                _emptyIndices.Push(i);
            }
        }

        public void Add(T value)
        {
            var availableIndex = _emptyIndices.Pop();
            _data[availableIndex] = value;
            _values[availableIndex] = true;
            Count++;
        }

        private T RemoveAt(int index)
        {
            if (_values[index]) {
                var removedValue = _data[index];
                _data[index] = default;
                Count--;
                _emptyIndices.Push(index);
                _values[index] = false;
                return removedValue;
            } else {
                throw new KeyNotFoundException(nameof(index));
            }
        }

        public T Remove(T value)
        {
            for (var i = 0; i <= Capacity; i++) {
                if (_values[i] && _data[i].Equals(value)) {
                    return RemoveAt(i);
                }
            }
            throw new KeyNotFoundException(nameof(value));
        }

        public int RemoveAll(Func<T, bool> predicate)
        {
            var removed = 0;
            for (var i = 0; i <= Capacity; i++) {
                if (!_values[i] || !predicate(_data[i])) continue;
                RemoveAt(i);
                removed++;
            }

            return removed;
        }
    }
}
