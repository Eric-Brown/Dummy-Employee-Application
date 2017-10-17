namespace Lab_03_EAB
{
    public enum ETYPE { SALARY, SALES, HOURLY, CONTRACT};
    public abstract class Employee
    {
        private const string FORMAT_STRING = "EmpID: {0}\nEmpType: {1}\nFirst Name: {2}\nLast Name: {3}\n";
        public uint EmpID { get; set; }
        public ETYPE EmpType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
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
