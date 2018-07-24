using System.Collections.Generic;

namespace Lab_03_EAB
{
    public interface IFileAccess
    {
        void WriteFileDB();

        void ReadFileDB();

        void OpenFileDB();

        void CloseFileDB();

        SortedDictionary<uint, Employee> EmployeeDB { get; set; }
    }
}