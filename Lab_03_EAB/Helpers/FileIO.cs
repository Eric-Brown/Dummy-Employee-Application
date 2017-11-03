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
    public class FileIO : IFileAccess, IDisposable
    {
        #region CONSTANTS
        private const string FILTER_STR = "Database (.db)|*.db";
        private const string FILE_DIAG_TITLE = "Choose file location";
        private const string BAD_PATH_MESSAGE = "Provided path did not exist.";
        #endregion
        #region MemberFields and Properties
        private static readonly DataContractSerializer serializer = new DataContractSerializer(typeof(SortedDictionary<uint,Employee>));
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
        public void OpenSaveFileDB()
        {
            SaveFileDialog saveFileDialog = GetSaveDialog();
            if (saveFileDialog == null) return;
        }
        /// <summary>
        /// Attempts to write to stream
        /// If the FileIO instance was instantiated with a BusinessRules object, that will be saved.
        /// <preconditions>Stream was opened for modification.
        ///     Stream is in a valid state to write.</preconditions>
        /// </summary>
        public void WriteFileDB()
        {
            if (stream == null || myBusinessRules == null)
            {
                return;
            }
            using (GZipStream gzip = new GZipStream(stream, CompressionLevel.Optimal, true))
            using (var binWriter = XmlDictionaryWriter.CreateBinaryWriter(gzip))
            {
                serializer.WriteObject(binWriter, employeeDB);
            }
            myBusinessRules.EmployeeCollection = employeeDB;
            myBusinessRules.FilePath = stream.Name;
        }
        /// <summary>
        /// Attempts to write the given object to stream.
        /// <preconditions>Stream was opened for modification.
        ///     Stream must be in a valid state to write.</preconditions>
        /// </summary>
        /// <param name="toWrite">The object to write to file</param>
        public void WriteFileDB(string newPath)
        {
            if (string.IsNullOrEmpty(newPath) || !Directory.Exists(newPath)) throw new FileNotFoundException();
            if(stream?.Name != newPath)
            {
                stream?.Close();
                stream = File.Open(newPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            WriteFileDB();
        }
        #endregion
        #region Reading and Opening
        /// <summary>
        /// Attempts to open a file
        /// </summary>
        public void OpenFileDB()
        {
            OpenFileDialog openFileDialog = GetOpenFileDialog();
            if (openFileDialog == null) return;
        }
        /// <summary>
        /// Attempts to open a file
        /// </summary>
        /// <param name="newPath"></param>
        public void OpenFileDB(string newPath)
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
            if (string.IsNullOrEmpty(newPath) || !File.Exists(newPath))
            {
                throw new ArgumentException(BAD_PATH_MESSAGE);
            }
            if(stream?.Name != newPath)
            {
                stream?.Close();
                stream = File.Open(newPath, FileMode.Open, FileAccess.ReadWrite);
            }
        }
        /// <summary>
        /// Reads a file that the stream is currently pointing at
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown if read is attempted while stream is not valid.</exception>
        public void ReadFileDB()
        {
            if (stream == null || !File.Exists(stream?.Name))
            {
                return;
            }
            using (GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress, true))
                using(var binReader = XmlDictionaryReader.CreateBinaryReader(gzip, new XmlDictionaryReaderQuotas()))
            {
                employeeDB = (SortedDictionary<uint, Employee>)serializer.ReadObject(binReader);
            }
            myBusinessRules.EmployeeCollection = employeeDB;
            myBusinessRules.FilePath = stream.Name;
        }
        /// <summary>
        /// Attempts to point the stream to the path provided and reads a file from it.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="business"></param>
        public void ReadFileDB(string newPath)
        {
            if (string.IsNullOrEmpty(newPath) || !File.Exists(newPath))
            {
                throw new FileNotFoundException();
            }
            if(stream?.Name != newPath)
            {
                stream?.Close();
                stream = File.Open(newPath, FileMode.Open, FileAccess.ReadWrite);
            }
            ReadFileDB();
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

        public void Dispose()
        {
            ((IDisposable)stream)?.Dispose();
        }
        private SaveFileDialog GetSaveDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                CheckPathExists = true,
                Title = FILE_DIAG_TITLE,
                Filter = FILTER_STR
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                stream?.Close();
                stream = File.Open(dialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                return dialog;
            }
            else return null;
        }
        private OpenFileDialog GetOpenFileDialog()
        {
            OpenFileDialog toReturn = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Title = FILE_DIAG_TITLE,
                Filter = FILTER_STR
            };
            if (toReturn.ShowDialog() == DialogResult.OK)
            {
                stream?.Close();
                stream = File.Open(toReturn.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                return toReturn;
            }
            return null;
        }
        #endregion
    }//End FileIO
}//End Namespace