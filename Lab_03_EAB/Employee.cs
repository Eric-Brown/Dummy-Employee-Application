namespace Lab_03_EAB
{
    enum ETYPE { SALARY, SALES, HOURLY, CONTRACT};
    abstract class Employee
    {
        private const string EMPID_LBL = "EmpID: ",
            EMPTYPE_LBL = "\nEmpType: ",
            FIRST_LBL = "\nFirst Name: ",
            LAST_LBL = "\nLast Name: ";
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
            return EMPID_LBL + EmpID + EMPTYPE_LBL + EmpType + FIRST_LBL + FirstName + LAST_LBL + LastName + '\n';
        }
    }
}
