// Project Prolog
// Name: Eric Brown
// CS3260 Section 001
// Project: Lab_06
// Date: 9/16/17 9:00 PM
// Purpose: To display a window allowing the client to add employees or create random employees
// Changed BusinessRules class to work with a sorted dictionary and added some commonsense restrictions
// for the properties of the employee objects.
// I declare that the following code was written by me or provided 
// by the instructor for this project. I understand that copying source
// code from any other source constitutes plagiarism, and that I will receive
// a zero on this project if I am found in violation of this policy.
// ---------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_03_EAB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Constants used by the program
        private const uint NUM_TEST_EMPS = 10;
        private const string HOURLYRATE = "Hourly Rate:",
            SALARY = "Monthly Salary:",
            COMMISSION = "Commission:",
            GROSSSALES = "Gross Sales:",
            HOURSWORKED = "Hours Worked:",
            WAGE = "Contract Wage:",
            INVALID_EMP_TYPE_ERROR = "Invalid Employee Type",
            PARSING_ERRMSG = "There was a problem parsing the entered values.\nPlease try again.",
            CONTAINS_NUM_PATTERN = ".*[0-9]+.*",
            HELP_MSG = "Select the employee type that you would like to add and fill in the appropriate data. Once done, click \"Add Employee\".\nClicking \"Test Data\" creates random employees and displays the results.\nNo negative values are allowed for the numeric entries.",
            ABOUT = "About",
            ERROR = "Error",
            NEW_EMP_LINE = "-------New Employee-------\n",
            ENTER_VAL_MSG = "Please ensure that you have entered in a value for text box ",
            NO_NUM_MSG = "There is not allowed to be any numbers in text box",
            PLEASE_CHNG_MSG = ". Please change the input",
            ALL_EMP_LINE = "\n------All Employees-----",
            EMP_EXISTS_MSG = "An employee already exists which uses this Employee ID number.\nContinuing will update that employee with this new entry.\nIf you wish to make a new employee, please change the Employee Number.",
            EMP_EXISTS_CPTN = "Employee ID Collision Detected";
        private readonly string[] FIRST_NAMES = { "Stewart", "Sunny", "Grant", "Greg", "Micheal", "Seth", "Anthony", "Matthew", "Jonathon", "Jenny", "Sam" };
        private readonly string[] LAST_NAMES = { "Linder", "Brown", "DePoirot", "Johnson", "Williams", "Xavier", "Green", "Goldberg", "Greenburg", "Flotsam", "Jenkins", "Jensen", "Null" };
        //End Constants
        //Business Rules, used as storage.
        private BusinessRules businessLogic = new BusinessRules();

        /// <summary>
        /// Creates a set of test employees with dummy data and stores it in a list
        /// </summary>
        /// <param name="sender">The object listener</param>
        /// <param name="e">Any additional event handler arguments</param>
        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            RTBxOutput.Document.Blocks.Clear();
            businessLogic.Clear();
            int numETypes = Enum.GetNames(typeof(ETYPE)).Length;
            //Iterate through the numbers and choose randomly from the first and last names. Other values are randomly generated using Random
            for (int i = 0; i < NUM_TEST_EMPS; i++)
            {
                switch((ETYPE)(i%numETypes))
                {
                    case ETYPE.CONTRACT:
                        businessLogic[i] = (new Contract((uint)i, 
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble()*random.Next())));
                        break;
                    case ETYPE.HOURLY:
                        businessLogic[i] = (new Hourly((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next()),
                            random.NextDouble() * random.Next()));
                        break;
                    case ETYPE.SALARY:
                        businessLogic[i] = (new Salary((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.SALES:
                        businessLogic[i] = (new Sales((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.Next() * random.NextDouble()),
                            (decimal)(random.Next() * random.NextDouble()),
                            (decimal)(random.Next() * random.NextDouble())));
                        break;
                    default:
                        //Should never reach this
                        throw new Exception(INVALID_EMP_TYPE_ERROR);
                }//End Switch
                RTBxOutput.AppendText(businessLogic.Last()?.ToString());
            }//End for loop
        }
        
        /// <summary>
        /// Initializes the components of the WPF form and also populates the combobox with values from an enum
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            CBxEmpType.ItemsSource = Enum.GetNames(typeof(ETYPE));
            CBxEmpType.SelectedIndex = 0;
        }
        /// <summary>
        /// Attempts to add an employee to the list. If it cannot add an employee it either shows an error window or it spits out an error to the output textbox
        /// </summary>
        /// <param name="sender">The object listener</param>
        /// <param name="e">Any additional event handler arguments</param>
        private void BtnAddEmp_Click(object sender, RoutedEventArgs e)
        {
            //If any of these operations fail, there is no point to continuing
            if (!(uint.TryParse(TxtEmpID.Text, out uint id) &&
                ParseAlpha(TxtFirst, out string first) &&
                ParseAlpha(TxtLast, out string last) &&
                decimal.TryParse(TxtSup1.Text, out decimal sup1)))
            {
                OutputError();
                return;
            }
            if(businessLogic[id] != null)
            {
                if (MessageBox.Show(EMP_EXISTS_MSG, EMP_EXISTS_CPTN, MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK)
                    return;
            }
            try
            {
                //clear the output box for new data
                RTBxOutput.Document.Blocks.Clear();
                //find the selected type and then add the values and parse additional ones as needed
                switch ((ETYPE)CBxEmpType.SelectedIndex)
                {
                    case ETYPE.CONTRACT:
                        businessLogic[businessLogic.Count()] = (new Contract(id, first, last, sup1));
                        break;
                    case ETYPE.HOURLY:
                        if (double.TryParse(TxtSup2.Text, out double worked))
                            businessLogic[businessLogic.Count()] = (new Hourly(id, first, last, sup1, worked));
                        else
                        {
                            OutputError();
                            return;
                        }
                        break;
                    case ETYPE.SALARY:
                        businessLogic[businessLogic.Count()] = (new Salary(id, first, last, sup1));
                        break;
                    case ETYPE.SALES:
                        if (decimal.TryParse(TxtSup2.Text, out decimal commiss) && decimal.TryParse(TxtSup3.Text, out decimal sales))
                            businessLogic[businessLogic.Count()] = (new Sales(id, first, last, sup1, commiss, sales));
                        else
                        {
                            OutputError();
                            return;
                        }
                        break;
                    default:
                        //This should never reach here
                        throw new Exception(INVALID_EMP_TYPE_ERROR);
                }
            }//End Try
            catch (ArgumentException exc)
            {
                MessageBox.Show(exc.Message, ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //print out the newest employee so that the client can confirm
            RTBxOutput.AppendText(NEW_EMP_LINE + businessLogic.Last()?.ToString() + ALL_EMP_LINE);
            //print out all the employees
            foreach(Employee toPrint in businessLogic)
            {
                RTBxOutput.AppendText(toPrint?.ToString());
            }
        }
        /// <summary>
        /// Prints an error message to the output textbox
        /// </summary>
        private void OutputError()
        {
            RTBxOutput.AppendText(PARSING_ERRMSG);
        }
        /// <summary>
        /// Attempts to place a string into "result" that has no numbers in it
        /// </summary>
        /// <param name="toRead">The textbox to parse</param>
        /// <param name="result">The string result of the parse. Could be null.</param>
        /// <returns>True if the parse succeeded. False otherwise.</returns>
        private bool ParseAlpha(TextBox toRead, out string result)
        {
            if(string.IsNullOrEmpty(toRead.Text) || Regex.IsMatch(toRead.Text,CONTAINS_NUM_PATTERN))
            {
                result = null;
                if (string.IsNullOrEmpty(toRead.Text))
                    MessageBox.Show(ENTER_VAL_MSG + toRead.Name, ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show( NO_NUM_MSG + toRead.Name + PLEASE_CHNG_MSG, ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            result = toRead.Text.Trim();
            return true;
        }
        /// <summary>
        /// Displays a messagebox that lists directions and purpose for the program
        /// </summary>
        /// <param name="sender">The object listener</param>
        /// <param name="e">Any additional event handler arguments</param>
        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(HELP_MSG,ABOUT,MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Immediately exits the program
        /// </summary>
        /// <param name="sender">The object listener</param>
        /// <param name="e">Any additional event handler arguments</param>
        private void MnuExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
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
            switch((ETYPE)CBxEmpType.SelectedIndex)
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
