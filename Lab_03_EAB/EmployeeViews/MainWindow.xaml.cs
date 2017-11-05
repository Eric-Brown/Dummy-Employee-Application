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
using Lab_03_EAB.EmployeeViewModel;

namespace Lab_03_EAB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string HELP_MSG = @"Use the file menu to navigate to an employee database that is already created.
Or click the buttons to manually add employees, or to create test employees, or to modify an existing employee.
Created by: Eric Brown",
                ABOUT = "About";

        /// <summary>
        /// Initializes the components of the WPF form and also populates the combobox with values from an enum
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new EmployeeViewModel.BusinessRulesViewModel();
        }

        private void OnAddRequest(object sender, RoutedEventArgs e)
        {
            Add_Emp_Window add_Emp_Window = new Add_Emp_Window();
            if(DataContext is BusinessRulesViewModel model)
            {
                add_Emp_Window.DataContext =  new EmployeeViewModel.EmployeeViewModel(model.Employees);
            }
            add_Emp_Window.Show();
        }
        private void OnModifyEmployeeRequest(object sender, RoutedEventArgs e)
        {
            Add_Emp_Window add_Emp_Window = new Add_Emp_Window();
            if (DataContext is BusinessRulesViewModel model)
            {
                Employee selected = model.SelectedEmployee;
                add_Emp_Window.DataContext = new EmployeeViewModel.EmployeeViewModel(model.SelectedEmployee, model.Employees);
            }
            add_Emp_Window.Show();
        }
        private void OnAddTestEmployeesCountRequest(object sender, RoutedEventArgs e)
        {

        }

        public void MnuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(HELP_MSG, ABOUT, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
