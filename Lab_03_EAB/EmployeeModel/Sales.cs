using System;
using System.Runtime.Serialization;
using Lab_03_EAB.Helpers;
using Lab_03_EAB.EmployeeModel;

namespace Lab_03_EAB
{
    [DataContract]
    [Serializable]
    public sealed class Sales : Salary
    {
        [DataMember]
        private const string FORMAT_STRING = "Commission: {0}\nGross Sales: {1}\n",
            BAD_VAL_ERR_MSG = "Only non-negative values may be used to construct a Sales employee.";
        private const string COMM_ERR_MSG = "Commission must be a positive value.";
        private const string GROSS_ERR_MSG = "Gross Sales must be a positive value.";

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
        public Sales(TextEmployee employee)
            :base(employee, ETYPE.SALES)
        {
            try
            {
                Commission = decimal.Parse(employee.Suppliment2);
                GrossSales = decimal.Parse(employee.Suppliment3);
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Gives a string representation of the class
        /// </summary>
        /// <returns>A string representing the current state of the class</returns>
        public override string ToString()
        {
            return base.ToString() + string.Format(FORMAT_STRING, commission, GrossSales);
        }
        public override string this[string columnName]
        {
            get
            {
                string result = null;
                switch(columnName)
                {
                    case nameof(Commission):
                        if (Commission < 0)
                            result = COMM_ERR_MSG;
                        break;
                    case nameof(GrossSales):
                        if (GrossSales < 0)
                            result = GROSS_ERR_MSG;
                        break;
                    default:
                        return base[columnName];
                }
                return result;
            }
        }
        public static new bool IsValidTextEmployee(TextEmployee employee)
        {
            bool toReturn = false;
            try
            {
                toReturn = Salary.IsValidTextEmployee(employee) && IS_POS_NUM.IsMatch(employee?.Suppliment2) && IS_POS_NUM.IsMatch(employee?.Suppliment3) && employee.EmpType == ETYPE.SALES;
            }
            catch(Exception)
            {
                return false;
            }
            return toReturn;
        }
    }
}
