
  
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
    public partial class Document : Entity<Document>
    {
        public static string TableName { get { return "Document"; } }

        #region "static ctor"

        static Document()
        {
            AppDb oAppdb = new AppDb();
            // Document.Init(oAppdb, oAppdb.TablePrefix + TableName);
            List<ReferenceTable> _ReferenceTables = Document.GetReferenceTables(oAppdb.TablePrefix);
            Document.Init(oAppdb, oAppdb.TablePrefix + TableName, "DocumentID", _ReferenceTables);
        }

        #endregion

        #region "Public Propeties"

        //Actual Properties from Table
        private Int32? _DocumentID;
        public Int32? DocumentID { get { return _DocumentID; } set { SetProperty("DocumentID", ref _DocumentID, value); } }    //**PK
        private string _FileName;
        public string FileName { get { return _FileName; } set { SetProperty("FileName", ref _FileName, value); } }
        private string _FileDescr;
        public string FileDescr { get { return _FileDescr; } set { SetProperty("FileDescr", ref _FileDescr, value); } }
        private Int32? _version;
        public Int32? version { get { return _version; } set { SetProperty("version", ref _version, value); } }
        private string _Extension;
        public string Extension { get { return _Extension; } set { SetProperty("Extension", ref _Extension, value); } }
        private string _FilePath;
        public string FilePath { get { return _FilePath; } set { SetProperty("FilePath", ref _FilePath, value); } }
        private Int32? _FileSrl;
        public Int32? FileSrl { get { return _FileSrl; } set { SetProperty("FileSrl", ref _FileSrl, value); } }
        private DateTime? _ValidFrom;
        public DateTime? ValidFrom { get { return _ValidFrom; } set { SetProperty("ValidFrom", ref _ValidFrom, value); } }
        private DateTime? _ValidTo;
        public DateTime? ValidTo { get { return _ValidTo; } set { SetProperty("ValidTo", ref _ValidTo, value); } }
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
        private Int32? _NotificationDays;
        public Int32? NotificationDays { get { return _NotificationDays; } set { SetProperty("NotificationDays", ref _NotificationDays, value); } }
        private string _RefTableID;
        public string RefTableID { get { return _RefTableID; } set { SetProperty("RefTableID", ref _RefTableID, value); } }
        
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
            
            Document.GetCustomReferenceTables(referenceTables, tablePrefix);
            return referenceTables;
        }
        #endregion


        //Following code is Commented 
        //Please note it is exists in the Template
    }
}
