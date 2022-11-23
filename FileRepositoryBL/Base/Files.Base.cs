
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Arohan.Data;
using FileRepository.BusinessObjects;

namespace FileRepository.BusinessObjects
{
    public partial class Files : Entity<Files>
    {
        public static string TableName { get { return "Files"; } }

        #region "static ctor"

        static Files()
        {
            AppDb oAppdb = new AppDb();
            // Files.Init(oAppdb, oAppdb.TablePrefix + TableName);
            List<ReferenceTable> _ReferenceTables = Files.GetReferenceTables(oAppdb.TablePrefix);
            Files.Init(oAppdb, oAppdb.TablePrefix + TableName, "FilesID", _ReferenceTables);
        }

        #endregion

        #region "Public Propeties"

        //Actual Properties from Table
        private Int32? _FilesID;
        public Int32? FilesID { get { return _FilesID; } set { SetProperty("FilesID", ref _FilesID, value); } }    //**PK
        private Int32? _RepositoryID;
        public Int32? RepositoryID { get { return _RepositoryID; } set { SetProperty("RepositoryID", ref _RepositoryID, value); } }
        private string _FileName;
        public string FileName { get { return _FileName; } set { SetProperty("FileName", ref _FileName, value); } }
        private string _FileDescr;
        public string FileDescr { get { return _FileDescr; } set { SetProperty("FileDescr", ref _FileDescr, value); } }       
        private string _Extension;
        public string Extension { get { return _Extension; } set { SetProperty("Extension", ref _Extension, value); } }
        private string _FilePath;
        public string FilePath { get { return _FilePath; } set { SetProperty("FilePath", ref _FilePath, value); } }
        private Int32? _FileSrl;
        public Int32? FileSrl { get { return _FileSrl; } set { SetProperty("FileSrl", ref _FileSrl, value); } }     
        private string _IsDelete;
        public string IsDelete { get { return _IsDelete; } set { SetProperty("IsDelete", ref _IsDelete, value); } }
        private Int32? _CreatedBy;
        public Int32? CreatedBy { get { return _CreatedBy; } set { SetProperty("CreatedBy", ref _CreatedBy, value); } }
        private DateTime? _CreatedOn;
        public DateTime? CreatedOn { get { return _CreatedOn; } set { SetProperty("CreatedOn", ref _CreatedOn, value); } }
        private Int32? _UpdtedBy;
        public Int32? UpdtedBy { get { return _UpdtedBy; } set { SetProperty("UpdtedBy", ref _UpdtedBy, value); } }
        private DateTime? _UpdatedOn;
        public DateTime? UpdatedOn { get { return _UpdatedOn; } set { SetProperty("UpdatedOn", ref _UpdatedOn, value); } }
        
        // Required for Select2 Objects
        // public string Select2Text { get; set; }

        #endregion

        #region "Additional FK Properties if any"

        #endregion

        #region "Child Properties if any"

        //Child Class Properties if any.


        #endregion

        #region "Reference Table Definitions"
        private static List<ReferenceTable> GetReferenceTables(string tablePrefix)
        {
            List<ReferenceTable> referenceTables = new List<ReferenceTable>();
            //// These Definitions will be used to Inner Join with reference tables.
            
            Files.GetCustomReferenceTables(referenceTables, tablePrefix);
            return referenceTables;
        }
        #endregion


        //Following code is Commented 
        //Please note it is exists in the Template
    }
}
