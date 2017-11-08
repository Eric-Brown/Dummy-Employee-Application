using System;
using System.Runtime.Serialization;
using Lab_03_EAB.Helpers;
using System.ComponentModel;
using Lab_03_EAB.EmployeeModel;

namespace Lab_03_EAB
{
    [DataContract]
    [Serializable]
    public sealed class Contract : Employee
    {
        [DataMember]
        private const string CONTRACT_FORMAT_STRING = "Contract Wage: {0}\n",
            BAD_WAGE_CONSTR = "Only non-negative values of wage may be used to construct a Contract employee.";
        private const string BAD_WAG_ERROR_MSG = "Please ensure that the Contract Wage is not empty and that it is a positive number.";

        /// <summary>
        /// ContractWage property and backing field. Negative values are rejected.
        /// </summary>
        [DataMember]
        private decimal contractWage;
        public Decimal ContractWage
        {
            get => contractWage;
            set
            {
                    contractWage = value;
            }
        }
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
            if (wage < 0) throw new ArgumentException(BAD_WAGE_CONSTR);
            ContractWage = wage;
        }
        public Contract(TextEmployee employee)
            :base(employee, ETYPE.CONTRACT)
        {
            try
            {
                ContractWage = decimal.Parse(employee.Suppliment1);
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message, innerException: ex);
            }
        }
        /// <summary>
        /// Gives a string representation of the class
        /// </summary>
        /// <returns>A string representing the current state of the class</returns>
        public override string ToString()
        {
            return base.ToString() + string.Format(CONTRACT_FORMAT_STRING, ContractWage) +"\n" + base.CourseListing();
        }

        public override string this[string columnname]
        {
            get
            {
                string result = null;
                switch(columnname)
                {
                    case nameof(ContractWage):
                        if (contractWage < 0)
                            result = BAD_WAG_ERROR_MSG;
                        break;
                    default:
                        return base[columnname];
                }
                return result;
            }
        }
        public static new bool IsValidTextEmployee(TextEmployee toTest)
        {
            bool toReturn = false;
            try
            {
                toReturn = Employee.IsValidTextEmployee(toTest) && IS_POS_NUM.IsMatch(toTest?.Suppliment1) && toTest.EmpType == ETYPE.CONTRACT;
            }
            catch (Exception)
            {
                return false;
            }
            return toReturn;
        }
    }
}
