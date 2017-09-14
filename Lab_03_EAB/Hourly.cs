using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_03_EAB
{
    sealed class Hourly : Employee
    {
        private const string RATE_LBL = "Hourly Rate: ",
            HOURS_LBL = "\nHours Worked: ";
        public Decimal HourlyRate { get; set; }
        public Double HoursWorked { get; set; }
        /// <summary>
        /// Constructor. Hands parameters to "Employee"'s constructor.
        /// </summary>
        /// <param name="id">The Employees Identifiying Number</param>
        /// <param name="first">The Employees first name</param>
        /// <param name="last">The Employees last name</param>
        /// <param name="rate">The Employees hourly pay rate</param>
        /// <param name="hours">The Employees hours worked</param>
        public Hourly(uint id, string first, string last, Decimal rate, double hours)
            :base(id,ETYPE.HOURLY, first,last)
        {
            HourlyRate = rate;
            HoursWorked = hours;
        }
        /// <summary>
        /// Gives a string representation of the class
        /// </summary>
        /// <returns>A string representing the current state of the class</returns>
        public override string ToString()
        {
            return base.ToString() + RATE_LBL + HourlyRate + HOURS_LBL + HoursWorked + "\n";
        }
    }
}
