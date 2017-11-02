using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lab_03_EAB.Helpers;
using System.Collections.ObjectModel;

namespace Lab_03_EAB.EmployeeViewModel
{
    class BusinessRulesViewModel : DependencyObject
    {
        private System.Windows.DependencyProperty EmployeesProperty;
        private System.Windows.DependencyProperty SelectedEmployeeProperty;
        private System.Windows.DependencyProperty CloseWindowFlagProperty;

        public BusinessRulesViewModel()
        {
            throw new System.NotImplementedException();
        }

        public Employee SelectedEmployee
        {
            get;set;
        }

        public ObservableCollection<Lab_03_EAB.BusinessRules> Employees
        {
            get => default(int);
            set
            {
            }
        }

        public bool? CloseWindowFlag
        {
            get => default(int);
            set
            {
            }
        }

        public RelayCommand AddEmployeeCommand
        {
            get => default(int);
            set
            {
            }
        }

        public RelayCommand RemoveEmployeeCommand
        {
            get => default(int);
            set
            {
            }
        }

        public RelayCommand ChangeEmployeeCommand
        {
            get => default(int);
            set
            {
            }
        }

        public RelayCommand SaveFileCommand
        {
            get => default(int);
            set
            {
            }
        }

        public RelayCommand OpenFileCommand
        {
            get => default(int);
            set
            {
            }
        }

        public RelayCommand NewFileCommand
        {
            get => default(int);
            set
            {
            }
        }

        public RelayCommand CreateTestEmployeesCommand
        {
            get => default(int);
            set
            {
            }
        }

        private void AddEmployee()
        {
            throw new System.NotImplementedException();
        }

        private void ChangeEmployee()
        {
            throw new System.NotImplementedException();
        }

        private void NewFile()
        {
            throw new System.NotImplementedException();
        }

        private void OpenFile()
        {
            throw new System.NotImplementedException();
        }

        private void SaveFile()
        {
            throw new System.NotImplementedException();
        }

        private void RemoveEmployee()
        {
            throw new System.NotImplementedException();
        }

        private void CreateTestEmployees()
        {
            throw new System.NotImplementedException();
        }
    }
}
