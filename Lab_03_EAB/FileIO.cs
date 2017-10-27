using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Windows.Forms;

namespace Lab_03_EAB
{
    /// <summary>
    /// Helper class that handles IO requests for .db files for employee dictionaries.
    /// </summary>
    [Serializable]
    public class FileIO : IFileAccess
    {
        private const string FILTER_STR = "Database (.db)|*.db";
        BinaryFormatter serializer = new BinaryFormatter();
        public FileStream stream;
        /// <summary>
        /// Constructor that copies the values from a businessrules object
        /// </summary>
        /// <param name="rules"></param>
        public FileIO(BusinessRules rules)
        {
            rules.CopyTo(employeeDB);
        }

        
        private SortedDictionary<uint, Employee> employeeDB = new SortedDictionary<uint, Employee>();
        public SortedDictionary<uint, Employee> EmployeeDB
        {
            get => employeeDB;
            set => employeeDB = value;
        }
        /// <summary>
        /// Closes a file handle if there is one.
        /// </summary>
        public void CloseFileDB()
        {
            stream?.Close();
        }
        /// <summary>
        /// Brings up a save file dialogue and sets the FileStream to that file
        /// </summary>
        public void SaveFileDB()
        {
            SaveFileDialog save = new SaveFileDialog()
            {
                Filter = FILTER_STR
            };
            if(save.ShowDialog() == DialogResult.OK)
            {
                stream?.Close();
                stream = File.Open(save.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
        }
        /// <summary>
        /// Opens a file dialogue and attempts to open the file pointed at.
        /// </summary>
        public void OpenFileDB()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = FILTER_STR
            };
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                stream?.Close();
                stream = File.Open(openFileDialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            }
        }
        /// <summary>
        /// Reads a file that the stream is currently pointing at
        /// </summary>
        public void ReadFileDB()
        {
            if (stream == null) return;
            using (GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress, true))
            {
                employeeDB = (SortedDictionary<uint, Employee>)serializer.Deserialize(stream);
            }
        }
        /// <summary>
        /// Attempts to point the stream to the path provided and reads a file from it.
        /// </summary>
        /// <param name="path"></param>
        public void ReadFileDB(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            stream?.Close();
            stream = File.Open(path, FileMode.OpenOrCreate);
            ReadFileDB();
        }
        /// <summary>
        /// Attempts to write the collection to the filestream
        /// </summary>
        public void WriteFileDB()
        {
            if (stream == null) return;
            using (GZipStream gzip = new GZipStream(stream, CompressionLevel.Optimal, true))
            {
                serializer.Serialize(stream, employeeDB);
            }
        }
        /// <summary>
        /// attempts to change the filestream to the path and then attempts to write to that stream
        /// </summary>
        /// <param name="path"></param>
        public void WriteFileDB(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            stream?.Close();
            stream = File.Open(path, FileMode.OpenOrCreate);
            WriteFileDB();
        }
    }
}