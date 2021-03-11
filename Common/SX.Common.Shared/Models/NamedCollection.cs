using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SX.Common.Shared.Models
{
    public abstract class NamedCollection<T> : INamedCollection<T>
      where T : class, INamed
    {
        protected List<T> _collection = new List<T>();

        public NamedCollection() { }

        public NamedCollection(IEnumerable<T> collection)
        {
            if (collection != null)
                _collection = collection.ToList();
        }

        public T this[int index]
        { get { return _collection[index]; } }

        public T this[string name]
        { get { return this.Get(name); } }

        public T Get(string name)
        { return _collection.FirstOrDefault(e => e.Name.Equals(name, CommonService.StringComparison)); }

        public T Get(int index)
        { return _collection[index]; }

        public virtual void Add(T item)
        {
            if (item == null)
                return;

            _collection.Add(item);
        }

        public virtual bool Remove(T item)
        {
            return _collection.Remove(item);
        }

        public virtual void Clear()
        {
            _collection.Clear();
        }

        public abstract bool Set(T item);

        public virtual bool Set(INamedCollection<T> collection)
        {
            if (collection == null)
                throw new CustomArgumentException("Невозможно установить пустую коллекцию!");

            var result = false;

            // обновляем элементы из новой коллекции
            foreach (var item in collection)
            {
                result = this.Set(item) || result;
            }

            // удаляем лишние элементы из текущей коллекции
            for (int i = this.Count - 1; i >= 0; i--)
            {
                var current = this[i];

                var exist = collection.Get(current.Name);
                if (exist == null)
                {
                    this.Remove(current);
                    result = true;
                }
            }

            return result;
        }

        public int Count
        { get { return _collection.Count; } }

        public bool IsReadOnly
        { get { return false; } }

        public object SyncRoot
        { get { return ((ICollection)_collection).SyncRoot; } }

        public bool IsSynchronized
        { get { return ((ICollection)_collection).IsSynchronized; } }

        public bool Contains(string name)
        { return this.Get(name) != null; }

        public bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_collection).CopyTo(array, index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
