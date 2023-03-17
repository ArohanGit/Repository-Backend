

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
    public partial class Repository : Entity<Repository>
    {
        public static string TableName { get { return "Repository"; } }

        #region "static ctor"

        static Repository()
        {
            AppDb oAppdb = new AppDb();
            // Repository.Init(oAppdb, oAppdb.TablePrefix + TableName);
            List<ReferenceTable> _ReferenceTables = Repository.GetReferenceTables(oAppdb.TablePrefix);
            Repository.Init(oAppdb, oAppdb.TablePrefix + TableName, "RepositoryID", _ReferenceTables);
        }

        #endregion

        #region "Public Propeties"

        //Actual Properties from Table
        private Int32? _RepositoryID;
        public Int32? RepositoryID { get { return _RepositoryID; } set { SetProperty("RepositoryID", ref _RepositoryID, value); } }    //**PK
        private string _RepositoryName;
        public string RepositoryName { get { return _RepositoryName; } set { SetProperty("RepositoryName", ref _RepositoryName, value); } }
        private string _RepositoryDescr;
        public string RepositoryDescr { get { return _RepositoryDescr; } set { SetProperty("RepositoryDescr", ref _RepositoryDescr, value); } }
        private Int32? _version;
        public Int32? version { get { return _version; } set { SetProperty("version", ref _version, value); } }
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
        private string _IsApproved;
        public string IsApproved { get { return _IsApproved; } set { SetProperty("IsApproved", ref _IsApproved, value); } }
        private Int32? _ApprovalLevel;
        public Int32? ApprovalLevel { get { return _ApprovalLevel; } set { SetProperty("ApprovalLevel", ref _ApprovalLevel, value); } }
        private string _RejectRemark;
        public string RejectRemark { get { return _RejectRemark; } set { SetProperty("RejectRemark", ref _RejectRemark, value); } }

        private Int32? _DepartmentID;
        public Int32? DepartmentID { get { return _DepartmentID; } set { SetProperty("DepartmentID", ref _DepartmentID, value); } }
        private string _RepositoryNo;
        public string RepositoryNo { get { return _RepositoryNo; } set { SetProperty("RepositoryNo", ref _RepositoryNo, value); } }
        private string _Remark;
        public string Remark { get { return _Remark; } set { SetProperty("Remark", ref _Remark, value); } }
        private string _RepoDuration;
        public string RepoDuration { get { return _RepoDuration; } set { SetProperty("RepoDuration", ref _RepoDuration, value); } }
        
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

            Repository.GetCustomReferenceTables(referenceTables, tablePrefix);
            return referenceTables;
        }
        #endregion


        //Following code is Commented 
        //Please note it is exists in the Template
    }
}
