using Lab_03_EAB.EmployeeModel;
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
        /// <summary>
        /// Window loaded event handler.
        /// Populates the window with information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Constructor that keeps a connection to businessrules
        /// </summary>
        public Add_Emp_Window()
        {
            InitializeComponent();
            CmbBoxEmpType.ItemsSource = Enum.GetValues(typeof(ETYPE));
            CmbBoxGrade.ItemsSource = Enum.GetValues(typeof(COURSE_GRADE));
            DataContext = new EmployeeViewModel.EmployeeViewModel(new BusinessRules());
        }

    }
}