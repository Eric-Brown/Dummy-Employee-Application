using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Lab_03_EAB
{
    [DataContract]
    [Serializable]
    public sealed class Hourly : Employee
    {
        
        [DataMember]
        private const string HOURLY_FORMAT_STRING = "Hourly Rate: {0}\nHours Worked: {1}\n",
            BAD_VAL_ERR_MSG = "Only non-negative values may be used to construct a Hourly employee.";
        /// <summary>
        /// HourlyRate property and backing field. Negative values are rejected.
        /// </summary>
        [DataMember]
        private decimal hourlyRate;
        public Decimal HourlyRate
        {
            get => hourlyRate;
            set
            {
                if (value >= 0)
                    hourlyRate = value;
            }
        }
        /// <summary>
        /// HoursWorked property and backing field. Negative values are rejected.
        /// </summary>
        [DataMember]
        private double hoursWorked;
        public Double HoursWorked
        {
            get => hoursWorked;
            set
            {
                if (value >= 0)
                    hoursWorked = value;
            }
        }
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
            if (rate < 0)
                throw new ArgumentException(BAD_VAL_ERR_MSG, rate.GetType().Name);
            if (hours < 0)
                throw new ArgumentException(BAD_VAL_ERR_MSG, hours.GetType().Name);
            HourlyRate = rate;
            HoursWorked = hours;
        }
        /// <summary>
        /// Gives a string representation of the class
        /// </summary>
        /// <returns>A string representing the current state of the class</returns>
        public override string ToString()
        {
            return base.ToString() + string.Format(HOURLY_FORMAT_STRING, HourlyRate, HoursWorked)+ CourseListing();
        }
    }
}
