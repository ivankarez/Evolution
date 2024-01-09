using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ivankarez.Evolution.Utils
{
    public class SortedList<T> : IList<T>, IReadOnlyList<T>
    {
        public int Count => items.Count;
        public bool IsReadOnly => false;
        public int LastIndex => Count - 1;
        public IComparer<T> Comparer { get; }
        public T Last => items[LastIndex];
        public T First => items[0];
        public bool IsEmpty => Count == 0;

        private readonly List<T> items;

        public SortedList(IEnumerable<T> items, IComparer<T> comparer)
        {
            this.items = items.OrderBy(i => i, comparer).ToList();
            Comparer = comparer;
        }

        public SortedList(IComparer<T> comparer) : this(new List<T>(), comparer) { }

        public T this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }

        public void Add(T item)
        {
            if (Count == 0)
            {
                items.Add(item);
                return;
            }

            var index = -1;
            for (int i = 0; i < Count; i++)
            {
                if (Comparer.Compare(item, items[i]) < 0)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                items.Add(item);
            }
            else
            {
                items.Insert(index, item);
            }
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new InvalidOperationException("Cannot insert item to a specific index in a SortedList");
        }

        public bool Remove(T item)
        {
            return items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
