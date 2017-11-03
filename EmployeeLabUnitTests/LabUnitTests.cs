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
using System.Linq;
using System.IO;
using System.Windows;
using Lab_03_EAB;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace EmployeeLabUnitTests
{
    [DataContract]
    public class Dummy
    {
        public Dummy() { }
        [DataMember]
       public SortedDictionary<uint, int> greg { get; set; }
    }
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
            Sales testSalesEmp = new Sales(0, FIRST, LAST, setMonthlySalary, setCommission, setGrossSales);
            string expected = string.Format(BASE_FORMAT_STRING + SALARY_FORMAT_STRING + SALES_FORMAT_STRING, 0, testSalesEmp.EmpType, FIRST, LAST, setMonthlySalary, setCommission, setGrossSales);
            string actual = testSalesEmp.ToString();
            Assert.AreEqual(expected, testSalesEmp.ToString());
        }
        [TestMethod]
        public void TestSalaryToString()
        {
            Salary testSalaryEmp = new Salary(0, FIRST, LAST, setMonthlySalary);
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
        [TestMethod]
        public void TestSerializing()
        {
            var ds = new DataContractSerializer(typeof(Employee));
            const string First = "Sam";
            const string Last = "Iam";
            const decimal DummyDecimal = 10.10m;
            const double DummyDouble = 10.0;
            uint ID = 0;
            Contract toTest = new Contract(ID++, First, Last, DummyDecimal);
            var s = new MemoryStream();
            ds.WriteObject(s, toTest);
            Salary a = new Salary(ID++, First, Last, DummyDecimal);
            ds.WriteObject(s, a);
            Hourly b = new Hourly(ID++, First, Last, DummyDecimal, DummyDouble);
            ds.WriteObject(s, b);
            Sales c = new Sales(ID++, First, Last, DummyDecimal, DummyDecimal, DummyDecimal);
            ds.WriteObject(s, c);
        }
        [TestMethod]
        public void TestDeserializing()
        {
            var ds = new DataContractSerializer(typeof(Employee));
            const string First = "Sam";
            const string Last = "Iam";
            const decimal DummyDecimal = 10.10m;
            const double DummyDouble = 10.0;
            uint ID = 0;
            Contract toTest = new Contract(ID++, First, Last, DummyDecimal);
            var s = new MemoryStream();
            ds.WriteObject(s, toTest);
            s.Seek(0, SeekOrigin.Begin); //Set stream to beginning
            Contract readToTest = (Contract)ds.ReadObject(s);
            Assert.IsTrue(readToTest.EmpID == toTest.EmpID
                && readToTest.ContractWage == toTest.ContractWage
                && readToTest.EmpType == toTest.EmpType
                && readToTest.FirstName == toTest.FirstName
                && readToTest.LastName == toTest.LastName);
            Salary a = new Salary(ID++, First, Last, DummyDecimal);
            s.SetLength(0);
            ds.WriteObject(s, a);
            s.Seek(0, SeekOrigin.Begin); //Set stream to beginning
            Salary reada = (Salary)ds.ReadObject(s);
            Assert.IsTrue(reada.EmpID == a.EmpID
    && reada.MonthlySalary == a.MonthlySalary
    && reada.EmpType == a.EmpType
    && reada.FirstName == a.FirstName
    && reada.LastName == a.LastName);
            Hourly b = new Hourly(ID++, First, Last, DummyDecimal, DummyDouble);
            s.SetLength(0);
            ds.WriteObject(s, b);
            s.Seek(0, SeekOrigin.Begin); //Set stream to beginning
            Hourly readb = (Hourly)ds.ReadObject(s);
            Assert.IsTrue(readb.EmpID == b.EmpID
    && readb.HoursWorked == b.HoursWorked
    && readb.EmpType == b.EmpType
    && readb.FirstName == b.FirstName
    && readb.LastName == b.LastName
    && readb.HourlyRate == b.HourlyRate);
            Sales c = new Sales(ID++, First, Last, DummyDecimal, DummyDecimal, DummyDecimal);
            s.SetLength(0);
            ds.WriteObject(s, c);
            s.Seek(0, SeekOrigin.Begin); //Set stream to beginning
            Sales readc = (Sales)ds.ReadObject(s);
            Assert.IsTrue(readc.EmpID == c.EmpID
    && readc.MonthlySalary == c.MonthlySalary
    && readc.EmpType == c.EmpType
    && readc.FirstName == c.FirstName
    && readc.LastName == c.LastName
    && readc.GrossSales == c.GrossSales
    && readc.Commission == c.Commission);

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
            Assert.AreEqual(null, toTest[0]);
            Assert.AreEqual(null, toTest[int.MinValue]);
            PopulateBusinessRules(toTest);
            Assert.AreNotEqual(null, toTest[0]);
            Assert.AreNotEqual(toTest[0], toTest[DEFAULT_EMPS_TO_CREATE - 1]);
            BusinessRules orderTest = new BusinessRules();
            //Testing if order is preserved for the object.
            for (int i = DEFAULT_EMPS_TO_CREATE - 1; i >= 0; --i)
                orderTest.Add(toTest[i]);
            for (int i = 0; i < DEFAULT_EMPS_TO_CREATE; i++)
                Assert.AreEqual(orderTest[i], toTest[i]);
            Employee employee = toTest[0];
            toTest[0] = null;
            Assert.IsFalse(toTest.Contains(employee));
        }
        /// <summary>
        /// Tests the .Count() and .Clear() methods in BusinessRules
        /// </summary>
        [TestMethod]
        public void TestCountAndClear()
        {
            BusinessRules toTest = new BusinessRules();
            Assert.AreEqual(0, toTest.Count());
            PopulateBusinessRules(toTest);
            Assert.AreEqual(DEFAULT_EMPS_TO_CREATE, toTest.Count());
            PopulateBusinessRules(toTest, DEFAULT_EMPS_TO_CREATE * 2);
            Assert.AreEqual(DEFAULT_EMPS_TO_CREATE * 2, toTest.Count());
            toTest.Clear();
            Assert.AreEqual(0, toTest.Count());
            Assert.AreEqual(null, toTest[0]);
            toTest.Clear();
            Assert.AreEqual(0, toTest.Count());
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
            toTest.Add(toAdd);
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
            foreach (Employee foo in toTest)
            {
                foo.EmpID = 0;
            }
            Assert.AreEqual(1, toTest.Count());
        }
        [TestMethod]
        public void TestCanAddFromArrray()
        {
            BusinessRules toTest = new BusinessRules();
            uint testID = 0;
            Random random = new Random();
            ETYPE knownType = ETYPE.CONTRACT;
            string[] goodArray =
            {
                testID.ToString(),
                FIRST_NAMES[random.Next(0,FIRST_NAMES.Count() - 1)],
                LAST_NAMES[random.Next(0,LAST_NAMES.Count() -1 )],
                (random.NextDouble() * DEFAULT_EMPS_TO_CREATE).ToString()
            };
            Assert.IsTrue(toTest.CanAddFromStringArray(knownType, goodArray));
            string[] badArray = new string[goodArray.Count()];
            goodArray.CopyTo(badArray, 0);
            //Set first name to two words
            badArray[1] = badArray[1] + " " + badArray[1];
            Assert.IsFalse(toTest.CanAddFromStringArray(knownType, badArray));
            //reset bad array
            goodArray.CopyTo(badArray, 0);
            //add letters to something that should only be a number
            badArray[badArray.Count() - 1] = badArray[badArray.Count() - 1] + badArray[1];
            Assert.IsFalse(toTest.CanAddFromStringArray(knownType, badArray));
        }
        [TestMethod]
        public void TestAddFromStringArray()
        {

            BusinessRules toTest = new BusinessRules();
            uint testID = 0;
            Random random = new Random();
            ETYPE knownType = ETYPE.CONTRACT;
            string[] goodArray =
            {
                testID.ToString(),
                FIRST_NAMES[random.Next(0,FIRST_NAMES.Count() - 1)],
                LAST_NAMES[random.Next(0,LAST_NAMES.Count() -1 )],
                (random.NextDouble() * DEFAULT_EMPS_TO_CREATE).ToString()
            };
            Assert.IsTrue(toTest.AddFromStringArray(knownType, goodArray));
            Assert.IsTrue(toTest.Count() > 0);
            string[] badArray = new string[goodArray.Count()];
            goodArray.CopyTo(badArray, 0);
            //Set first name to two words
            badArray[1] = badArray[1] + " " + badArray[1];
            Assert.IsFalse(toTest.AddFromStringArray(knownType, badArray));
            Assert.IsFalse(toTest.Count() > 1);
            //reset bad array
            goodArray.CopyTo(badArray, 0);
            //add letters to something that should only be a number
            badArray[badArray.Count() - 1] = badArray[badArray.Count() - 1] + badArray[1];
            Assert.IsFalse(toTest.AddFromStringArray(knownType, badArray));
            Assert.IsFalse(toTest.Count() > 1);
        }
        [TestMethod]
        public void TestAddandRemove()
        {
            BusinessRules a = new BusinessRules();
            BusinessRules b = new BusinessRules();
            PopulateBusinessRules(b);
            foreach(var emp in b)
            {
                a.Add(emp);
            }
            Assert.AreEqual(b.Count(), a.Count());
            foreach(var emp in b)
            {
                a.Remove(emp);
            }
            Assert.AreEqual(0, a.Count());
        }
        [TestMethod]
        public void TestContains()
        {
            Contract toAdd = new Contract(0, "Sam", "Iam", 10.00m);
            BusinessRules a = new BusinessRules();
            Assert.IsFalse(a.Contains(toAdd));
            a.Add(toAdd);
            Assert.IsTrue(a.Contains(toAdd));
        }
        [TestMethod]
        public void TestSerializing()
        {
            BusinessRules a = new BusinessRules();
            PopulateBusinessRules(a);
            MemoryStream s = new MemoryStream();
            DataContractSerializer greg = new DataContractSerializer(typeof(BusinessRules));
            greg.WriteObject(s, a);
        }
        [TestMethod]
        public void TestDeserializing()
        {
            BusinessRules a = new BusinessRules();
            PopulateBusinessRules(a);
            MemoryStream s = new MemoryStream();
            DataContractSerializer greg = new DataContractSerializer(typeof(BusinessRules));
            greg.WriteObject(s, a);
            s.Seek(0, SeekOrigin.Begin);
            BusinessRules b = (BusinessRules)greg.ReadObject(s);
            Assert.IsTrue(b.Equals(a));
        }

    [TestMethod]
        public void TestCopyTo()
        {
            BusinessRules a = new BusinessRules();
            PopulateBusinessRules(a);
            Employee[] employee = new Employee[DEFAULT_EMPS_TO_CREATE];
            a.CopyTo(employee, 0);
            int index = 0;
            foreach (var emp in a)
            {
                Assert.IsTrue(a.Contains(employee[index]));
                index++;
            }
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
                        toPopulate.Add(new Contract((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.HOURLY:
                        toPopulate.Add(new Hourly((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next()),
                            random.NextDouble() * random.Next()));
                        break;
                    case ETYPE.SALARY:
                        toPopulate.Add(new Salary((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.SALES:
                        toPopulate.Add(new Sales((uint)i,
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
    }//End Test BusinessRules

    [TestClass]
    public class TestFileIO
    {
        private readonly string[] FIRST_NAMES = { "Stewart", "Sunny", "Grant", "Greg", "Micheal", "Seth", "Anthony", "Matthew", "Jonathon", "Jenny", "Sam" };
        private readonly string[] LAST_NAMES = { "Linder", "Brown", "DePoirot", "Johnson", "Williams", "Xavier", "Green", "Goldberg", "Greenburg", "Flotsam", "Jenkins", "Jensen", "Null" };
        private const int DEFAULT_EMPS_TO_CREATE = 10;
        private const string INVALID_EMP_TYPE = "Invalid employee type attempted to be created.";
        private string testPath = Path.Combine(Directory.GetCurrentDirectory(), Path.GetTempFileName());
        [TestMethod]
        public void TestWriteFileDB()
        {
            BusinessRules businessRules = new BusinessRules();
            PopulateBusinessRules(businessRules);
            FileIO fileIO = new FileIO(businessRules);
            fileIO.WriteFileDB(testPath);
            fileIO.CloseFileDB();
            File.Exists(testPath);
            FileStream fileStream = File.Open(testPath, FileMode.Open);
            Assert.IsNotNull(fileStream);
            Assert.IsTrue(fileStream.Length > 0);
        }
        [TestMethod]
        public void TestReadFileDB()
        {
            BusinessRules businessRules = new BusinessRules();
            PopulateBusinessRules(businessRules);
            FileIO fileIO = new FileIO(businessRules);
            BusinessRules rules = new BusinessRules();
            fileIO.WriteFileDB(testPath);
            fileIO.CloseFileDB();
            FileIO test = new FileIO(rules);
            test.ReadFileDB(testPath);
            rules.EmployeeCollection = test.EmployeeDB;
            Assert.IsTrue(rules.EmployeeCollection.Count == businessRules.EmployeeCollection.Count);
        }
        [TestMethod]
        public void TestCloseDB()
        {
            BusinessRules business = new BusinessRules();
            PopulateBusinessRules(business);
            FileIO fileIO = new FileIO(business);
            fileIO.OpenFileDB();
            fileIO.CloseFileDB();
            //try
            //{
            //    fileIO.stream.Seek(1, SeekOrigin.Begin);
            //    Assert.Fail();
            //}
            //catch(ObjectDisposedException e)
            //{
            //    Assert.AreEqual("Cannot access a closed file.", e.Message);
            //}
            //catch(Exception e)
            //{
            //    Assert.Fail();
            //}
        }
        [TestMethod]
        public void TestOpenDB()
        {
            BusinessRules business = new BusinessRules();
            PopulateBusinessRules(business);
            FileIO fileIO = new FileIO(business);
            fileIO.OpenFileDB();
            //Assert.IsNotNull(fileIO.stream);
        }
        [TestMethod]
        public void TestSaveFileDB()
        {
            BusinessRules business = new BusinessRules();
            PopulateBusinessRules(business);
            FileIO fileIO = new FileIO(business);
            //fileIO.SaveFileDB();
            //Assert.IsNotNull(fileIO.stream);
            fileIO.CloseFileDB();
        }

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
                        toPopulate.Add(new Contract((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.HOURLY:
                        toPopulate.Add(new Hourly((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next()),
                            random.NextDouble() * random.Next()));
                        break;
                    case ETYPE.SALARY:
                        toPopulate.Add(new Salary((uint)i,
                            FIRST_NAMES.ElementAt(random.Next(0, FIRST_NAMES.Length - 1)),
                            LAST_NAMES.ElementAt(random.Next(0, LAST_NAMES.Length - 1)),
                            (decimal)(random.NextDouble() * random.Next())));
                        break;
                    case ETYPE.SALES:
                        toPopulate.Add(new Sales((uint)i,
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

    }//End TestFileIO Class
}
