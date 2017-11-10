using System.Runtime.Serialization;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Lab_03_EAB.Helpers;
using Lab_03_EAB.EmployeeModel;
using System.Text.RegularExpressions;

namespace Lab_03_EAB
{
    /// <summary>
    /// Enum that represents the known subtypes of an employee.
    /// <warning>Other code in the project relies on these being default integral values.</warning>
    /// </summary>
    [DataContract]
    [Serializable]
    public enum ETYPE
    {
        [EnumMember]
        SALARY,
        [EnumMember]
        SALES,
        [EnumMember]
        HOURLY,
        [EnumMember]
        CONTRACT
    };
    /// <summary>
    /// Abstract class which contains all the information and behaviors which are in common for all employees.
    /// </summary>
    [DataContract, KnownType(typeof(Contract)), KnownType(typeof(Salary)), KnownType(typeof(Sales)), KnownType(typeof(Hourly))]
    [Serializable]
    public abstract class Employee : IDataErrorInfo, INotifyPropertyChanged
    {
        #region Constants
        /// <summary>
        /// Used to call string.Format without having to use several "magic" strings.
        /// </summary>
        [DataMember]
        private const string FORMAT_STRING = "EmpID: {0}\nEmpType: {1}\nFirst Name: {2}\nLast Name: {3}\n";
        [DataMember]
        private const int SALARY_MIN_CREDITS = 6;
        [DataMember]
        private const int SALES_MIN_CREDITS = 3;
        [DataMember]
        private const int HOURLY_MIN_CREDITS = 1;
        [DataMember]
        protected const string IS_ALPHA_PTN = @"(?i)(?!.*[\d]+.*)^.*";
        protected const string IS_POS_NUM_PTN = @"^\d*\.?\d*$";
        protected const string IS_POS_INT_PTN = @"^\d+$";
        static protected readonly Regex IS_ALPHA = new Regex(IS_ALPHA_PTN, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static protected readonly Regex IS_POS_NUM = new Regex(IS_POS_NUM_PTN, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static protected readonly Regex IS_POS_INT = new Regex(IS_POS_INT_PTN, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const string NAME_BAD_ERR_MSG = "Please ensure that the name contains no numbers and is not empty.";
        private const string ID_BAD_ERR_MSG = "Please ensure that the Employee's ID is only a number and contains no letters.";
        #endregion
        #region EventsAndHandlers
        /// <summary>
        /// Event delegate for when the employee's ID has been changed.
        /// </summary>
        public event System.EventHandler<PropertyChangeEventArgs<uint>> EmpIDChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Broadcasts to any listeners that the employee ID has changed.
        /// </summary>
        /// <param name="e">EventArgs subtype that contains old and new value of changed property</param>
        protected virtual void OnEmpIDChanged(PropertyChangeEventArgs<uint> e)
        {
            EmpIDChanged?.Invoke(this, e);
        }
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        #region Properties
        [DataMember]
        private uint empID;
        /// <summary>
        /// Represents the unique ID of the employee.
        /// </summary>
        public uint EmpID
        {
            get => empID;
            set
            {
                if (value == empID)
                    return;
                uint oldValue = empID;
                empID = value;
                OnEmpIDChanged(new PropertyChangeEventArgs<uint>(oldValue, empID));
                OnPropertyChanged(nameof(EmpID));
            }
        }
        [DataMember]
        private string firstName;
        /// <summary>
        /// Represents the first name of the employee. Rejects null or empty values.
        /// </summary>
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        [DataMember]
        private string lastName;
        /// <summary>
        /// Represents the last name of the employee. Rejects null or empty values.
        /// </summary>
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        [DataMember]
        private ETYPE empType;
        /// <summary>
        /// Represents the type of the employee.
        /// </summary>
        public ETYPE EmpType
        {
            get => empType;
            private set
            {
                empType = value;
                OnPropertyChanged(nameof(EmpType));
            }
        }

        public bool Overtime
        {
            get
            {
                if (empType == ETYPE.HOURLY) return true;
                else return false;
            }
        }
        public bool Benefits
        {
            get
            {
                if (empType == ETYPE.SALARY || empType == ETYPE.SALES) return true;
                else return false;
            }
        }
        public bool HasCommission
        {
            get
            {
                if (empType == ETYPE.SALES) return true;
                else return false;
            }
        }
        [DataMember]
        private SortedDictionary<string, Course> roster;
        /// <summary>
        /// Represents the assigned courses for the employee for the semester.
        /// </summary>
        public SortedDictionary<string, Course> CourseRoster
        {
            get => roster;
            set
            {
                roster = value;
                OnPropertyChanged(nameof(CourseRoster));
            }
        }
        /// <summary>
        /// Returns whether or not an employee is elegible for Educational Benefits
        /// </summary>
        /// <Note>Contract is not eligible
        /// Salary requires a minimum of 6 credit hours and a grade minimum of B+
        /// Hourly requires a minimum of 1 credit hour and a grade minimum of B
        /// Sales requires a minimum of 3 credit hours and a grade miniumum of C+</Note>
        public bool EducationalBenefits
        {
            get
            {
                switch(empType)
                {
                    case ETYPE.SALARY:
                        return MeetsRequirements(COURSE_GRADE.B_PLUS, SALARY_MIN_CREDITS);
                    case ETYPE.SALES:
                        return MeetsRequirements(COURSE_GRADE.B, SALES_MIN_CREDITS);
                    case ETYPE.HOURLY:
                        return MeetsRequirements(COURSE_GRADE.C_PLUS, HOURLY_MIN_CREDITS);
                    default:
                        return false;
                }
            }
        }

        public string Error => null;

        public virtual string this[string columnName]
        {
            get
            {
                string result = null;
                switch(columnName)
                {
                    case nameof(EmpID):
                        break;
                    case nameof(EmpType):
                        break;
                    case nameof(FirstName):
                        if (string.IsNullOrEmpty(firstName) || !IS_ALPHA.IsMatch(firstName))
                            result = NAME_BAD_ERR_MSG;
                        break;
                    case nameof(LastName):
                        if (string.IsNullOrEmpty(lastName) || !IS_ALPHA.IsMatch(lastName))
                            result = NAME_BAD_ERR_MSG;
                        break;
                    case nameof(CourseRoster):
                        break;
                    default:
                        break;
                }
                return result;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor.
        /// Is protected since only subtypes of this class should call it.
        /// </summary>
        /// <param name="type">The type of the employee.</param>
        /// <param name="id">The Employees Identifiying Number</param>
        /// <param name="first">The Employees first name</param>
        /// <param name="last">The Employees last name</param>
        protected Employee(uint id, ETYPE type, string first, string last)
        {
            empID = id;
            empType = type;
            firstName = first;
            lastName = last;
            CourseRoster = new SortedDictionary<string, Course>();
        }
        /// <summary>
        /// Creates a new employee object based off of the information in TextEmployee.
        /// </summary>
        /// <param name="textEmployee">The text information of the employee to be created.</param>
        /// <param name="type">The type of the employee that is being constructed.</param>
        /// <exception cref="ArgumentNullException">Thrown when TextEmployee is null.</exception>
        protected Employee(TextEmployee textEmployee, ETYPE type)
        {
            if (textEmployee == null) throw new ArgumentNullException(paramName: nameof(textEmployee));
            EmpID = textEmployee.EmpID ?? uint.MaxValue;
            FirstName = textEmployee.FirstName;
            LastName = textEmployee.LastName;
            CourseRoster = new SortedDictionary<string, Course>();
        }
        #endregion
        #region Methods
        private bool MeetsRequirements(COURSE_GRADE minGrade, int minCreds)
        {
            int totalGrade= 0;
            totalGrade = CourseRoster.Values.Aggregate(totalGrade, (a, b) => a + (int)b.Grade);
            int totalCredits = 0;
            totalCredits = CourseRoster.Values.Aggregate(totalCredits, (a, b) => a + b.Credits);
            double gradeAverage = System.Math.Round((totalGrade / (double)CourseRoster.Count()));
            return (gradeAverage >= (int)minGrade && totalCredits >= minCreds);
        }
        /// <summary>
        /// Gives a string representation of the class
        /// </summary>
        /// <returns>A string representing the current state of the class</returns>
        public override string ToString()
        {
            StringBuilder toReturn = new StringBuilder();
            toReturn.AppendLine(string.Format(FORMAT_STRING, EmpID, EmpType, FirstName, LastName));
            return toReturn.ToString();
        }
        public string CourseListing()
        {
            StringBuilder toReturn = new StringBuilder();
            foreach (Course value in CourseRoster?.Values)
            {
                toReturn.AppendLine(value.ToString() + "\n");
            }
            return toReturn.ToString();
        }
        public static bool IsValidTextEmployee(TextEmployee toTest)
        {
            if (toTest == null) return false;
            bool result = false;
            try
            {
                result = (IS_ALPHA.IsMatch(toTest.FirstName) && IS_ALPHA.IsMatch(toTest.LastName) && toTest.EmpID != null);
            }
            catch(Exception)
            {
                return false;
            }
            return result;
        }
        [OnDeserialized]
        private void DeserializedFix(StreamingContext context)
        {
            if (roster == null)
                roster = new SortedDictionary<string, Course>();
        }

        #endregion
    }//End Class Employee Definition
}//End Namespace Lab_03_EAB Scope
