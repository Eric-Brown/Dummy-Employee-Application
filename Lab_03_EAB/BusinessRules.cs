using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Lab_03_EAB
{
    /// <summary>
    /// Class that contains the business rules for the application
    /// </summary>
    public sealed class BusinessRules :IEmpIDIndexable<Employee>
    {
        const int NUM_EMPS_IN_LIST = 10;
        private SortedDictionary<uint, Employee> employee = new SortedDictionary<uint, Employee>();
        /// <summary>
        /// Indexer which returns the value stored in the internal data structure at the index given.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>A value if valid, null otherwise.</returns>
        public Employee this [int i]
        {
            get
            {
                if (i >= employee.Count)
                    return null;
                else
                    //KeyValue is immutable, so we use the key retrieved to get a mutable element.
                    return employee[employee.ElementAt(i).Key];
            }
            set
            {
                if (i >= employee.Count)
                {
                    if (employee.ContainsKey(value.EmpID))
                    {
                        employee[value.EmpID] = value;
                    }
                    else
                        employee.Add(value.EmpID, value);
                }
                else
                    employee[employee.ElementAt(i).Key] = value;
            }
        }
        /// <summary>
        /// Allows indexing into the internal datastructure through looking up the employee's ID
        /// </summary>
        /// <param name="empID">The Employee ID to look-up</param>
        /// <returns>An employee reference if the Employee ID is found. Null otherwise.</returns>
        public Employee this[uint empID]
        {
            get
            {
                if (employee.ContainsKey(empID))
                    return employee[empID];
                else
                    return null;
            }
            set
            {
                if (employee.ContainsKey(empID))
                    employee[empID] = value;
                else
                    employee.Add(value.EmpID, value);
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
        public Employee Last() => employee.Last().Value;

        /// <summary>
        /// Exposes the datastructures count method.
        /// </summary>
        /// <returns>Returns the number of items in the datastructure</returns>
        public int Count() => employee.Count();

        /// <summary>
        /// Returns a value from the enumerator of the datastructure
        /// </summary>
        /// <returns>An enumerator which returns Employee objects.</returns>
        public IEnumerator<Employee> GetEnumerator()
        {
            foreach (KeyValuePair<uint,Employee> pair in employee)
            {
                yield return pair.Value;
            }
        }
    }
}