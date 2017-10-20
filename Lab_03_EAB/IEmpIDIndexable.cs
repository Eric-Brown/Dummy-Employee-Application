using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_03_EAB
{
    /// <summary>
    /// Represents a more specific interface for objects which can return employee references from an employee ID value and are indexable.
    /// </summary>
    /// <typeparam name="T">The type of object returned</typeparam>
    public interface IEmpIDIndexable<T> : IIndexable<T>
    {
        T this[uint empIDIndex]
        {
            get;
            set;
        }
    }
}