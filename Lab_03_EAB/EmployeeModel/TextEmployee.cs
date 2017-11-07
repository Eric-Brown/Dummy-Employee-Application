using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab_03_EAB.EmployeeModel
{
    public class TextEmployee : INotifyPropertyChanged, IDataErrorInfo
    {
        private const string WAGE_LABLE = "Contract Wage:";
        private const string GROSS_SALES_LABEL = "Gross Sales:";
        private const string COMMISSION_LABEL = "Commission:";
        private const string SALARY_LABEL = "Salary:";
        private const string HOURS_WORKED_LABEL = "Hours Worked:";
        private const string HOUR_RATE_LABEL = "Hourly Rate:";

        protected const string IS_ALPHA_PTN = @"(?i)(?!.*[\d]+.*)^.*";
        protected const string IS_POS_NUM_PTN = @"^\d*\.?\d*$";
        protected const string IS_POS_INT_PTN = @"^\d+$";
        protected readonly Regex IS_ALPHA = new Regex(IS_ALPHA_PTN, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        protected readonly Regex IS_POS_NUM = new Regex(IS_POS_NUM_PTN, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        protected readonly Regex IS_POS_INT = new Regex(IS_POS_INT_PTN, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        protected readonly Regex IS_NUMERIC = new Regex(@"^\s*-?\d*\.?\d*\s*$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        uint? id;
        public uint? EmpID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(EmpID));
            }
        }
        string first;
        public string FirstName
        {
            get => first;
            set
            {
                first = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        string last;
        public string LastName
        {
            get => last;
            set
            {
                last = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        ETYPE type;
        public ETYPE EmpType
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(EmpType));
                switch (EmpType)
                {
                    case ETYPE.CONTRACT:
                        Sup1Lable = WAGE_LABLE;
                        Sup2Visibility = false;
                        Sup3Visibility = false;
                        break;
                    case ETYPE.HOURLY:
                        Sup1Lable = HOUR_RATE_LABEL;
                        Sup2Lable = HOURS_WORKED_LABEL;
                        Sup2Visibility = true;
                        Sup3Visibility = false;
                        break;
                    case ETYPE.SALARY:
                        Sup1Lable = SALARY_LABEL;
                        Sup2Visibility = false;
                        Sup3Visibility = false;
                        break;
                    case ETYPE.SALES:
                        Sup1Lable = SALARY_LABEL;
                        Sup2Lable = COMMISSION_LABEL;
                        Sup3Lable = GROSS_SALES_LABEL;
                        Sup2Visibility = true;
                        Sup3Visibility = true;
                        break;
                    default:
                        break;
                }
            }
        }
        string sup1;
        public string Suppliment1
        {
            get => sup1;
            set
            {
                sup1 = value;
                OnPropertyChanged(nameof(Suppliment1));
            }
        }
        private string sup1Lab;
        public string Sup1Lable
        {
            get => sup1Lab;
            set
            {
                sup1Lab = value;
                OnPropertyChanged(nameof(Sup1Lable));
            }
        }
        string sup2;
        public string Suppliment2
        {
            get => sup2;
            set
            {
                sup2 = value;
                OnPropertyChanged(nameof(Suppliment2));
            }
        }
        private bool sup2Vis;
        public bool Sup2Visibility
        {
            get => sup2Vis;
            set
            {
                sup2Vis = value;
                OnPropertyChanged(nameof(Sup2Visibility));
            }
        }
        private string sup2Lab;
        public string Sup2Lable
        {
            get => sup2Lab;
            set
            {
                sup2Lab = value;
                OnPropertyChanged(nameof(Sup2Lable));
            }
        }
        string sup3;
        public string Suppliment3
        {
            get => sup3;
            set
            {
                sup3 = value;
                OnPropertyChanged(nameof(Suppliment3));
            }
        }
        private bool sup3Vis;
        public bool Sup3Visibility
        {
            get => sup3Vis;
            set
            {
                sup3Vis = value;
                OnPropertyChanged(nameof(Sup3Visibility));
            }
        }
        private string sup3Lab;
        public string Sup3Lable
        {
            get => sup3Lab;
            set
            {
                sup3Lab = value;
                OnPropertyChanged(nameof(Sup3Lable));
            }
        }
        public TextEmployee()
            :this(null)
        {

        }
        public TextEmployee(Employee emp)
        {
            EmpType = (emp == null) ? ETYPE.CONTRACT : emp.EmpType;
            EmpID = emp?.EmpID;
            FirstName = emp?.FirstName;
            LastName = emp?.LastName;
            switch(EmpType)
            {
                case ETYPE.CONTRACT:
                    Contract conEmp = emp as Contract;
                    Suppliment1 = conEmp?.ContractWage.ToString();
                    break;
                case ETYPE.HOURLY:
                    Hourly hourEmp = emp as Hourly;
                    Suppliment1 = hourEmp?.HourlyRate.ToString();
                    Suppliment2 = hourEmp?.HoursWorked.ToString();
                    break;
                case ETYPE.SALARY:
                    Salary salEmp = emp as Salary;
                    Suppliment1 = salEmp?.MonthlySalary.ToString();
                    break;
                case ETYPE.SALES:
                    Sales sEmp = emp as Sales;
                    Suppliment1 = sEmp?.MonthlySalary.ToString();
                    Suppliment2 = sEmp?.Commission.ToString();
                    Suppliment3 = sEmp?.GrossSales.ToString();
                    break;
                default:
                    break;
            }
        }
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch(columnName)
                {
                    case nameof(EmpID):
                        if (EmpID == null)
                            result = "An ID is required to create an employee.";
                        break;
                    case nameof(EmpType):
                        break;
                    case nameof(FirstName):
                        if (string.IsNullOrEmpty(FirstName))
                            result = "A name is required to create an employee.";
                        else if (!IS_ALPHA.IsMatch(FirstName))
                            result = "Names cannot contain numbers.";
                        break;
                    case nameof(LastName):
                        if (string.IsNullOrEmpty(LastName))
                            result = "A last name is required to create an employee.";
                        else if (!IS_ALPHA.IsMatch(LastName))
                            result = "Names cannot contain numbers.";
                        break;
                    case nameof(Suppliment1):
                        if (string.IsNullOrEmpty(Suppliment1))
                            result = "A value must be entered here.";
                        else if (!IS_NUMERIC.IsMatch(Suppliment1))
                            result = "Value must be a number.";
                        else if (!IS_POS_NUM.IsMatch(Suppliment1))
                            result = "Number must be non-negative.";
                        break;
                    case nameof(Suppliment2):
                        switch (EmpType)
                        {
                            case ETYPE.CONTRACT:
                                break;
                            case ETYPE.HOURLY:
                                if (string.IsNullOrEmpty(Suppliment2))
                                    result = "A value must be entered here.";
                                else if (!IS_NUMERIC.IsMatch(Suppliment2))
                                    result = "Value must be a number.";
                                else if (!IS_POS_NUM.IsMatch(Suppliment2))
                                    result = "Number must be non-negative.";
                                break;
                            case ETYPE.SALARY:
                                break;
                            case ETYPE.SALES:
                                if (string.IsNullOrEmpty(Suppliment2))
                                    result = "A value must be entered here.";
                                else if (!IS_NUMERIC.IsMatch(Suppliment2))
                                    result = "Value must be a number.";
                                else if (!IS_POS_NUM.IsMatch(Suppliment2))
                                    result = "Number must be non-negative.";
                                break;
                        }
                        break;
                    case nameof(Suppliment3):
                        switch (EmpType)
                        {
                            case ETYPE.CONTRACT:
                                break;
                            case ETYPE.HOURLY:
                                break;
                            case ETYPE.SALARY:
                                break;
                            case ETYPE.SALES:
                                if (string.IsNullOrEmpty(Suppliment3))
                                    result = "A value must be entered here.";
                                else if (!IS_NUMERIC.IsMatch(Suppliment3))
                                    result = "Value must be a number.";
                                else if (!IS_POS_NUM.IsMatch(Suppliment3))
                                    result = "Number must be non-negative.";
                                break;
                        }
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

        public string Error
        {
            get
            {
                StringBuilder toReturn = new StringBuilder();
                toReturn.Append(this[nameof(EmpID)]);
                toReturn.Append(this[nameof(EmpType)]);
                toReturn.Append(this[nameof(FirstName)]);
                toReturn.Append(this[nameof(LastName)]);
                toReturn.Append(this[nameof(Suppliment1)]);
                toReturn.Append(this[nameof(Suppliment2)]);
                toReturn.Append(this[nameof(Suppliment3)]);
                if (toReturn.Length == 0) return null;
                return toReturn.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
