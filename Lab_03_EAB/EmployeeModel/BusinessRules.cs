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
using System.ComponentModel;
using Lab_03_EAB.EmployeeModel;

namespace Lab_03_EAB
{
    /// <summary>
    /// Class that contains the business rules for the application
    /// </summary>
    [DataContract]
    [Serializable]
    public sealed class BusinessRules : ICollection<Employee>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Constants
        #endregion
        #region MemberData
        [DataMember]
        private SortedDictionary<uint, Employee> employeeCollection = new SortedDictionary<uint, Employee>();
        [DataMember]
        private string myFileName = "New...";
        [DataMember]
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
        public string FilePath
        {
            get => myPath;
            set
            {
                myPath = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(FilePath)));
                myFileName = Path.GetFileName(myPath);
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(FileName)));
            }
        }
        #endregion
        #region EventsAndHandlers
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        /// <summary>
        /// Implementation of INotifyCollectionChanged.
        /// Invokes the handler. This function is called whenever the data has changed.
        /// </summary>
        /// <param name="e">e is always going to be NotifyCollectionChangeAction.Reset</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmployeeCollection)));
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
        public bool ContainsKey(uint key)
        {
            return EmployeeCollection.ContainsKey(key);
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
        public bool AddFromEmployeeStrings(TextEmployee employee, IEnumerable<Course> courses)
        {
            if (!CanAddTextEmployee(employee)) return false;
            try
            {
                uint idToAdd = employee.EmpID ?? uint.MaxValue;
                if (employeeCollection.ContainsKey(idToAdd)) employeeCollection.Remove(idToAdd);
                switch (employee.EmpType)
                {
                    case ETYPE.CONTRACT:
                        employeeCollection.Add(idToAdd, new Contract(employee));
                        break;
                    case ETYPE.HOURLY:
                        employeeCollection.Add(idToAdd, new Hourly(employee));
                        break;
                    case ETYPE.SALARY:
                        employeeCollection.Add(idToAdd, new Salary(employee));
                        break;
                    case ETYPE.SALES:
                        employeeCollection.Add(idToAdd, new Sales(employee));
                        break;
                }
                if (courses != null)
                {
                    foreach (Course toAdd in courses)
                        employeeCollection[idToAdd].CourseRoster.Add(toAdd.CourseID, toAdd);
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
        public bool CanAddTextEmployee(TextEmployee employee)
        {
            if (employee == null) return false;
            bool result = false;
            switch(employee.EmpType)
            {
                case ETYPE.CONTRACT:
                    result = Contract.IsValidTextEmployee(employee);
                    break;
                case ETYPE.HOURLY:
                    result = Hourly.IsValidTextEmployee(employee);
                    break;
                case ETYPE.SALARY:
                    result = Salary.IsValidTextEmployee(employee);
                    break;
                case ETYPE.SALES:
                    result = Sales.IsValidTextEmployee(employee);
                    break;
                default:
                    break;
            }
            return result;
        }
        public void CopyTo(SortedDictionary<uint, Employee> destination)
        {
            foreach (var pair in employeeCollection)
                destination.Add(pair.Key, pair.Value);
        }
        [OnDeserialized]
        private void ReRegister(StreamingContext context)
        {
            foreach(var pair in employeeCollection)
            {
                employeeCollection[pair.Key].EmpIDChanged += EmpIDChangeHandler;
            }
        }
        public bool Equals(BusinessRules obj)
        {
            bool toReturn = true;
            toReturn = toReturn && myFileName == obj.myFileName;
            toReturn = toReturn && myPath == obj.myPath;
            foreach(var pair in employeeCollection)
            {
                toReturn = toReturn && (obj.employeeCollection[pair.Key] != null);
            }
            return toReturn;
        }
        #endregion
    }//End Class BusinessRules Definition
}// End Lab_03_EAB Namespace scope