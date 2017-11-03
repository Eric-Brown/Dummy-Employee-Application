using System;
using System.Runtime.Serialization;

namespace Lab_03_EAB
{
    [DataContract]
    [Serializable]
    public sealed class Sales : Salary
    {
        [DataMember]
        private const string FORMAT_STRING = "Commission: {0}\nGross Sales: {1}\n",
            BAD_VAL_ERR_MSG = "Only non-negative values may be used to construct a Sales employee.";
        /// <summary>
        /// Property Commission and backing field. Negative values are rejected.
        /// </summary>
        [DataMember]
        private decimal commission;
        public Decimal Commission
        {
            get => commission;
            set
            {
                if (value >= 0)
                    commission = value;
            }
        }
        /// <summary>
        /// Property GrossSales and backing field. Negative values are rejected.
        /// </summary>
        [DataMember]
        private decimal grossSales;
        public Decimal GrossSales
        {
            get => grossSales;
            set
            {
                if (value >= 0)
                    grossSales = value;
            }
        }
        /// <summary>
        /// Constructor. Hands parameters to "Salary"'s constructor.
        /// </summary>
        /// <param name="id">The Employees Identifiying Number</param>
        /// <param name="first">The Employees first name</param>
        /// <param name="last">The Employees last name</param>
        /// <param name="salary">The Employees salary</param>
        /// <param name="commission">The Employees commission rate</param>
        /// <param name="sales">The Employees gross sales value</param>
        public Sales(uint id, string first, string last, Decimal salary, Decimal commission, Decimal sales)
            : base(id, first, last, salary, ETYPE.SALES)
        {
            if (commission < 0)
                throw new ArgumentException(BAD_VAL_ERR_MSG, commission.GetType().Name);
            if (sales < 0)
                throw new ArgumentException(BAD_VAL_ERR_MSG, sales.GetType().Name);
            Commission = commission;
            GrossSales = sales;
        }
        /// <summary>
        /// Gives a string representation of the class
        /// </summary>
        /// <returns>A string representing the current state of the class</returns>
        public override string ToString()
        {
            return base.ToString() + string.Format(FORMAT_STRING, commission, GrossSales);
        }
    }
}
