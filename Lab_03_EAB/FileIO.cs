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
        #region CONSTANTS
        private const string FILTER_STR = "Database (.db)|*.db";
        private const string FILE_DIAG_TITLE = "Choose file location";
        private const string BAD_PATH_MESSAGE = "Provided path did not exist.";
        #endregion
        #region MemberFields and Properties
        static private BinaryFormatter serializer = new BinaryFormatter();
        private FileStream stream;
        private BusinessRules myBusinessRules;
        private SortedDictionary<uint, Employee> employeeDB = new SortedDictionary<uint, Employee>();
        public SortedDictionary<uint, Employee> EmployeeDB
        {
            get => employeeDB;
            set => employeeDB = value;
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Creates an instance of FileIO that will need to be given an BusinessRules object before it can write.
        /// </summary>
        public FileIO()
        {

        }
        /// <summary>
        /// Creates an instance of FileIO that has already copied the BusinessRules data
        /// Instances created this way do not need to have an object specified during its method calls.
        /// </summary>
        /// <param name="businessRules">The BusinessRules object to copy from</param>
        public FileIO(BusinessRules businessRules)
        {
            myBusinessRules = businessRules;
            businessRules?.CopyTo(employeeDB);
            if(File.Exists(businessRules?.FilePath))
            {
                stream = File.Open(businessRules.FilePath, FileMode.Open, FileAccess.ReadWrite);
            }
        }
        #endregion
        #region Saving and Writing
        /// <summary>
        /// Acquires a valid path to save to and writes to it.
        /// </summary>
        /// <param name="business">The object to write. Not necessary if one was provided in the constructor.</param>
        /// <param name="path">The desired path to save to. If one is not provided, a dialogue window will prompt the user for one.</param>
        public void SaveFileDB(string path = null, BusinessRules business = null)
        {
            /*
             * Psuedocode:
             * Create a dialog if:
             *      path is empty or path is bad
             *      AND
             *      business is null or business path is empty or bad or business path is equal to a stream that is already open
             * If the user cancels: stop
             * If the user continues: acquire the path
             * Open the file with the first good path found
             * Write to the file
             *      If business is not null, then write its data
             *      else, write my own data
             */
             //Acquire a valid path and set the stream to it
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                if (business == null || string.IsNullOrEmpty(business.FilePath) || !Directory.Exists(business.FilePath))
                {
                    //If we get here, we know that we were not provided with a valid path
                    SaveFileDialog save = new SaveFileDialog()
                    {
                        CheckFileExists = true,
                        CheckPathExists = true,
                        OverwritePrompt = true,
                        Title = FILE_DIAG_TITLE,
                        Filter = FILTER_STR
                    };
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        stream?.Close();
                        stream = File.Open(save.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    }
                    else
                        return;
                }
                else //If we get here, we know that business has a valid path
                {
                    if (stream?.Name != business.FilePath)
                    {
                        stream?.Close();
                        stream = File.Open(business.FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    }
                }
            }
            else //If we get here, we know that path is valid
            {
                if(stream?.Name != path)
                {
                    stream?.Close();
                    stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }
            }
            //If we were given an object to save, we overwrite our copy of data.
            business?.CopyTo(employeeDB);
            //Now that we have a path and our data, we will write the file to stream.
            WriteFileDB(business);
        }
        /// <summary>
        /// Attempts to write to stream
        /// If the FileIO instance was instantiated with a BusinessRules object, that will be saved.
        /// <preconditions>Stream was opened for modification.
        ///     Stream is in a valid state to write.</preconditions>
        /// </summary>
        public void WriteFileDB()
        {
            if (stream == null) return;
            using (GZipStream gzip = new GZipStream(stream, CompressionLevel.Optimal, true))
            {
                serializer.Serialize(stream, employeeDB);
            }
            if(myBusinessRules != null)
            {
                myBusinessRules = new BusinessRules(myBusinessRules.EmployeeCollection, stream.Name);
            }
        }
        /// <summary>
        /// Attempts to write the given object to stream.
        /// <preconditions>Stream was opened for modification.
        ///     Stream must be in a valid state to write.</preconditions>
        /// </summary>
        /// <param name="toWrite">The object to write to file</param>
        public void WriteFileDB(BusinessRules toWrite)
        {
            if (toWrite == null) return;
            toWrite.CopyTo(employeeDB);
            WriteFileDB();
            toWrite = new BusinessRules(toWrite.EmployeeCollection, stream.Name);
        }
        #endregion
        #region Reading and Opening
        /// <summary>
        /// Attempts to open a file and read it.
        /// </summary>
        public void OpenFileDB()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Title = FILE_DIAG_TITLE,
                Filter = FILTER_STR
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                stream?.Close();
                stream = File.Open(openFileDialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
        }
        /// <summary>
        /// Attempts to open a file and read the data into the provided BusinessRules object.
        /// </summary>
        /// <param name="business">The object that will be read into.</param>
        public void OpenFileDB(string path = null, BusinessRules businessRules = null)
        {
            /*
             * Psuedocode:
             * Call OpenFileDB() only if:
             *      path is nullorempty, file doesn't exist
             *      AND
             *      businessRules is either:
             *          null
             *          FileName is nullorempty
             *          FileName is bad
             * Once a stream is secured, we read into:
             *      the business object if it is not null
             *      ourselves otherwise
             */
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                if(businessRules == null || string.IsNullOrEmpty(businessRules.FilePath) || !File.Exists(businessRules.FilePath))
                {
                    //if we get here, it means we do not have a valid path
                    OpenFileDB();
                }
                else
                {
                    //if we get here, it means our object has a good path
                    if(stream?.Name != businessRules.FilePath)
                    {
                        stream?.Close();
                        stream = File.Open(businessRules.FilePath, FileMode.Open, FileAccess.ReadWrite);
                    }
                }
            }
            else
            {
                //if we get here, it means our path was valid
                if(stream?.Name != path)
                {
                    stream?.Close();
                    stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite);
                }
            }
            ReadFileDB();
            businessRules = new BusinessRules(employeeDB, stream.Name);
        }
        /// <summary>
        /// Reads a file that the stream is currently pointing at
        /// If the FileIO instance was instantiated with a BusinessRules object, that object will be read into.
        /// </summary>
        public void ReadFileDB()
        {
            if (stream == null) return;
            using (GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress, true))
            {
                employeeDB = (SortedDictionary<uint, Employee>)serializer.Deserialize(stream);
            }
            if(myBusinessRules != null)
            {
                myBusinessRules = new BusinessRules(employeeDB, stream.Name);
            }
        }
        /// <summary>
        /// Attempts to point the stream to the path provided and reads a file from it.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="business"></param>
        public void ReadFileDB(string path = null, BusinessRules business = null)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                if (business == null || string.IsNullOrEmpty(business.FilePath) || !File.Exists(business.FilePath))
                {
                    //if we get here we have no good path
                    throw new ArgumentException(BAD_PATH_MESSAGE);
                }
                else
                {
                    //if we get here, business has a good path
                    if (stream?.Name != business.FilePath)
                    {
                        stream?.Close();
                        stream = File.Open(business.FilePath, FileMode.Open, FileAccess.ReadWrite);
                    }
                }
            }
            else
            {
                //if we get here, path is good
                if (stream?.Name != path)
                {
                    stream?.Close();
                    stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite);
                }
            }
            ReadFileDB();
            business = new BusinessRules(employeeDB, stream.Name);
        }
        #endregion
        #region Methods
        /// <summary>
        /// Closes a file handle if there is one.
        /// </summary>
        public void CloseFileDB()
        {
            stream?.Close();
        }
        #endregion
    }//End FileIO
}//End Namespace