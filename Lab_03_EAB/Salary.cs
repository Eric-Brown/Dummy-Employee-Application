using System;


namespace Lab_03_EAB
{
    /// <summary>
    /// Represents a Salaried Employee
    /// </summary>
    class Salary :Employee
    {
        private const string SALARY_LBL = "Monthly Salary: ";
        public Decimal MonthlySalary { get; set; }
        /// <summary>
        /// Constructor. Hands parameters to "Employee"'s constructor.
        /// </summary>
        /// <param name="id">The Employees Identifiying Number</param>
        /// <param name="first">The Employees first name</param>
        /// <param name="last">The Employees last name</param>
        /// <param name="salary">The Employees salary</param>
        public Salary(uint id, string first, string last, Decimal salary)
            :base(id,ETYPE.SALARY, first,last)
        {
            MonthlySalary = salary;
        }

        protected Salary(uint id, string first, string last, Decimal salary, ETYPE type = ETYPE.SALARY)
        :base(id,type,first,last)
        {
            MonthlySalary = salary;
        }
        /// <summary>
        /// Gives a string representation of the class
        /// </summary>
        /// <returns>A string representing the current state of the class</returns>
        public override string ToString()
        {
            return base.ToString() + SALARY_LBL + MonthlySalary + "\n";
        }
    }

}
