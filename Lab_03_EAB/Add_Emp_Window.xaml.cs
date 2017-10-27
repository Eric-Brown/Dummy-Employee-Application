using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab_03_EAB
{
    /// <summary>
    /// Interaction logic for Add_Emp_Window.xaml
    /// </summary>
    public partial class Add_Emp_Window : Window
    {
        private const string HOURLYRATE = "Hourly Rate:",
            SALARY = "Monthly Salary:",
            COMMISSION = "Commission:",
            GROSSSALES = "Gross Sales:",
            HOURSWORKED = "Hours Worked:",
            WAGE = "Contract Wage:",
            INVALID_EMP_TYPE_ERROR = "Invalid Employee Type",
            PARSING_ERRMSG = "There was a problem parsing the entered values.\nPlease try again.",
            CONTAINS_NUM_PATTERN = ".*[0-9]+.*",
            ENTER_VAL_MSG = "Please ensure that you have entered in a value for text box ",
            NO_NUM_MSG = "There is not allowed to be any numbers in text box",
            PLEASE_CHNG_MSG = ". Please change the input",
            EMP_EXISTS_MSG = "An employee already exists which uses this Employee ID number.\nContinuing will update that employee with this new entry.\nIf you wish to make a new employee, please change the Employee Number.",
            EMP_EXISTS_CPTN = "Employee ID Collision Detected";
        private const string ADD_EMP_ERR_MSG = "Could not add the employee. Please verify that all the entered information is in the proper format.";
        private const string EMP_NOT_ADDED_CAPTION = "Employee Not Added";
        private const string MOD_EMP_TITLE = "Modify Employee";
        private const string MOD_EMP_BTN_NAME = "Modify";

        /// <summary>
        /// closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private BusinessRules businessRules;
        /// <summary>
        /// Window loaded event handler.
        /// Populates the window with information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CBxEmpType.ItemsSource = Enum.GetNames(typeof(ETYPE));
            if (selectedItem != null)
            {
                Employee temp = selectedItem as Employee;
                CBxEmpType.SelectedItem = Enum.GetName(typeof(ETYPE),temp?.EmpType);
                CBxEmpType.IsEnabled = false;
                TxtFirst.Text = temp?.FirstName;
                TxtLast.Text = temp?.LastName;
                TxtEmpID.Text = temp?.EmpID.ToString();
                TxtEmpID.IsEnabled = false;
                switch(temp?.EmpType)
                {
                    case ETYPE.CONTRACT:
                        TxtSup1.Text = (temp as Contract).ContractWage.ToString();
                        break;
                    case ETYPE.HOURLY:
                        TxtSup1.Text = (temp as Hourly).HourlyRate.ToString();
                        TxtSup2.Text = (temp as Hourly).HoursWorked.ToString();
                        break;
                    case ETYPE.SALES:
                        TxtSup1.Text = (temp as Sales).MonthlySalary.ToString();
                        TxtSup2.Text = (temp as Sales).Commission.ToString();
                        TxtSup3.Text = (temp as Sales).GrossSales.ToString();
                        break;
                    case ETYPE.SALARY:
                        TxtSup1.Text = (temp as Salary).MonthlySalary.ToString();
                        break;
                }
            }
            else
            {
                CBxEmpType.SelectedIndex = 0;
                CBxEmpType.IsEnabled = true;
                TxtEmpID.IsEnabled = true;
            }
        }

        private object selectedItem;
        /// <summary>
        /// Event handler for the Add/Mod button click.
        /// Attempts to add an employee, and displays a message box on failure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddMod_Click(object sender, RoutedEventArgs e)
        {
            List<string> entries = new List<string> {
                TxtEmpID.Text,
                TxtFirst.Text,
                TxtLast.Text,
                TxtSup1.Text,
                TxtSup2.Text,
                TxtSup3.Text
            };
            ETYPE selected = (ETYPE)Enum.Parse(typeof(ETYPE), CBxEmpType.SelectedItem.ToString());
            if (businessRules.CanAddEntry(selected, entries.ToArray()))
            {
                businessRules.AddFromArray(selected, entries.ToArray());
                this.Close();
            }
            else
                MessageBox.Show(ADD_EMP_ERR_MSG, EMP_NOT_ADDED_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /// <summary>
        /// Constructor that keeps a connection to businessrules
        /// </summary>
        /// <param name="rules"></param>
        public Add_Emp_Window(BusinessRules rules)
        {
            InitializeComponent();
            businessRules = rules;
        }
        /// <summary>
        /// constructor when the modify event is triggered
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <param name="rules"></param>
        public Add_Emp_Window(object selectedItem, BusinessRules rules)
        {
            InitializeComponent();
            if (selectedItem != null)
            {
                this.selectedItem = selectedItem;
                Title = MOD_EMP_TITLE;
                BtnAddMod.Content = MOD_EMP_BTN_NAME;
            }
            businessRules = rules;
        }

        /// <summary>
        /// Changes label and textbox content and visibility depending on what is selected in the combobox
        /// </summary>
        /// <param name="sender">The object listener</param>
        /// <param name="e">Any additional event handler arguments</param>
        private void CBxEmpType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LblSup2.Visibility = Visibility.Collapsed;
            TxtSup2.Visibility = Visibility.Collapsed;
            LblSup3.Visibility = Visibility.Collapsed;
            TxtSup3.Visibility = Visibility.Collapsed;
            switch ((ETYPE)CBxEmpType.SelectedIndex)
            {
                case ETYPE.SALARY:
                    LblSup1.Content = SALARY;
                    break;
                case ETYPE.SALES:
                    LblSup1.Content = SALARY;
                    LblSup2.Content = COMMISSION;
                    LblSup3.Content = GROSSSALES;
                    LblSup2.Visibility = Visibility.Visible;
                    TxtSup2.Visibility = Visibility.Visible;
                    LblSup3.Visibility = Visibility.Visible;
                    TxtSup3.Visibility = Visibility.Visible;
                    break;
                case ETYPE.HOURLY:
                    LblSup1.Content = HOURLYRATE;
                    LblSup2.Content = HOURSWORKED;
                    LblSup2.Visibility = Visibility.Visible;
                    TxtSup2.Visibility = Visibility.Visible;
                    break;
                case ETYPE.CONTRACT:
                    LblSup1.Content = WAGE;
                    break;
                default:
                    //This should never get here
                    throw new Exception(INVALID_EMP_TYPE_ERROR);
            }
        }
    }
}
