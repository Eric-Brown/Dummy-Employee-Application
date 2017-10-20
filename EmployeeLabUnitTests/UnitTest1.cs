using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab_03_EAB;

namespace EmployeeLabUnitTests
{
    [TestClass]
    public class TestEmployee
    {
        private const string FIRST = "Samway", LAST = "Jackall";
        private const double setHours = 10.0;
        private const decimal setHourRate = 5.0M,
            setContractWage = 10.0M,
            setMonthlySalary = 20.0M,
            setCommission = 11.0M,
            setGrossSales = 12.0M;
        private const string BASE_FORMAT_STRING = "EmpID: {0}\nEmpType: {1}\nFirst Name: {2}\nLast Name: {3}\n",
            HOURLY_FORMAT_STRING = "Hourly Rate: {4}\nHours Worked: {5}\n",
            CONTRACT_FORMAT_STRING = "Contract Wage: {4}\n",
            SALARY_FORMAT_STRING = "Monthly Salary: {4}\n",
            SALES_FORMAT_STRING = "Commission: {5}\nGross Sales: {6}\n";
        [TestMethod]
        public void TestHourlyToString()
        {
            Hourly testHourlyEmp = new Hourly(0, FIRST, LAST, setHourRate, setHours);
            string expected = string.Format(BASE_FORMAT_STRING + HOURLY_FORMAT_STRING, 0, testHourlyEmp.EmpType, FIRST, LAST, setHourRate, setHours);
            string actual = testHourlyEmp.ToString();
            Assert.AreEqual(expected, testHourlyEmp.ToString());
        }
        [TestMethod]
        public void TestContractToString()
        {
            Contract testContractEmp = new Contract(0, FIRST, LAST, setContractWage);
            string expected = string.Format(BASE_FORMAT_STRING + CONTRACT_FORMAT_STRING, 0, testContractEmp.EmpType, FIRST, LAST, setContractWage);
            Assert.AreEqual(expected, testContractEmp.ToString());
        }
        [TestMethod]
        public void TestSalesToString()
        {
            Sales testSalesEmp = new Sales(0, FIRST, LAST,setMonthlySalary,setCommission,setGrossSales);
            string expected = string.Format(BASE_FORMAT_STRING + SALARY_FORMAT_STRING + SALES_FORMAT_STRING, 0, testSalesEmp.EmpType, FIRST, LAST, setMonthlySalary,setCommission,setGrossSales);
            string actual = testSalesEmp.ToString();
            Assert.AreEqual(expected, testSalesEmp.ToString());
        }
        [TestMethod]
        public void TestSalaryToString()
        {
            Salary testSalaryEmp = new Salary(0, FIRST, LAST,setMonthlySalary);
            string expected = string.Format(BASE_FORMAT_STRING + SALARY_FORMAT_STRING, 0, testSalaryEmp.EmpType, FIRST, LAST, setMonthlySalary);
            Assert.AreEqual(expected, testSalaryEmp.ToString());
        }
        [TestMethod]
        public void TestEmployeeProperties()
        {
            Salary testSalaryEmp = new Salary(0, FIRST, LAST, setMonthlySalary);
            string unexpected = "";
            testSalaryEmp.FirstName = unexpected;
            Assert.AreNotEqual(unexpected, testSalaryEmp.FirstName);
            testSalaryEmp.LastName = unexpected;
            Assert.AreNotEqual(unexpected, testSalaryEmp.LastName);
            string expected = "Greg";
            testSalaryEmp.FirstName = expected;
            Assert.AreEqual(expected, testSalaryEmp.FirstName);
            testSalaryEmp.LastName = expected;
            Assert.AreEqual(expected, testSalaryEmp.LastName);
        }
        [TestMethod]
        public void TestHourlyProperties()
        {
            Hourly testHourlyEmp = new Hourly(0, FIRST, LAST, setHourRate, setHours);
            decimal unexpected = testHourlyEmp.HourlyRate * -1;
            testHourlyEmp.HourlyRate = unexpected;
            Assert.AreNotEqual(unexpected, testHourlyEmp.HourlyRate);
            double unexpectedHours = testHourlyEmp.HoursWorked * -1;
            testHourlyEmp.HoursWorked = unexpectedHours;
            Assert.AreNotEqual(unexpectedHours, testHourlyEmp.HoursWorked);
            decimal expected = testHourlyEmp.HourlyRate * 2;
            testHourlyEmp.HourlyRate = expected;
            Assert.AreEqual(expected, testHourlyEmp.HourlyRate);
            double expectedHours = testHourlyEmp.HoursWorked * 2;
            testHourlyEmp.HoursWorked = expectedHours;
            Assert.AreEqual(expectedHours, testHourlyEmp.HoursWorked);
        }
        [TestMethod]
        public void TestContractProperties()
        {
            Contract testContractEmp = new Contract(0, FIRST, LAST, setContractWage);
            decimal expected = testContractEmp.ContractWage * 2;
            testContractEmp.ContractWage = expected;
            Assert.AreEqual(expected, testContractEmp.ContractWage);
            decimal unexpected = testContractEmp.ContractWage * -1;
            testContractEmp.ContractWage = unexpected;
            Assert.AreNotEqual(unexpected, testContractEmp.ContractWage);
        }
        [TestMethod]
        public void TestSalesProperties()
        {
            Sales testSalesEmp = new Sales(0, FIRST, LAST, setMonthlySalary, setCommission, setGrossSales);
            decimal expected = setCommission + 1;
            testSalesEmp.Commission = expected;
            Assert.AreEqual(expected, testSalesEmp.Commission);
            decimal expectedSales = setGrossSales + 1;
            testSalesEmp.GrossSales = expectedSales;
            Assert.AreEqual(expectedSales, testSalesEmp.GrossSales);
            decimal unexpected = testSalesEmp.Commission * -1;
            testSalesEmp.Commission = unexpected;
            Assert.AreNotEqual(unexpected, testSalesEmp.Commission);
            decimal unexpectedSales = testSalesEmp.GrossSales * -1;
            testSalesEmp.GrossSales = unexpectedSales;
            Assert.AreNotEqual(unexpectedSales, testSalesEmp.GrossSales);
        }
        [TestMethod]
        public void TestSalaryProperties()
        {
            Salary testSalaryEmp = new Salary(0, FIRST, LAST, setMonthlySalary);
            decimal expected = testSalaryEmp.MonthlySalary + 1;
            testSalaryEmp.MonthlySalary = expected;
            Assert.AreEqual(expected, testSalaryEmp.MonthlySalary);
            decimal unexpected = testSalaryEmp.MonthlySalary * -1;
            testSalaryEmp.MonthlySalary = unexpected;
            Assert.AreNotEqual(unexpected, testSalaryEmp.MonthlySalary);
        }

    }
    [TestClass]
    public class TestBusinessRules
    {
        [TestMethod]
        public void TestIndex()
        {
            BusinessRules toTest = new BusinessRules();
            Assert.AreEqual(toTest[0], null);
        }
    }
}
