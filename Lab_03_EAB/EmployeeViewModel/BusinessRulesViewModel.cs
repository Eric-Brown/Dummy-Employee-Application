using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lab_03_EAB.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Lab_03_EAB.EmployeeViewModel
{
    class BusinessRulesViewModel : INotifyPropertyChanged
    {
        #region Constants
        private const string
            ERROR_CAPTION = "Error",
            NEW_EMP_LINE = "-------New Employee-------\n",
            INVALID_EMP_TYPE_ERROR = "Invalid Employee Type",
            ALL_EMP_LINE = "\n------All Employees-----";
        /// <summary>
        /// Used to create random first names for employees.
        /// </summary>
        private static readonly string[] FIRST_NAMES = { "Stewart", "Sunny", "Grant", "Greg", "Micheal", "Seth", "Anthony", "Matthew", "Jonathon", "Jenny", "Sam" };
        /// <summary>
        /// Used to create random last names for employees.
        /// </summary>
        private static readonly string[] LAST_NAMES = { "Linder", "Brown", "DePoirot", "Johnson", "Williams", "Xavier", "Green", "Goldberg", "Greenburg", "Flotsam", "Jenkins", "Jensen", "Null" };
        /// <summary>
        /// Default number of random employees to create when creating test employees.
        /// </summary>
        private const int DEFAULT_NUM_TEST_EMPS = 10;
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;

        #region Data Properties
        private Employee selectedEmployee;
        public Employee SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = (Employee)value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedEmployee)));
            }
        }

        private void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            PropertyChanged?.Invoke(this, propertyChangedEventArgs);
        }

        public int Index {
            get;
            set; 
}
        private BusinessRules employees;
        public BusinessRules Employees
        {
            get => employees;
            set
            {
                employees = (BusinessRules)value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Employees)));
            }
        }
        private bool? closeWindowFlag;
        public bool? CloseWindowFlag
        {
            get => closeWindowFlag;
            set
            {
                closeWindowFlag = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(CloseWindowFlag)));
            }
        }
        private ObservableCollection<BusinessRules> employeesCollections;
        public ObservableCollection<BusinessRules> EmployeesCollections
        {
            get => employeesCollections;
            set
            {
                employeesCollections = (ObservableCollection<BusinessRules>)value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(EmployeesCollections)));
            }
        }
        #endregion
        #region Command Properties
        public RelayCommand AddEmployeeCommand
        {
            get; set;
        }

        public RelayCommand RemoveEmployeeCommand
        {
            get; set;
        }

        public RelayCommand ChangeEmployeeCommand
        {
            get; set;
        }

        public RelayCommand SaveFileCommand
        {
            get; set;
        }

        public RelayCommand OpenFileCommand
        {
            get; set;
        }

        public RelayCommand NewFileCommand
        {
            get; set;
        }

        public RelayCommand CreateTestEmployeesCommand
        {
            get; set;
        }
        #endregion

        public BusinessRulesViewModel()
        {
            EmployeesCollections = new ObservableCollection<BusinessRules>();
            CreateTestEmployees(null);
            Employees = EmployeesCollections.First<BusinessRules>();
            //Set up commands next
            CreateTestEmployeesCommand = new RelayCommand(CreateTestEmployees);
            SaveFileCommand = new RelayCommand(SaveFile);
            NewFileCommand = new RelayCommand(NewFile);
            OpenFileCommand = new RelayCommand(OpenFile);
        }
        private void AddEmployee()
        {
            throw new System.NotImplementedException();
        }

        private void ChangeEmployee()
        {
            throw new System.NotImplementedException();
        }

        private void NewFile(object parameter)
        {
            EmployeesCollections.Add(new BusinessRules());
        }

        private void OpenFile(object parameter)
        {
            BusinessRules toAdd;
            try
            {
                using (FileIO theFile = new FileIO(toAdd = new BusinessRules()))
                {
                    theFile.OpenFileDB();
                    theFile.ReadFileDB();
                    EmployeesCollections.Add(toAdd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveFile(object parameter)
        {
            try
            {
                using (FileIO thefile = new FileIO(Employees))
                {
                    thefile.OpenSaveFileDB();
                    thefile.WriteFileDB();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveEmployee()
        {
            employees.Remove(SelectedEmployee as Employee);
        }

        private void CreateTestEmployees(object parameter)
        {
            Random random = new Random();
            //RTBxOutput.Document.Blocks.Clear();
            BusinessRules toAdd = new BusinessRules();
            int numETypes = Enum.GetNames(typeof(ETYPE)).Length;
            //Iterate through the numbers and choose randomly from the first and last names. Other values are randomly generated using Random
            for (int i = 0; i < DEFAULT_NUM_TEST_EMPS; i++)
            {
                switch ((ETYPE)(i % numETypes))
                {
                    case ETYPE.CONTRACT:
                        toAdd.Add(new Contract((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.HOURLY:
                        toAdd.Add(new Hourly((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next()),
                            random.NextDouble() * random.Next()));
                        break;
                    case ETYPE.SALARY:
                        toAdd.Add(new Salary((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.SALES:
                        toAdd.Add(new Sales((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.Next() * random.NextDouble()),
                            (decimal)(random.Next() * random.NextDouble()),
                            (decimal)(random.Next() * random.NextDouble())));
                        break;
                }//End Switch
            }//End for loop
            EmployeesCollections.Add(toAdd);
        }

        private void CanAddEmployee()
        {
            throw new System.NotImplementedException();
        }

        private void CanChangeEmployee()
        {
            throw new System.NotImplementedException();
        }
    }//End Class BusinessRulesViewModel
}//End Namespace
