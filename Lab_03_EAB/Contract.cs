using System;


namespace Lab_03_EAB
{
    sealed class Contract : Employee
    {
        private const string WAGE_LBL = "Contract Wage: ";
        public Decimal ContractWage { get; set; }
        /// <summary>
        /// Constructor. Hands parameters to "Employee"'s constructor.
        /// </summary>
        /// <param name="id">The Employees Identifiying Number</param>
        /// <param name="first">The Employees first name</param>
        /// <param name="last">The Employees last name</param>
        /// <param name="wage">The Employees contracted wage</param>
        public Contract(uint id, string first, string last, Decimal wage)
            :base(id, ETYPE.CONTRACT , first,last)
        {
            ContractWage = wage;
        }
        /// <summary>
        /// Gives a string representation of the class
        /// </summary>
        /// <returns>A string representing the current state of the class</returns>
        public override string ToString()
        {
            return base.ToString() + WAGE_LBL + ContractWage + "\n";
        }
    }
}
