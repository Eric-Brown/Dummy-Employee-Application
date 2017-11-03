using System;
using System.Runtime.Serialization;

namespace Lab_03_EAB
{
    /// <summary>
    /// Represents a Salaried Employee
    /// </summary>
    [DataContract]
    [Serializable]
    public class Salary :Employee
    {
        [DataMember]
        private const string SALARY_FORMAT_STRING = "Monthly Salary: {0}\n",
            BAD_VAL_ERR_MSG = "Only non-negative values of salary may be used to construct a Salary employee.";
        /// <summary>
        /// MonthlySalary property and backing field. Negative values are rejected.
        /// </summary>
        [DataMember]
        private decimal monthlySalary;
        public Decimal MonthlySalary
        {
            get => monthlySalary;
            set
            {
                if (value >= 0)
                    monthlySalary = value;
            }
        }
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
            if (salary < 0) throw new ArgumentException(BAD_VAL_ERR_MSG);
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
            return base.ToString() + string.Format(SALARY_FORMAT_STRING, MonthlySalary);
        }
    }

}
