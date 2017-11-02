using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_03_EAB
{
    /// <summary>
    /// EventArgs Class that can be used for an event were a property of type T changes.
    /// </summary>
    /// <typeparam name="T">The type of the property</typeparam>
    public class PropertyChangeEventArgs<T> : System.EventArgs
    {
        public readonly T oldValue;
        public readonly T newValue;
        public PropertyChangeEventArgs(T oldVal, T newVal)
        {
            oldValue = oldVal;
            newValue = newVal;
        }
    }
}