using Lab_03_EAB.EmployeeModel;
using Lab_03_EAB.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace Lab_03_EAB.EmployeeViewModel
{
    internal class BusinessRulesViewModel : INotifyPropertyChanged
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
        private static readonly string[] FIRST_NAMES = {
            "D'Marcus", "T'varisuness", "Tyroil", "D'Squarius", "Ibrahim",
            "T.J.", "Jackmerius", "D'Isiah", "D'Jasper", "Leoz", "Javaris", "Davoin",
            "Grant", "Hingle", "L'Carpetron", "J'Dinkalage", "Xmus Jaxon",
            "Greg", "Saggitariutt", "D'Glester", "Swirvithan", "Quatro", "Beezer",
            "Micheal", "Shakiraquan", "X-Wing", "Sequester", "Scoish", "T.J. A.J.",
            "Seth", "Donkey", "Torque", "Eeeeeee", "Coznesster", "Elipses", "Nyquillus",
            "Anthony", "Bismo", "Decatholac", "Mergatroid", "Quiznatodd", "D'Pez", "Quackadilly",
            "Matthew", "Goolius", "Bisquiteen", "Fartrell", "Blyrone", "Cartoons", "Jammie",
            "Jonathon", "Equine", "Dahistorius", "Ewokoniad", "Eqqsnuizitine", "Huka'laknaka",
            "Jenny", "King", "Ladennifer", "Harvard", "Firstname", "Creme", "Cosgrove", "Ha Ha",
            "Sam", "Doink", "Legume", "Leger", "Quisperny", "Grunky","D'Brickashaw", "Strunk",
            "Stumptavian", "Cornelius", "Vagonius", "Marmadune","Swordless", "Prince", "J.R. Junior",
            "Faux", "Fozzy", "Myriad", "Busters", "Turdine", "Rerutweeds", "Ishmaa'ily", "Takittothu'",
            "Snarf", "Frostee", "Splendiferous", "Triple", "Logjammer"
        };

        /// <summary>
        /// Used to create random last names for employees.
        /// </summary>
        private static readonly string[] LAST_NAMES = {
            "Linder", "Williums", "juckson", "King", "Smoochie-Wallace", "Green",
            "Brown", "Moizoos", "Tacktheritrix", "Billings-Clyde", "Probincrux III",
            "DePoirot", "Jilliumz", "Javarison-Lamar", "Shower-Handel", "McCringleberry",
            "Johnson", "Dookmarriot", "Morgoone", "Flaxon-Waxon", "Jefferspin", "Hardunkichud",
            "Williams", "L'Goodling-Splatt", "Quatro", "Buckshank", "Washingbeard", "Carter",
            "Xavier", "@Aliciousness", "Grundelplith", "Maloish", "Backslishinfourth V",
            "Green", "Eeeeeeee", "Teeth", "Lewith", "Smiff", "Corter", "Dillwad", "Funyuns",
            "Goldberg", "Mango", "Skittle", "Bidness", "Poopsie", "Blip", "Boozler", "Trisket",
            "Greenburg", "Cluggins", "Blashington", "Plural", "Jammie-Jammie", "Ducklings",
            "Flotsam", "Lamystorius", "Sigourneth JuniorStein", "Buble-Schineslow", "Hakanakaheekalucka'hukahakafaka",
            "Jenkins", "Chambermaid", "Jadaniston", "University", "Lastname", "De La Creme",
            "Jensen", "Shumway", "Clinton-Dix", "Ahanahue","Duprix","Douzable","G'Dunzoid Sr.",
            "Null", "Peep", "Ferguson","Flugget","Roboclick", "Carradine","Thicket-Suede","Shazbot",
            "Mimetown", "Amukamara","Juniors Jr.", "Doadles","Whittaker","Profiteroles","Brownce",
            "Cupcake","Myth","Kitchen","Limit","Mintz-Plasse","Rucker","Finch","Parakeet-Shoes",
            "D'Baggagecling", "Ron Rodgers"
        };

        private static readonly string[] SUBJECTS =
        {
            "CS", "ECE", "ANTH", "GEOG", "ARTH", "BIOL", "CHEM", "HLTH", "ECON", "EDUC", "COMM", "COMP", "LNGS",
            "HIST", "IT", "MATH","LATN", "ESL", "LANG", "MUAC", "PHIL", "PSYC", "STAT", "SOCI", "DRAM", "MUCC"
        };

        private static readonly string FAKE_DESCRIPTION = "Imagine a good description here.";
        private const int COURSE_DIVISIONS = 4;

        /// <summary>
        /// Default number of random employees to create when creating test employees.
        /// </summary>
        private const int DEFAULT_NUM_TEST_EMPS = 10;

        private const string OPEN_FILE_MSG = "This file is already open.";
        private const int DIVISION = 1000;
        private const int SECTION = 100;
        private const int MAX_CRED = 5;

        #endregion Constants

        public event PropertyChangedEventHandler PropertyChanged;

        #region Data Properties

        private ICollectionView viewEmployees;
        private Employee selectedEmployee;

        public Employee SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = (Employee)value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedEmployee)));
                SelectedEmployeeDescription = selectedEmployee.ToString();
            }
        }

        private string searchText;

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SearchText)));
                viewEmployees.Refresh();
            }
        }

        private void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            PropertyChanged?.Invoke(this, propertyChangedEventArgs);
        }

        public int Index
        {
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

        private string selectedEmployeeDescr;

        public string SelectedEmployeeDescription
        {
            get => selectedEmployeeDescr;
            private set
            {
                selectedEmployeeDescr = value;
                PropertyChanged?.Invoke(this, (new PropertyChangedEventArgs(nameof(SelectedEmployeeDescription))));
            }
        }

        #endregion Data Properties

        #region Command Properties

        private void SearchBarFilter(object item, FilterEventArgs args)
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                args.Accepted = true;
                return;
            }
            Regex toMatch = new Regex(".*" + SearchText + ".*", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            if (args.Item is Employee emp)
            {
                if (toMatch.IsMatch(emp.ToString()))
                {
                    args.Accepted = true;
                    return;
                }
            }
            args.Accepted = false;
        }

        private bool SearchBarFilterPredicate(object item)
        {
            Employee greg = item as Employee;
            if (string.IsNullOrEmpty(SearchText))
            {
                return true;
            }
            Regex toMatch = new Regex(".*" + SearchText + ".*", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            if (greg != null)
            {
                if (toMatch.IsMatch(greg.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        public RelayCommand RemoveEmployeeCommand
        {
            get; set;
        }

        private void RemoveEmployee(object employee)
        {
            employees.Remove(employee as Employee);
        }

        public RelayCommand MoveEmployeeCommand
        {
            get; set;
        }

        public RelayCommand AddEmployeeCommand
        {
            get; set;
        }

        private void AddEmployee(object parameter)
        {
            Add_Emp_Window add_Emp_Window = new Add_Emp_Window();
            add_Emp_Window.DataContext = new EmployeeViewModel(Employees);
            add_Emp_Window.Show();
        }

        public RelayCommand ModifyEmployeeCommand
        {
            get; set;
        }

        private void ModifyEmployee(object employee)
        {
            Add_Emp_Window add_Emp_Window = new Add_Emp_Window();
            Employee toPass = employee as Employee;
            add_Emp_Window.DataContext = new EmployeeViewModel(toPass, Employees);
            add_Emp_Window.Show();
        }

        public RelayCommand SaveFileCommand
        {
            get; set;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand OpenFileCommand
        {
            get; set;
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
                    if (EmployeesCollections.Where(a => a.FilePath == toAdd.FilePath).Count() == 0)
                        EmployeesCollections.Add(toAdd);
                    else MessageBox.Show(OPEN_FILE_MSG, ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand NewFileCommand
        {
            get; set;
        }

        private void NewFile(object parameter)
        {
            EmployeesCollections.Add(new BusinessRules());
        }

        public RelayCommand CreateTestEmployeesCommand
        {
            get; set;
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
            foreach (Employee emp in toAdd)
            {
                for (int i = 0; i < random.Next(0, COURSE_DIVISIONS + 1); i++)
                {
                    string courseID = $"{SUBJECTS[random.Next(0, SUBJECTS.Length - 1)]}{random.Next(0, COURSE_DIVISIONS) * DIVISION + random.Next(0, COURSE_DIVISIONS) * SECTION}";
                    Array gradeValues = Enum.GetValues(typeof(Course.COURSE_GRADE));
                    Course aCourse = new Course(courseID, FAKE_DESCRIPTION, (Course.COURSE_GRADE)gradeValues.GetValue(random.Next(0, gradeValues.Length - 1)), DateTime.Now, random.Next(1, MAX_CRED));
                    try
                    {
                        emp.CourseRoster.Add(courseID, aCourse);
                    }
                    finally { }
                }
            }
            EmployeesCollections.Add(toAdd);
        }

        #endregion Command Properties

        public BusinessRulesViewModel()
        {
            EmployeesCollections = new ObservableCollection<BusinessRules>();
            CreateTestEmployees(null);
            Employees = EmployeesCollections.First<BusinessRules>();
            viewEmployees = CollectionViewSource.GetDefaultView(employees);
            viewEmployees.Filter = SearchBarFilterPredicate;
            //Set up commands next
            CreateTestEmployeesCommand = new RelayCommand(CreateTestEmployees);
            SaveFileCommand = new RelayCommand(SaveFile);
            NewFileCommand = new RelayCommand(NewFile);
            OpenFileCommand = new RelayCommand(OpenFile);
            RemoveEmployeeCommand = new RelayCommand(RemoveEmployee);
            ModifyEmployeeCommand = new RelayCommand(ModifyEmployee);
            AddEmployeeCommand = new RelayCommand(AddEmployee);
        }
    }//End Class BusinessRulesViewModel
}//End Namespace