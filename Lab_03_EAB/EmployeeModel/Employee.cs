using System.Runtime.Serialization;
using System;
namespace Lab_03_EAB
{
    /// <summary>
    /// Enum that represents the known subtypes of an employee.
    /// <warning>Other code in the project relies on these being default integral values.</warning>
    /// </summary>
    [DataContract]
    [Serializable]
    public enum ETYPE
    {
        [EnumMember]
        SALARY,
        [EnumMember]
        SALES,
        [EnumMember]
        HOURLY,
        [EnumMember]
        CONTRACT
    };
    /// <summary>
    /// Abstract class which contains all the information and behaviors which are in common for all employees.
    /// </summary>
    [DataContract, KnownType(typeof(Contract)), KnownType(typeof(Salary)), KnownType(typeof(Sales)), KnownType(typeof(Hourly))]
    [Serializable]
    public abstract class Employee
    {
        #region Constants
        /// <summary>
        /// Used to call string.Format without having to use several "magic" strings.
        /// </summary>
        [DataMember]
        private const string FORMAT_STRING = "EmpID: {0}\nEmpType: {1}\nFirst Name: {2}\nLast Name: {3}\n";
        #endregion
        #region EventsAndHandlers
        /// <summary>
        /// Event delegate for when the employee's ID has been changed.
        /// </summary>
        public event System.EventHandler<PropertyChangeEventArgs<uint>> EmpIDChanged;
        /// <summary>
        /// Broadcasts to any listeners that the employee ID has changed.
        /// </summary>
        /// <param name="e">EventArgs subtype that contains old and new value of changed property</param>
        protected virtual void OnEmpIDChanged(PropertyChangeEventArgs<uint> e)
        {
            EmpIDChanged?.Invoke(this, e);
        }
        #endregion
        #region Properties
        [DataMember]
        private uint empID;
        /// <summary>
        /// Represents the unique ID of the employee.
        /// </summary>
        public uint EmpID
        {
            get => empID;
            set
            {
                if (value == empID)
                    return;
                uint oldValue = empID;
                empID = value;
                OnEmpIDChanged(new PropertyChangeEventArgs<uint>(oldValue, empID));
            }
        }
        [DataMember]
        private string firstName;
        /// <summary>
        /// Represents the first name of the employee. Rejects null or empty values.
        /// </summary>
        public string FirstName
        {
            get => firstName;
            set
            {
                if (!string.IsNullOrEmpty(value))
                    firstName = value;
            }
        }
        [DataMember]
        private string lastName;
        /// <summary>
        /// Represents the last name of the employee. Rejects null or empty values.
        /// </summary>
        public string LastName
        {
            get => lastName;
            set
            {
                if (!string.IsNullOrEmpty(value))
                    lastName = value;
            }
        }
        [DataMember]
        private ETYPE empType;
        /// <summary>
        /// Represents the type of the employee.
        /// </summary>
        public ETYPE EmpType { get => empType; private set => empType = value; }
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor.
        /// Is protected since only subtypes of this class should call it.
        /// </summary>
        /// <param name="type">The type of the employee.</param>
        /// <param name="id">The Employees Identifiying Number</param>
        /// <param name="first">The Employees first name</param>
        /// <param name="last">The Employees last name</param>
        protected Employee(uint id, ETYPE type, string first, string last)
        {
            empID = id;
            empType = type;
            firstName = first;
            lastName = last;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Gives a string representation of the class
        /// </summary>
        /// <returns>A string representing the current state of the class</returns>
        public override string ToString()
        {
            return string.Format(FORMAT_STRING, EmpID, EmpType, FirstName, LastName);
        }
        #endregion
    }//End Class Employee Definition
}//End Namespace Lab_03_EAB Scope
