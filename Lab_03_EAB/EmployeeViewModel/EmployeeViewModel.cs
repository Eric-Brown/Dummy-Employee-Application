using Lab_03_EAB.EmployeeModel;
using Lab_03_EAB.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab_03_EAB.EmployeeViewModel
{
    class EmployeeViewModel : INotifyPropertyChanged
    {
        private const string COURSE_BAD_MSG = "Cannot add the course as specified.\nPlease ensure that the data entered is correct and that you have not already added a course of the same ID.";
        private const string COURSE_BAD_CAPTION = "Cannot Add Course";
        private const string WAGE_LABLE = "Contract Wage:";
        private const string GROSS_SALES_LABEL = "Gross Sales:";
        private const string COMMISSION_LABEL = "Commission:";
        private const string SALARY_LABEL = "Salary:";
        private const string HOURS_WORKED_LABEL = "Hours Worked:";
        private const string HOUR_RATE_LABEL = "Hourly Rate:";
        private const string EMPLOYEE_BAD_MSG = "Employee could not be added as specified.\nPlease ensure that all data entered is correct and that an employee of the same ID does not already exist.";
        private const string EMPLOYEE_BAD_CAPTION = "Could not add employee.";
        #region Event and Handler
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        #region Observable Properties
        private ObservableCollection<Course> courses;
        public ObservableCollection<Course> Courses
        {
            get => courses;
            set
            {
                courses = value;
                OnPropertyChanged(nameof(Courses));
            }
        }
        private Course courseToAdd;
        public Course CourseToAdd
        {
            get => courseToAdd;
            set
            {
                courseToAdd = value;
                OnPropertyChanged(nameof(CourseToAdd));
            }
        }

        private Employee currentEmployee;
        public Employee CurrentEmployee
        {
            get => currentEmployee;
            set
            {
                currentEmployee = value;
                OnPropertyChanged(nameof(CurrentEmployee));
            }
        }
        private bool isNew;
        public bool IsNew
        {
            get => isNew;
            set
            {
                isNew = value;
                OnPropertyChanged(nameof(IsNew));
            }
        }
        private string suppliment1;
        public string Suppliment1Lable
        {
            get => suppliment1;
            set
            {
                suppliment1 = value;
                OnPropertyChanged(nameof(Suppliment1Lable));
            }
        }
        private bool suppliment2Vis;
        public bool Suppliment2Visibility
        {
            get => suppliment2Vis;
            set
            {
                suppliment2Vis = value;
                OnPropertyChanged(nameof(Suppliment2Visibility));
            }
        }
        private string suppliment2;
        public string Suppliment2Lable
        {
            get => suppliment2;
            set
            {
                suppliment2 = value;
                OnPropertyChanged(nameof(Suppliment2Lable));
            }
        }
        private bool suppliment3Vis;
        public bool Suppliment3Visibility
        {
            get => suppliment3Vis;
            set
            {
                suppliment3Vis = value;
                OnPropertyChanged(nameof(Suppliment3Visibility));
            }
        }
        private string suppliment3;
        public string Suppliment3Lable
        {
            get => suppliment3;
            set
            {
                suppliment3 = value;
                OnPropertyChanged(nameof(Suppliment3Lable));
            }
        }
        private string buttonLable;
        public string AddButtonContent
        {
            get => buttonLable;
            set
            {
                buttonLable = value;
                OnPropertyChanged(nameof(AddButtonContent));
            }
        }
        private string courseCreditstoAdd;
        public string CourseCreditsToAdd
        {
            get => courseCreditstoAdd;
            set
            {
                courseCreditstoAdd = value;
                OnPropertyChanged(nameof(CourseCreditsToAdd));
            }
        }
        private bool? closeWindowFlag;
        public bool? CloseWindowFlag
        {
            get => closeWindowFlag;
            set
            {
                closeWindowFlag = value;
                OnPropertyChanged((nameof(CloseWindowFlag)));
            }
        }

        #endregion
        #region Commands
        public RelayCommand AddCourseCommand
        {
            get; set;
        }
        private void AddCourse(object parameter)
        {
            bool isGood;
            var query = Courses?.Where((a) => a.CourseID == courseToAdd.CourseID);
            int queryCount = query?.Count() ?? 0;
            isGood = !(queryCount > 0 || CourseToAdd.Error != null);
            if (!isGood)
            {
                MessageBox.Show(COURSE_BAD_MSG, COURSE_BAD_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Courses.Add(courseToAdd);
            CourseToAdd = new Course();
        }
        public RelayCommand AddModifyEmployeeCommand
        {
            get; set;
        }

        private void AddModEmployee(object parameter)
        {
            if (resultDestination[CurrentEmployee.EmpID] != null)
                resultDestination.Remove(resultDestination[CurrentEmployee.EmpID]);
            resultDestination.Add(currentEmployee);
            CloseWindowFlag = false;
        }
        private void EmployeeTypeChanged(object parameter, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == "EmpType")
            {
                switch (currentEmployee.EmpType)
                {
                    case ETYPE.CONTRACT:
                        Suppliment1Lable = WAGE_LABLE;
                        Suppliment2Visibility = false;
                        Suppliment3Visibility = false;
                        break;
                    case ETYPE.HOURLY:
                        Suppliment1Lable = HOUR_RATE_LABEL;
                        Suppliment2Lable = HOURS_WORKED_LABEL;
                        Suppliment2Visibility = true;
                        Suppliment3Visibility = false;
                        break;
                    case ETYPE.SALARY:
                        Suppliment1Lable = SALARY_LABEL;
                        Suppliment2Visibility = false;
                        Suppliment3Visibility = false;
                        break;
                    case ETYPE.SALES:
                        Suppliment1Lable = SALARY_LABEL;
                        Suppliment2Lable = COMMISSION_LABEL;
                        Suppliment3Lable = GROSS_SALES_LABEL;
                        Suppliment2Visibility = true;
                        Suppliment3Visibility = true;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
        private BusinessRules resultDestination;
        public EmployeeViewModel(BusinessRules rules)
            : this(null, rules)
        {

        }
        public EmployeeViewModel(Employee employee, BusinessRules rules)
        {
            AddButtonContent = employee == null ? "Add" : "Modify";
            IsNew = (employee == null) ? true : false;
            resultDestination = rules;
            courseToAdd = new Course();
            List<Course> list = employee?.CourseRoster?.Values?.ToList();
            if (list == null)
                Courses = new ObservableCollection<Course>();
            else
                Courses = new ObservableCollection<Course>(list);
            if (employee == null)
                CurrentEmployee = new Contract();
            else
                CurrentEmployee = employee;
            AddCourseCommand = new RelayCommand(AddCourse);
            AddModifyEmployeeCommand = new RelayCommand(AddModEmployee);
        }//end constructor
    }//end class
}//end namespace
