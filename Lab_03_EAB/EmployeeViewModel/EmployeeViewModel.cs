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

        private TextEmployee currentEmployee;
        public TextEmployee CurrentEmployee
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
        private bool canAdd;
        public bool CanAdd
        {
            get => canAdd;
            set
            {
                canAdd = value;
                OnPropertyChanged(nameof(CanAdd));
            }
        }
        private bool canAddCourse;
        public bool CanAddCourse
        {
            get => canAddCourse;
            set
            {
                canAddCourse = value;
                OnPropertyChanged(nameof(CanAddCourse));
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
            if (resultDestination.CanAddTextEmployee(CurrentEmployee) &&
                ((IsNew && !resultDestination.ContainsKey(CurrentEmployee.EmpID ?? uint.MaxValue)) || !IsNew))
            {
                resultDestination.AddFromEmployeeStrings(CurrentEmployee, Courses);
                CloseWindowFlag = false;
            }
            else
                MessageBox.Show(EMPLOYEE_BAD_MSG, EMPLOYEE_BAD_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnCurrentEmployeeChanged(object sender, PropertyChangedEventArgs args)
        {
            if (CurrentEmployee.Error != null) CanAdd = false;
            else CanAdd = true;
        }
        private void OnCourseChange(object sender, PropertyChangedEventArgs args)
        {
            if (CourseToAdd.Error != null) CanAddCourse = false;
            else CanAddCourse = true;
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
            CanAdd = false;
            CanAddCourse = false;
            resultDestination = rules;
            courseToAdd = new Course();
            courseToAdd.PropertyChanged += OnCourseChange;
            List<Course> list = employee?.CourseRoster?.Values?.ToList();
            if (list == null)
                Courses = new ObservableCollection<Course>();
            else
                Courses = new ObservableCollection<Course>(list);
            CurrentEmployee = new TextEmployee(employee);
            CurrentEmployee.PropertyChanged += OnCurrentEmployeeChanged;
            AddCourseCommand = new RelayCommand(AddCourse);
            AddModifyEmployeeCommand = new RelayCommand(AddModEmployee);
        }//end constructor
    }//end class
}//end namespace
