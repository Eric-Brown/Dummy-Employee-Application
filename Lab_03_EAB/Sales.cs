using System;


namespace Lab_03_EAB
{
    sealed class Sales : Salary
    {
        private const string COM_LBL = "Commission: ",
                SALES_LBL = "\nGross Sales: ";
        public Decimal Commission { get; set; }
        public Decimal GrossSales { get; set; }
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
            Commission = commission;
            GrossSales = sales;
        }
        /// <summary>
        /// Gives a string representation of the class
        /// </summary>
        /// <returns>A string representing the current state of the class</returns>
        public override string ToString()
        {
            return base.ToString() + COM_LBL + Commission + SALES_LBL + GrossSales + "\n";
        }
    }
}
