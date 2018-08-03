using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Lab_03_EAB.EmployeeModel
{


    [DataContract]
    public class Course : INotifyPropertyChanged, IDataErrorInfo
    {
        [DataContract]
        [Serializable]
        ///Represents the possible recieved grades in a course.
        public enum COURSE_GRADE
        {
            [EnumMember]
            A = 100,

            [EnumMember]
            A_MINUS = 94,

            [EnumMember]
            B_PLUS = 90,

            [EnumMember]
            B = 86,

            [EnumMember]
            B_MINUS = 83,

            [EnumMember]
            C_PLUS = 80,

            [EnumMember]
            C = 76,

            [EnumMember]
            C_MINUS = 73,

            [EnumMember]
            D_PLUS = 70,

            [EnumMember]
            D = 66,

            [EnumMember]
            D_MINUS = 63,

            [EnumMember]
            E = 60
        }
        [DataMember]
        private const string FORMAT_STRING = "\tCourse ID: {0}\n\tCourse Description: {1}\n\tCourse Grade: {2}\n\tCourse Credits: {3}\n\tApproved Date: {4:d}";

        private const string DATE_BAD_ERR_MSG = "The course must have been approved within the current century.";
        private const string BAD_CRED_ERR_MSG = "Credits must be between 1 and 5.";
        private const string BAD_DESC_ERR_MSG = "Course must have a description.";
        private const string BAD_ID_ERR_MSG = "Course must have an ID.";

        [DataMember]
        protected readonly DateTime TOO_EARLY = new DateTime(1917, 1, 1);

        protected readonly DateTime TOO_LATE = new DateTime(2117, 1, 1);

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
        private DateTime date = new DateTime(2015, 11, 10);

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

        public string Error
        {
            get
            {
                StringBuilder errors = new StringBuilder();
                errors.Append(this[nameof(ApprovedDate)]);
                errors.Append(this[nameof(CourseID)]);
                errors.Append(this[nameof(CourseDescription)]);
                errors.Append(this[nameof(Credits)]);
                if (errors.Length == 0) return null;
                return errors.ToString();
            }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case nameof(ApprovedDate):
                        if (ApprovedDate <= TOO_EARLY || ApprovedDate >= TOO_LATE)
                            result = DATE_BAD_ERR_MSG;
                        break;

                    case nameof(CourseID):
                        if (string.IsNullOrEmpty(CourseID))
                            result = BAD_ID_ERR_MSG;
                        break;

                    case nameof(CourseDescription):
                        if (string.IsNullOrEmpty(CourseDescription))
                            result = BAD_DESC_ERR_MSG;
                        break;

                    case nameof(Credits):
                        if (Credits < 1 || Credits > 5)
                            result = BAD_CRED_ERR_MSG;
                        break;
                }
                return result;
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
}