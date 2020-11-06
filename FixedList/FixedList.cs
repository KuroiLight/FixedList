using System;
using System.Collections.Generic;

namespace FixedList
{
    public class FixedList<T>
    {
        private readonly T[] _data;
        private readonly bool[] _setValues;
        private readonly Stack<int> _availableSlots;
        public int Count { get; private set; }

        public bool IsReadOnly => true;

        public T this[int index]
        {
            get => _data[index];
            set => throw new NotSupportedException();
        }

        public int Capacity { get; private set; }

        public FixedList(int capacity)
        {
            Capacity = capacity;
            _data = new T[capacity];
            _setValues = new bool[capacity];
            _availableSlots = new Stack<int>(capacity);
            for (var i = 0; i < capacity - 1; i++) {
                _setValues[i] = false;
                _availableSlots.Push(i);
            }
        }

        public void Add(T value)
        {
            var availableIndex = _availableSlots.Pop();
            _data[availableIndex] = value;
            _setValues[availableIndex] = true;
            Count++;
        }

        public void RemoveAt(int index)
        {
            if (_setValues[index]) {
                var removedValue = _data[index];
                _data[index] = default;
                Count--;
                _availableSlots.Push(index);
                _setValues[index] = false;
            } else {
                throw new KeyNotFoundException(nameof(index));
            }
        }

        public bool Remove(T value)
        {
            for (var i = 0; i < Capacity; i++) {
                if (!_setValues[i] || !_data[i].Equals(value)) continue;
                this.RemoveAt(i);
                return true;
            }

            return false;
        }

        public int RemoveAll(Func<T, bool> predicate)
        {
            var removed = 0;
            for (var i = 0; i < Capacity; i++) {
                if (!_setValues[i] || !predicate(_data[i])) continue;
                RemoveAt(i);
                removed++;
            }

            return removed;
        }

        public int IndexOf(T item)
        {
            for (var i = 0; i < Capacity; i++) {
                if (_setValues[i] && _data[i].Equals(item)) return i;
            }

            return -1;
        }

        public void Clear()
        {
            for (var i = 0; i < Capacity; i++) {
                if (!_setValues[i]) continue;
                RemoveAt(i);
            }
        }

        public bool Contains(T item)
        {
            for (var i = 0; i < Capacity; i++) {
                if (!_setValues[i] && !_data[i].Equals(item)) continue;
                return true;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (var i = 0; i <= Capacity; i++) {
                if (!_setValues[i]) continue;
                array[arrayIndex] = _data[i];
                arrayIndex++;
            }
        }

        public FixedList<T> Compacted(bool resize = false)
        {
            var newCapacity = (resize ? Count - 1 : Capacity);
            var newFixedList = new FixedList<T>(newCapacity);

            for (var i = 0; i < Capacity; i++) {
                if (_setValues[i]) {
                    newFixedList.Add(_data[i]);
                }
            }

            return newFixedList;
        }

        public void ForEach(Action<T> action)
        {
            for (var i = 0; i < Capacity; i++) {
                if (_setValues[i]) {
                    action(_data[i]);
                }
            }
        }

        public T Find(Func<T, bool> predicate)
        {
            for (var i = 0; i < Capacity; i++) {
                if (_setValues[i] && predicate(_data[i])) {
                    return _data[i];
                }
            }

            return default;
        }
    }
}
