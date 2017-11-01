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
            HELP_MSG = @"Use the file menu to navigate to an employee database that is already created.
Or click the buttons to manually add employees, or to create test employees, or to modify an existing employee.
Created by: Eric Brown",
            INVALID_EMP_TYPE_ERROR = "Invalid Employee Type",

            ALL_EMP_LINE = "\n------All Employees-----";

        //End Constants
        //Business Rules, used as storage.
        private BusinessRules businessLogic = new BusinessRules();

        private void MnuSave_Click(object sender, RoutedEventArgs e)
        {
            using (FileIO file = new FileIO(businessLogic))
            {

            }
        }

        private void DisplayException(Exception ex)
        {
            throw new NotImplementedException();
        }

        private void MnuOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                businessLogic.OpenFile();
            }
            catch(Exception ex)
            {
                DisplayException(ex);
            }
        }

        private void MnuNew_Click(object sender, RoutedEventArgs e)
        {
            businessLogic.Clear();
            //What I want:
            //businessLogic.NewFile();
            //Maybe overloaded to accept path of new file?
            businessLogic.Newfile();
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            Add_Emp_Window window = new Add_Emp_Window(DatGridTab1.SelectedItem, businessLogic);
            window.Show();
        }

        private void DatGridTab1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BtnMod_Click(sender, e);
        }

        private void DatGridTab1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RTBxOutput.Document.Blocks.Clear();
            RTBxOutput.AppendText(DatGridTab1.SelectedItem?.ToString());
        }

        private void BtnTestNum_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            businessLogic.Remove(DatGridTab1.SelectedItem as Employee);
        }

        /// <summary>
        /// Creates a set of test employees with dummy data and stores it in a list
        /// </summary>
        /// <param name="sender">The object listener</param>
        /// <param name="e">Any additional event handler arguments</param>
        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            businessLogic.AddTestEmps();
        }
        
        /// <summary>
        /// Initializes the components of the WPF form and also populates the combobox with values from an enum
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DatGridTab1.ItemsSource = businessLogic;
        }
        /// <summary>
        /// Attempts to add an employee to the list. If it cannot add an employee it either shows an error window or it spits out an error to the output textbox
        /// </summary>
        /// <param name="sender">The object listener</param>
        /// <param name="e">Any additional event handler arguments</param>
        private void BtnAddEmp_Click(object sender, RoutedEventArgs e)
        {
            Add_Emp_Window window = new Add_Emp_Window(businessLogic);
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
