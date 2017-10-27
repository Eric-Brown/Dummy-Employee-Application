using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Windows;


namespace Lab_03_EAB
{
    /// <summary>
    /// Class that contains the business rules for the application
    /// </summary>
    [Serializable]
    public sealed class BusinessRules :IList<Employee>, INotifyCollectionChanged
    {
        private static readonly Regex isWord = new Regex(@"(?i)[a-z]+(?!=\b[a-z]+)");
        private static readonly Regex isNumber = new Regex(@"^\d*\.?\d*$");
        private readonly string[] FIRST_NAMES = { "Stewart", "Sunny", "Grant", "Greg", "Micheal", "Seth", "Anthony", "Matthew", "Jonathon", "Jenny", "Sam" };
        private readonly string[] LAST_NAMES = { "Linder", "Brown", "DePoirot", "Johnson", "Williams", "Xavier", "Green", "Goldberg", "Greenburg", "Flotsam", "Jenkins", "Jensen", "Null" };

        private const int DEFAULT_NUM_TEST_EMPS = 10;
        private SortedDictionary<uint, Employee> employeeCollection = new SortedDictionary<uint, Employee>();
        public SortedDictionary<uint,Employee> EmployeeCollection
        {
            get => employeeCollection;
            set
            {
                employeeCollection.Clear();
                foreach(var pair in value)
                {
                    employeeCollection.Add(pair.Key, pair.Value);
                    pair.Value.EmpIDChanged += EmpIDChangeHandler;
                }
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        int ICollection<Employee>.Count => employeeCollection.Count;

        public bool IsReadOnly => false;

        /// <summary>
        /// Indexer which returns the value stored in the internal data structure at the index given.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>A value if valid, null otherwise.</returns>
        public Employee this [int i]
        {
            get
            {
                if (i >= employeeCollection.Count || i < 0)
                    return null;
                else
                    //KeyValue is immutable, so we use the key retrieved to get a mutable element.
                    return employeeCollection[employeeCollection.ElementAt(i).Key];
            }
            set
            {
                if (i >= employeeCollection.Count)
                {
                    if (employeeCollection.ContainsKey(value.EmpID))
                    {
                        employeeCollection[value.EmpID] = value;
                    }
                    else
                    {
                        employeeCollection.Add(value.EmpID, value);
                    }
                }
                else
                {
                    employeeCollection[employeeCollection.ElementAt(i).Key] = value;
                }
                if (value != null) value.EmpIDChanged += EmpIDChangeHandler;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        /// <summary>
        /// Event Handler for when an employees id has changed
        /// </summary>
        /// <param name="sender">The employee whos ID has changed</param>
        /// <param name="args">EventArgs subtype that contains the old and new values of the ID</param>
        private void EmpIDChangeHandler(object sender, PropertyChangeEventArgs<uint> args)
        {
            //Remove the old entry and update to a new entry
            employeeCollection[args.newValue] = (Employee)sender;
            employeeCollection.Remove(args.oldValue);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        internal void AddFromArray(ETYPE selected, string[] v)
        {
            try
            {
                uint idToAdd = uint.Parse(v[0]);
                if ( employeeCollection.ContainsKey(idToAdd)) employeeCollection.Remove(idToAdd);
            switch (selected)
                {
                    case ETYPE.CONTRACT:
                        employeeCollection.Add(idToAdd, new Contract(idToAdd, v[1], v[2], decimal.Parse(v[3])));
                    break;
                    case ETYPE.HOURLY:
                        employeeCollection.Add(idToAdd, new Hourly(idToAdd, v[1], v[2], decimal.Parse(v[3]), double.Parse(v[4])));
                        break;
                    case ETYPE.SALARY:
                        employeeCollection.Add(idToAdd, new Salary(idToAdd, v[1], v[2], decimal.Parse(v[3])));
                        break;
                    case ETYPE.SALES:
                        employeeCollection.Add(idToAdd, new Sales(idToAdd, v[1], v[2], decimal.Parse(v[3]), decimal.Parse(v[4]), decimal.Parse(v[5])));
                        break;
                }
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            catch(FormatException e)
            {
                MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedItem">The type of the employee that could be added.</param>
        /// <param name="v">Array with entries that are ordered: ID, First, Last, Sup1, Sup2, Sup3</param>
        /// <returns></returns>
        internal bool CanAddEntry(ETYPE selectedItem, string[] v)
        {
            //test shit here
            //All employees will have: ID, First, Last
            try
            {
                if (isNumber.IsMatch(v[0]) && isWord.IsMatch(v[1]) && isWord.IsMatch(v[2]))
                {
                    switch (selectedItem)
                    {
                        //Contract has 1
                        case ETYPE.CONTRACT:
                            return (isNumber.IsMatch(v[3]));
                        //Hourly has 2
                        case ETYPE.HOURLY:
                            return (isNumber.IsMatch(v[3]) && isNumber.IsMatch(v[4]));
                        //Salary has 1
                        case ETYPE.SALARY:
                            return (isNumber.IsMatch(v[3]));
                        //Sales has 3
                        case ETYPE.SALES:
                            return (isNumber.IsMatch(v[3]) && isNumber.IsMatch(v[4]) && isNumber.IsMatch(v[5]));
                    }
                }
            }
            //Exception catch will cause a return of false
            catch (Exception e)
            { }
            return false;
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
                if (employeeCollection.ContainsKey(empID))
                    return employeeCollection[empID];
                else
                    return null;
            }
            set
            {
                if (employeeCollection.ContainsKey(empID))
                    employeeCollection[empID] = value;
                else
                    employeeCollection.Add(value.EmpID, value);
                if (value != null) value.EmpIDChanged += EmpIDChangeHandler;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        internal void AddTestEmps(int numTestEmps = DEFAULT_NUM_TEST_EMPS)
        {
            Random random = new Random();
            //RTBxOutput.Document.Blocks.Clear();
            Clear();
            int numETypes = Enum.GetNames(typeof(ETYPE)).Length;
            //Iterate through the numbers and choose randomly from the first and last names. Other values are randomly generated using Random
            for (int i = 0; i < numTestEmps; i++)
            {
                switch ((ETYPE)(i % numETypes))
                {
                    case ETYPE.CONTRACT:
                        employeeCollection.Add((uint)i,new Contract((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.HOURLY:
                        employeeCollection.Add((uint)i,new Hourly((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next()),
                            random.NextDouble() * random.Next()));
                        break;
                    case ETYPE.SALARY:
                        employeeCollection.Add((uint)i,new Salary((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.SALES:
                        employeeCollection.Add((uint)i,new Sales((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.Next() * random.NextDouble()),
                            (decimal)(random.Next() * random.NextDouble()),
                            (decimal)(random.Next() * random.NextDouble())));
                        break;
                }//End Switch
            }//End for loop
            OnCollectionChanged(new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Exposes the datastructures clear method. Might remove later for enforcing encapsulation.
        /// </summary>
        public void Clear()
        {
            employeeCollection.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Exposes the datastructures last element added.
        /// </summary>
        /// <returns></returns>
        public Employee Last() => (employeeCollection.Count == 0) ? null : employeeCollection.Last().Value;

        /// <summary>
        /// Implements the function from IList
        /// Returns a value from the enumerator of the datastructure
        /// </summary>
        /// <returns>An enumerator which returns Employee objects.</returns>
        public IEnumerator<Employee> GetEnumerator()
        {
            foreach (KeyValuePair<uint,Employee> pair in employeeCollection.ToList())
            {
                yield return pair.Value;
            }
        }

        public int IndexOf(Employee item)
        {
            if (employeeCollection.ContainsKey(item.EmpID))
            {
                int i = 0;
                while (!Object.Equals(employeeCollection.ElementAt(i).Value, item))
                    ++i;
                return i;
            }
            else return -1;
        }

        /// <summary>
        /// Due to working with a sorted dictionary, this cannot be implemented. Do not call.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <exception cref="NotImplementedException">This will always throw</exception>
        public void Insert(int index, Employee item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            Employee[] temp = employeeCollection.Values.ToArray();
            if (index < temp.Count())
            {
                employeeCollection.Remove(temp[index].EmpID);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void Add(Employee item)
        {
            if (employeeCollection.ContainsKey(item.EmpID))
                employeeCollection[item.EmpID] = item;
            else
                employeeCollection.Add(item.EmpID, item);
            if (item != null) item.EmpIDChanged += EmpIDChangeHandler;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(Employee item)
        {
            return employeeCollection.ContainsValue(item);
        }

        public void CopyTo(Employee[] array, int arrayIndex)
        {
            employeeCollection.Values.CopyTo(array, arrayIndex);
        }

        public void CopyTo(SortedDictionary<uint,Employee> destination)
        {
            foreach (var pair in employeeCollection)
                destination.Add(pair.Key, pair.Value);
        }

        public bool Remove(Employee item)
        {
            if (employeeCollection.ContainsValue(item))
            {
                employeeCollection.Remove(item.EmpID);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                return true;
            }
            else
                return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (KeyValuePair<uint, Employee> pair in employeeCollection.ToList())
            {
                yield return pair.Value;
            }
        }
    }
}