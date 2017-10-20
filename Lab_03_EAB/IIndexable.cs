using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_03_EAB
{
    /// <summary>
    /// Represents an object which contains an index of Objects
    /// </summary>
    /// <typeparam name="T">The type of the object returned by the index.</typeparam>
    public interface IIndexable<T>
    {
        T this[int index]
        {
            get;
            set;
        }
        int Count();
        void Clear();
        T Last();
    }
}