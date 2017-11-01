using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Windows;
using System.IO;
using System.Runtime.Serialization;

namespace Lab_03_EAB
{
    /// <summary>
    /// Class that contains the business rules for the application
    /// </summary>
    [Serializable]
    public sealed class BusinessRules : ICollection<Employee>, INotifyCollectionChanged
    {
        #region Constants
        /// <summary>
        /// Utility regex that is used to determine if a string is comprised of only characters from a to z.
        /// </summary>
        private static readonly Regex isOneWord = new Regex(@"(?i)(?<!\s*\S+\s*)[a-z]+(?!\s*\S+)", RegexOptions.Compiled);
        /// <summary>
        /// Utility regex that is used to determine if a string is comprised of only digits and a possible decimal point.
        /// </summary>
        private static readonly Regex isNumber = new Regex(@"^\d*\.?\d*$", RegexOptions.Compiled);
        /// <summary>
        /// Used to create random first names for employees.
        /// </summary>
        private static readonly string[] FIRST_NAMES = { "Stewart", "Sunny", "Grant", "Greg", "Micheal", "Seth", "Anthony", "Matthew", "Jonathon", "Jenny", "Sam" };
        /// <summary>
        /// Used to create random last names for employees.
        /// </summary>
        private static readonly string[] LAST_NAMES = { "Linder", "Brown", "DePoirot", "Johnson", "Williams", "Xavier", "Green", "Goldberg", "Greenburg", "Flotsam", "Jenkins", "Jensen", "Null" };
        /// <summary>
        /// Default number of random employees to create when creating test employees.
        /// </summary>
        private const int DEFAULT_NUM_TEST_EMPS = 10;
        #endregion
        #region MemberData
        private SortedDictionary<uint, Employee> employeeCollection = new SortedDictionary<uint, Employee>();
        private string myFileName;
        private string myPath;

        #endregion
        #region Properties
        /// <summary>
        /// Indexer that gets or sets an employee object.
        /// Order is not preserved for objects as they are added. Employees are automatically sorted by ID as they are added.
        /// If a value is assigned to this and the ID is already present, the previous value is removed.
        /// If the index is greater than the count or negative, nothing is done when attempting an assignment.
        /// </summary>
        /// <param name="i">The index of the desired value</param>
        /// <returns>An employee if valid, null otherwise.</returns>
        public Employee this[int i]
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
                if (i < 0 || i >= employeeCollection.Count)
                {
                    return;
                }
                if(value == null)
                {
                    employeeCollection.Remove(employeeCollection.ElementAt(i).Key);
                }
                else if (employeeCollection.ContainsKey(value.EmpID))
                {
                    if (Object.Equals(employeeCollection[value.EmpID], value))
                        return;
                    else
                        employeeCollection.Remove(value.EmpID);
                }
                if (value != null)
                {
                    value.EmpIDChanged += EmpIDChangeHandler;
                    employeeCollection.Add(value.EmpID, value);
                }
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }



        /// <summary>
        /// Indexer that gets or sets an Employee based on lookup though ID
        /// <preconditions>When attempting an assignment, the value's key (if it is not null)
        /// must match the ID that is being assigned to.</preconditions>
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
                if (!employeeCollection.ContainsKey(empID) || (value != null && empID != value.EmpID) || Object.Equals(value,employeeCollection[empID]))
                    return;
                employeeCollection.Remove(empID);
                if(value != null)
                {
                    employeeCollection.Add(empID, value);
                    value.EmpIDChanged += EmpIDChangeHandler;
                }
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        /// <summary>
        /// Property for the sorted dictionary. 
        /// When assigning a new dictionary, the setter will ensure that the business class is registered with all employees.
        /// <todo>Make this private.</todo>
        /// </summary>
        public SortedDictionary<uint, Employee> EmployeeCollection
        {
            get => employeeCollection;
            set
            {
                employeeCollection.Clear();
                foreach (var pair in value)
                {
                    employeeCollection.Add(pair.Key, pair.Value);
                    pair.Value.EmpIDChanged += EmpIDChangeHandler;
                }
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        public string FileName => myFileName;
        public string FilePath => myPath;
        #endregion
        #region EventsAndHandlers
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Implementation of INotifyCollectionChanged.
        /// Invokes the handler. This function is called whenever the data has changed.
        /// </summary>
        /// <param name="e">e is always going to be NotifyCollectionChangeAction.Reset</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
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
        #endregion
        #region Constructors
        public BusinessRules()
        {
        }
        public BusinessRules(SortedDictionary<uint, Employee> dictionary, string path = null)
        {
            EmployeeCollection = dictionary;
            if(!string.IsNullOrEmpty(path))
            {
                myPath = path;
                myFileName = Path.GetFileName(path);
            }
        }
        #endregion
        #region ICollection Implementation
        /// <summary>
        /// Returns the number of currently contained employees.
        /// </summary>
        int ICollection<Employee>.Count => employeeCollection.Count;
        /// <summary>
        /// Expresses whether or not the collection is non-modifiable.
        /// </summary>
        public bool IsReadOnly => false;
        /// <summary>
        /// Exposes the datastructures clear method. Might remove later for enforcing encapsulation.
        /// </summary>
        public void Clear()
        {
            employeeCollection.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        /// <summary>
        /// Returns an enumerator for employee objects that are contained in the collection.
        /// </summary>
        /// <returns>An enumerator which returns Employee objects.</returns>
        public IEnumerator<Employee> GetEnumerator()
        {
            foreach (KeyValuePair<uint, Employee> pair in employeeCollection.ToList())
            {
                yield return pair.Value;
            }
        }
        /// <summary>
        /// Returns an enumerator that iterates through employee objects.
        /// </summary>
        /// <returns>An enumerator that returns employee objects.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (KeyValuePair<uint, Employee> pair in employeeCollection.ToList())
            {
                yield return pair.Value;
            }
        }
        /// <summary>
        /// Adds an employee to the collection.
        /// If an employee already has the same ID, the old value will be removed.
        /// Null values are ignored.
        /// </summary>
        /// <param name="item">The value to add.</param>
        public void Add(Employee item)
        {
            if (item == null) return;
            if (employeeCollection.ContainsKey(item.EmpID))
            {
                employeeCollection.Remove(item.EmpID);
            }
            employeeCollection.Add(item.EmpID, item);
            item.EmpIDChanged += EmpIDChangeHandler;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        /// <summary>
        /// Determines whether a given item is contained in the collection
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <returns>A bool specifying whether the item is in the collection</returns>
        public bool Contains(Employee item)
        {
            return employeeCollection.ContainsValue(item);
        }
        /// <summary>
        /// Copies contained employees to an array starting at the specified index.
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="arrayIndex">The index to start at</param>
        public void CopyTo(Employee[] array, int arrayIndex)
        {
            employeeCollection.Values.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Removes the specified value if it is contained in the collection.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <returns>Whether or not the removal was successful</returns>
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
        #endregion
        #region Methods
        /// <summary>
        /// Attempts to add an employee of the specified type through a string array.
        /// </summary>
        /// <param name="selected">The type of employee to create</param>
        /// <param name="attributeArray">A string array of the attributes. MUST have the order of: ID, First, Last, Sup1, Sup2, Sup3</param>
        /// <returns>Whether or not the operation was successful</returns>
        /// <exception cref="OverflowException">Thrown if the formatting was good, but results in an overflow for the datatype.</exception>
        public bool AddFromStringArray(ETYPE selected, string[] attributeArray)
        {
            try
            {
                uint idToAdd = uint.Parse(attributeArray[0]);
                if (employeeCollection.ContainsKey(idToAdd)) employeeCollection.Remove(idToAdd);
                switch (selected)
                {
                    case ETYPE.CONTRACT:
                        employeeCollection.Add(idToAdd, new Contract(idToAdd, attributeArray[1], attributeArray[2], decimal.Parse(attributeArray[3])));
                        break;
                    case ETYPE.HOURLY:
                        employeeCollection.Add(idToAdd, new Hourly(idToAdd, attributeArray[1], attributeArray[2], decimal.Parse(attributeArray[3]), double.Parse(attributeArray[4])));
                        break;
                    case ETYPE.SALARY:
                        employeeCollection.Add(idToAdd, new Salary(idToAdd, attributeArray[1], attributeArray[2], decimal.Parse(attributeArray[3])));
                        break;
                    case ETYPE.SALES:
                        employeeCollection.Add(idToAdd, new Sales(idToAdd, attributeArray[1], attributeArray[2], decimal.Parse(attributeArray[3]), decimal.Parse(attributeArray[4]), decimal.Parse(attributeArray[5])));
                        break;
                }
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                return true;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is FormatException)
                    return false;
                else
                    throw;
            }
        }
        /// <summary>
        /// Determines whether or not a call to AddFromStringArray will succeed.
        /// </summary>
        /// <param name="selectedItem">The type of the employee that could be added.</param>
        /// <param name="attributeArray">Array with entries that are ordered: ID, First, Last, Sup1, Sup2, Sup3</param>
        /// <returns>Whether or not AddFromStringArray will succeed.</returns>
        public bool CanAddFromStringArray(ETYPE selectedItem, string[] attributeArray)
        {
            //All employees will have: ID, First, Last
            try
            {
                if (isNumber.IsMatch(attributeArray[0]) && isOneWord.IsMatch(attributeArray[1]) && isOneWord.IsMatch(attributeArray[2]))
                {
                    switch (selectedItem)
                    {
                        //Contract has 1
                        case ETYPE.CONTRACT:
                            return (isNumber.IsMatch(attributeArray[3]));
                        //Hourly has 2
                        case ETYPE.HOURLY:
                            return (isNumber.IsMatch(attributeArray[3]) && isNumber.IsMatch(attributeArray[4]));
                        //Salary has 1
                        case ETYPE.SALARY:
                            return (isNumber.IsMatch(attributeArray[3]));
                        //Sales has 3
                        case ETYPE.SALES:
                            return (isNumber.IsMatch(attributeArray[3]) && isNumber.IsMatch(attributeArray[4]) && isNumber.IsMatch(attributeArray[5]));
                    }
                }
            }
            //Exceptions are silenced
            catch (Exception e)
            {
            }
            return false;
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
                        employeeCollection.Add((uint)i, new Contract((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.HOURLY:
                        employeeCollection.Add((uint)i, new Hourly((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next()),
                            random.NextDouble() * random.Next()));
                        break;
                    case ETYPE.SALARY:
                        employeeCollection.Add((uint)i, new Salary((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.SALES:
                        employeeCollection.Add((uint)i, new Sales((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.Next() * random.NextDouble()),
                            (decimal)(random.Next() * random.NextDouble()),
                            (decimal)(random.Next() * random.NextDouble())));
                        break;
                }//End Switch
            }//End for loop
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public void CopyTo(SortedDictionary<uint, Employee> destination)
        {
            foreach (var pair in employeeCollection)
                destination.Add(pair.Key, pair.Value);
        }
        #endregion
    }//End Class BusinessRules Definition
}// End Lab_03_EAB Namespace scope