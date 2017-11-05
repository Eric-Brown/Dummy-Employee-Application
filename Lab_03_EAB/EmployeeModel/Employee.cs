using System.Runtime.Serialization;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

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
    [DataContract]
    [Serializable]
    ///Represents the possible recieved grades in a course.
    public enum COURSE_GRADE
    {
        [EnumMember]
        A,
        [EnumMember]
        A_MINUS,
        [EnumMember]
        B_PLUS,
        [EnumMember]
        B,
        [EnumMember]
        B_MINUS,
        [EnumMember]
        C_PLUS,
        [EnumMember]
        C,
        [EnumMember]
        C_MINUS,
        [EnumMember]
        D_PLUS,
        [EnumMember]
        D,
        [EnumMember]
        D_MINUS,
        [EnumMember]
        E
    }
    [DataContract]
    public class Course :INotifyPropertyChanged
    {
        [DataMember]
        private const string FORMAT_STRING = "\tCourse ID: {0}\n\tCourse Description: {1}\n\tCourse Grade: {2}\n\tCourse Credits: {3}\n\tApproved Date: {4:d}";
        [DataMember]
        private string cID = "";
        public string CourseID
        {
            get => cID;
            set
            {
                cID = value;
                OnPropertyChanged(nameof(CourseID));
            }
        }
        [DataMember]
        private string cDesc = "";
        public string CourseDescription
        {
            get => cDesc;
            set
            {
                cDesc = value;
                OnPropertyChanged(nameof(CourseDescription));
            }
        }
        [DataMember]
        private COURSE_GRADE grd = COURSE_GRADE.A;
        public COURSE_GRADE Grade
        {
            get => grd;
            set
            {
                grd = value;
                OnPropertyChanged(nameof(Grade));
            }
        }
        [DataMember]
        private DateTime date = new DateTime(2015,11,10);
        public DateTime ApprovedDate
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged(nameof(ApprovedDate));
            }
        }
        [DataMember]
        private int cred = 0;
        public int Credits
        {
            get => cred;
            set
            {
                cred = value;
                OnPropertyChanged(nameof(Credits));
            }
        }
        public Course()
        {
        }
        public Course(string ID, string Description, COURSE_GRADE gRADE, DateTime date, int creds)
        {
            CourseID = ID;
            CourseDescription = Description;
            Grade = gRADE;
            ApprovedDate = date;
            Credits = creds;
        }
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return string.Format(FORMAT_STRING, CourseID, CourseDescription, Grade, Credits, ApprovedDate);
        }
    }
    /// <summary>
    /// Abstract class which contains all the information and behaviors which are in common for all employees.
    /// </summary>
    [DataContract, KnownType(typeof(Contract)), KnownType(typeof(Salary)), KnownType(typeof(Sales)), KnownType(typeof(Hourly))]
    [Serializable]
    public abstract class Employee
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
        #endregion
        #region EventsAndHandlers
        /// <summary>
        /// Event delegate for when the employee's ID has been changed.
        /// </summary>
        public event System.EventHandler<PropertyChangeEventArgs<uint>> EmpIDChanged;
        /// <summary>
        /// Broadcasts to any listeners that the employee ID has changed.
        /// </summary>
        /// <param name="e">EventArgs subtype that contains old and new value of changed property</param>
        protected virtual void OnEmpIDChanged(PropertyChangeEventArgs<uint> e)
        {
            EmpIDChanged?.Invoke(this, e);
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
                if (!string.IsNullOrEmpty(value))
                    firstName = value;
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
                if (!string.IsNullOrEmpty(value))
                    lastName = value;
            }
        }
        [DataMember]
        private ETYPE empType;
        /// <summary>
        /// Represents the type of the employee.
        /// </summary>
        public ETYPE EmpType { get => empType; private set => empType = value; }

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
        #endregion
        #region Methods
        private bool MeetsRequirements(COURSE_GRADE minGrade, int minCreds)
        {
            int totalGrade= 0;
            totalGrade = CourseRoster.Values.Aggregate(totalGrade, (a, b) => a + (int)b.Grade);
            int totalCredits = 0;
            totalCredits = CourseRoster.Values.Aggregate(totalCredits, (a, b) => a + b.Credits);
            COURSE_GRADE gradeAverage = (COURSE_GRADE)System.Math.Round((totalGrade / (double)CourseRoster.Count()));
            //The better grades will have a lower value
            return (gradeAverage <= minGrade && totalGrade >= minCreds);
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
                toReturn.AppendLine(value.ToString());
            }
            return toReturn.ToString();
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
