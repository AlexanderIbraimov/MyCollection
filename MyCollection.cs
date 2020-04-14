using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyCollection
{
    public class MyList<T> : IEnumerable<T>
    {
        public MyList()
        {
            array = new T[100];
            Size = 0;
        }
        public MyList(IEnumerable<T> collection)
        {
            array = collection.ToArray();
            Size = collection.Count();
        }
        public MyList(int capacity)
        {
            array = new T[capacity];
            Size = capacity;
        }
        public MyList(params T[] collection)
        {
            array = collection.ToArray();
            Size = collection.Count();
        }

        private T[] array;

        private int Size { get; set; }
        public int Count { get { return Size; } }
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException("Индекс находился вне границ списка");
                return array[index];
            }
            private set
            {
                array[index] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return array[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            var tempArray = obj as MyList<T>;
            if (tempArray.Count != this.Count)
                return false;
            else
            {
                for (int i = 0; i < Count; i++)
                {
                    if (tempArray[i] == null && this[i] == null) continue;
                    if (tempArray[i] == null ||
                        this[i] == null ||
                        !tempArray[i].Equals(this[i]))
                        return false;
                }
                return true;
            }
        }
        public override int GetHashCode()
        {
            var prime = 3123423;
            int hash = 1;
            for (int i = 0; i < array.Length && i < 20; i++)
            {
                unchecked
                {
                    hash *= prime;
                    hash ^= Convert.ToInt32(array[i]);
                }
            }
            return hash;
        }
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Count; i++)
                result += i + 1 + ":\t" + array[i] + "\n";
            return result;
        }

        public void Add(T value)
        {
            if (Size == array.Length)
            {
                var enlargedCollection = new T[Size * 2];
                Array.Copy(array, enlargedCollection, array.Length);
                array = enlargedCollection;
            }
            array[Size] = value;
            Size++;
        }
        public bool Contains(T value)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].Equals(value))
                    return true;
            return false;
        }
        public void Remove(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException("Индекс находился вне границ списка");

            var temp = new T[array.Length];
            for (int i = 0, k = 0; i < Count; i++)
            {
                if (i == index) continue;
                temp[k] = array[i];
                k++;
            }
            Size--;
            array = temp;
        }
        public void RemoveRange(int start, int end)
        {
            if (start < 0 || end < 0 || start >= Count || end >= Count)
                throw new IndexOutOfRangeException("Индекс находился вне границ списка");
            if (start > end)
            {
                int temp = start;
                start = end;
                end = temp;
            }

            var tempArray = new T[array.Length];
            for (int i = 0, k = 0; i < Count; i++)
            {
                if (start <= i && i <= end)
                    continue;
                tempArray[k] = array[i];
                k++;
            }
            Size = Size - end + start - 1;
            array = tempArray;
        }
        public void Clear()
        {
            Size = 0;
            array = new T[array.Length];
        }
        private static void Sort(Array array)
        {
            for (int i = array.Length - 1; i > 0; i--)
                for (int j = 1; j <= i; j++)
                {
                    object element1 = array.GetValue(j - 1);
                    object element2 = array.GetValue(j);
                    var comparableElement1 = (IComparable)element1;
                    if (comparableElement1.CompareTo(element2) > 0)
                    {
                        object temporary = array.GetValue(j);
                        array.SetValue(array.GetValue(j - 1), j);
                        array.SetValue(temporary, j - 1);
                    }
                }
        }
        public void Sort()
        {
            var tempArray = new T[Count];
            for (int i = 0; i < Count; i++)
                tempArray[i] = array[i];
            Sort(tempArray);
            array = tempArray;
        }
        public void CopyTo(T[] array)
        {
            if (array.Length < Count)
                throw new ArgumentException();
            for (int i = 0; i < Count; i++)
                array[i] = this[i];
        }
        public int IndexOf(T value)
        {
            for (int i = 0; i < Count; i++)
                if (array[i].Equals(value))
                    return i;
            return -1;
        }
    }
}
