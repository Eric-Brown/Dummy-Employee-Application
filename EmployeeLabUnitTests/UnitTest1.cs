// ****** Note: Project Prolog added here in case it was not meant to be in MainWindow.xaml.cs *****
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab_03_EAB;
using System.Linq;

namespace EmployeeLabUnitTests
{
    /// <summary>
    /// This class contains tests which tests the functionality of Employee and it's subtypes.
    /// </summary>
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
        private const string EXPECTED_NAME = "Greg";

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
            string expected = EXPECTED_NAME;
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
    /// <summary>
    /// Class which contains tests for the functionality of the BusinessRules class
    /// </summary>
    [TestClass]
    public class TestBusinessRules
    {
        private readonly string[] FIRST_NAMES = { "Stewart", "Sunny", "Grant", "Greg", "Micheal", "Seth", "Anthony", "Matthew", "Jonathon", "Jenny", "Sam" };
        private readonly string[] LAST_NAMES = { "Linder", "Brown", "DePoirot", "Johnson", "Williams", "Xavier", "Green", "Goldberg", "Greenburg", "Flotsam", "Jenkins", "Jensen", "Null" };
        private const int DEFAULT_EMPS_TO_CREATE = 10;
        private const string INVALID_EMP_TYPE = "Invalid employee type attempted to be created.";
        private const string TEST_LAST_NAME = "A";
        /// <summary>
        /// Tests the indexer of BusinessRules using it as though it were an array
        /// </summary>
        [TestMethod]
        public void TestIndex()
        {
            BusinessRules toTest = new BusinessRules();
            Assert.AreEqual(toTest[0], null);
            Assert.AreEqual(toTest[int.MinValue], null);
            PopulateBusinessRules(toTest);
            Assert.AreNotEqual(toTest[0], null);
            Assert.AreNotEqual(toTest[0], toTest[DEFAULT_EMPS_TO_CREATE - 1]);
            BusinessRules orderTest = new BusinessRules();
            //Testing if order is preserved for the object.
            for (int i = DEFAULT_EMPS_TO_CREATE - 1, j = 0; i >= 0; --i, ++j)
                orderTest[j] = toTest[i];
            for (int i = 0; i < DEFAULT_EMPS_TO_CREATE; i++)
                Assert.AreEqual(orderTest[i], toTest[i]);
            toTest[0] = null;
            Assert.AreEqual(toTest[0], null);
        }
        /// <summary>
        /// Tests the .Count() and .Clear() methods in BusinessRules
        /// </summary>
        [TestMethod]
        public void TestCountAndClear()
        {
            BusinessRules toTest = new BusinessRules();
            Assert.AreEqual(0,toTest.Count());
            PopulateBusinessRules(toTest);
            Assert.AreEqual(DEFAULT_EMPS_TO_CREATE, toTest.Count());
            PopulateBusinessRules(toTest,DEFAULT_EMPS_TO_CREATE * 2);
            Assert.AreEqual(DEFAULT_EMPS_TO_CREATE * 2, toTest.Count());
            toTest.Clear();
            Assert.AreEqual(toTest.Count(), 0);
            Assert.AreEqual(toTest[0], null);
            toTest.Clear();
            Assert.AreEqual(toTest.Count(), 0);
        }
        /// <summary>
        /// Tests the .Last() method of BusinessRules
        /// </summary>
        [TestMethod]
        public void TestLast()
        {
            BusinessRules toTest = new BusinessRules();
            Assert.AreEqual(toTest.Last(), null);
            PopulateBusinessRules(toTest);
            Assert.AreNotEqual(toTest.Last(), null);
            Assert.AreEqual(toTest[DEFAULT_EMPS_TO_CREATE - 1], toTest.Last());
        }
        /// <summary>
        /// Tests the indexing of BusinessRules using employee ID as a key
        /// </summary>
        [TestMethod]
        public void TestEmpIDIndex()
        {
            BusinessRules toTest = new BusinessRules();
            uint index = 0;
            Assert.AreEqual(null, toTest[index]);
            Salary toAdd = new Salary(index + DEFAULT_EMPS_TO_CREATE, FIRST_NAMES[0], LAST_NAMES[0], 0);
            toTest[0] = toAdd;
            Assert.AreEqual(toAdd, toTest[(uint)(index + DEFAULT_EMPS_TO_CREATE)]);
            PopulateBusinessRules(toTest, 100);
            Random random = new Random();
            //Taking advantage of knowledge of the "PopulateBusinessRules" function
            toAdd.EmpID = 200;
            Assert.AreNotEqual(toAdd.EmpID, toTest[(uint)random.Next(0, toTest.Count())].EmpID);
            Assert.AreEqual((uint)200, toTest[(uint)200].EmpID);
        }
        /// <summary>
        /// Tests the enumerator of BusinessClass
        /// </summary>
        [TestMethod]
        public void TestEnumerator()
        {
            BusinessRules toTest = new BusinessRules();
            PopulateBusinessRules(toTest);
            //changes should stick if done in a loop like this
            foreach (Employee foo in toTest)
            {
                foo.LastName = TEST_LAST_NAME;
            }
            Assert.AreEqual(TEST_LAST_NAME, toTest[0].LastName);
            foreach(Employee foo in toTest)
            {
                foo.EmpID = 0;
            }
            Assert.AreEqual(1, toTest.Count());
        }
        /// <summary>
        /// Populates a BusinessRules object with a specified number of random employees
        /// </summary>
        /// <param name="toPopulate">The BusinessRules object to populate</param>
        /// <param name="numToCreate">The amount of employees to populate the BusinessRules object with.</param>
        private void PopulateBusinessRules(BusinessRules toPopulate, int numToCreate = DEFAULT_EMPS_TO_CREATE)
        {
            Random random = new Random();
            toPopulate.Clear();
            int numETypes = Enum.GetNames(typeof(ETYPE)).Length;
            for (int i = 0; i < numToCreate; i++)
            {
                switch ((ETYPE)(i % numETypes))
                {
                    case ETYPE.CONTRACT:
                        toPopulate[i] = (new Contract((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.HOURLY:
                        toPopulate[i] = (new Hourly((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next()),
                            random.NextDouble() * random.Next()));
                        break;
                    case ETYPE.SALARY:
                        toPopulate[i] = (new Salary((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.SALES:
                        toPopulate[i] = (new Sales((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.Next() * random.NextDouble()),
                            (decimal)(random.Next() * random.NextDouble()),
                            (decimal)(random.Next() * random.NextDouble())));
                        break;
                    default:
                        //If code reaches here, something is seriously wrong.
                        throw new Exception(INVALID_EMP_TYPE);
                }//End Switch
            }//End for loop

        }
    }
}
