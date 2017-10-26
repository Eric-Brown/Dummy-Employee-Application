// Project Prolog
// Name: Eric Brown
// CS3260 Section 001
// Project: Lab_06
// Date: 10/19/17 10:00 PM
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
        private const string
            ABOUT = "About",
            ERROR = "Error",
            NEW_EMP_LINE = "-------New Employee-------\n",
            HELP_MSG = "Select the employee type that you would like to add and fill in the appropriate data. Once done, click \"Add Employee\".\nClicking \"Test Data\" creates random employees and displays the results.\nNo negative values are allowed for the numeric entries.",
                        INVALID_EMP_TYPE_ERROR = "Invalid Employee Type",

            ALL_EMP_LINE = "\n------All Employees-----";

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
            //RTBxOutput.Document.Blocks.Clear();
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
                //RTBxOutput.AppendText(businessLogic.Last()?.ToString());
            }//End for loop
            DatGridTab1.ItemsSource = null;
            DatGridTab1.ItemsSource = businessLogic;
        }
        
        /// <summary>
        /// Initializes the components of the WPF form and also populates the combobox with values from an enum
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            businessLogic.Add(new Salary(2, "sam", "iam", 12));
            //businessLogic.Add(new Contract(3, "blah", "do", 2));
            DatGridTab1.ItemsSource = businessLogic;
        }
        /// <summary>
        /// Attempts to add an employee to the list. If it cannot add an employee it either shows an error window or it spits out an error to the output textbox
        /// </summary>
        /// <param name="sender">The object listener</param>
        /// <param name="e">Any additional event handler arguments</param>
        private void BtnAddEmp_Click(object sender, RoutedEventArgs e)
        {
            Add_Emp_Window window = new Add_Emp_Window();
            window.Show();
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


    }
}
