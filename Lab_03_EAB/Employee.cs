using System.Runtime.Serialization;
using System;
namespace Lab_03_EAB
{
    [Serializable]
    public enum ETYPE { SALARY, SALES, HOURLY, CONTRACT};
    [Serializable]
    public abstract class Employee
    {
        public event System.EventHandler<PropertyChangeEventArgs<uint>> EmpIDChanged;
        
        private const string FORMAT_STRING = "EmpID: {0}\nEmpType: {1}\nFirst Name: {2}\nLast Name: {3}\n";
        
        private uint empID;

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
        /// <summary>
        /// Broadcasts to any listeners that the employee ID has changed.
        /// </summary>
        /// <param name="e">EventArgs subtype that contains old and new value of changed property</param>
        protected virtual void OnEmpIDChanged(PropertyChangeEventArgs<uint> e)
        {
            EmpIDChanged?.Invoke(this, e);
        }
        private ETYPE empType;
        public ETYPE EmpType { get => empType; set => empType = value; }
        /// <summary>
        /// FirstName property and backing field. Rejects null or empty names.
        /// </summary>
        
        private string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                if (!string.IsNullOrEmpty(value))
                    firstName = value;
            }
        }
        /// <summary>
        /// LastName property and backing field. Rejects null or empty names.
        /// </summary>
        
        private string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                if (!string.IsNullOrEmpty(value))
                    lastName = value;
            }
        }
        /// <summary>
        /// Constructor. Initializes fields.
        /// </summary>
        /// <param name="id">The Employees Identifiying Number</param>
        /// <param name="first">The Employees first name</param>
        /// <param name="last">The Employees last name</param>
        public Employee(uint id, ETYPE type, string first, string last)
        {
            EmpID = id;
            EmpType = type;
            FirstName = first;
            LastName = last;
        }
        /// <summary>
        /// Gives a string representation of the class
        /// </summary>
        /// <returns>A string representing the current state of the class</returns>
        public override string ToString()
        {
            return string.Format(FORMAT_STRING, EmpID, EmpType, FirstName, LastName);
        }
    }
}
