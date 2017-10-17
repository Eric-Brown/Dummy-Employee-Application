using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Lab_03_EAB
{
    /// <summary>
    /// Class that contains the business rules for the application
    /// </summary>
    public sealed class BusinessRules
    {
        const int NUM_EMPS_IN_LIST = 10;
        private List<Employee> employee = new List<Employee>(NUM_EMPS_IN_LIST);
        /// <summary>
        /// Indexer which returns the value stored in the internal data structure at the index given.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>A value if valid, null otherwise.</returns>
        public Employee this [int i]
        {
            get
            {
                if (i == employee.Count)
                    return null;
                else return employee[i];
            }
            set
            {
                if (i == employee.Count)
                    employee.Add(value);
                else
                    employee[i] = value;
            }
        }

        /// <summary>
        /// Exposes the datastructures clear method. Might remove later for enforcing encapsulation.
        /// </summary>
        public void Clear() => employee.Clear();

        /// <summary>
        /// Exposes the datastructures last element added.
        /// </summary>
        /// <returns></returns>
        public Employee Last() => employee.Last();

        /// <summary>
        /// Exposes the datastructures count method.
        /// </summary>
        /// <returns>Returns the number of items in the datastructure</returns>
        public int Count() => employee.Count();

        /// <summary>
        /// Gets the enumerator of the datastructure
        /// </summary>
        /// <returns>An enumerator which returns Employee objects.</returns>
        public IEnumerator<Employee> GetEnumerator() => employee.GetEnumerator();
    }
}