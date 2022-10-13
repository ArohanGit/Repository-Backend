
  
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
    public partial class Role : Entity<Role>
    {
        public static string TableName { get { return "Role"; } }

        #region "static ctor"

        static Role()
        {
            AppDb oAppdb = new AppDb();
            // Role.Init(oAppdb, oAppdb.TablePrefix + TableName);
            List<ReferenceTable> _ReferenceTables = Role.GetReferenceTables(oAppdb.TablePrefix);
            Role.Init(oAppdb, oAppdb.TablePrefix + TableName, "RoleID", _ReferenceTables);
        }

        #endregion

        #region "Public Propeties"

        //Actual Properties from Table
        private Int32? _RoleID;
        public Int32? RoleID { get { return _RoleID; } set { SetProperty("RoleID", ref _RoleID, value); } }    //**PK

        private string _Name;
        public string Name { get { return _Name; } set { SetProperty("Name", ref _Name, value); } }

        private string _ShowGSTChallan;
        public string ShowGSTChallan { get { return _ShowGSTChallan; } set { SetProperty("ShowGSTChallan", ref _ShowGSTChallan, value); } }

        private string _OnApprovalSaveChallan;
        public string OnApprovalSaveChallan { get { return _OnApprovalSaveChallan; } set { SetProperty("OnApprovalSaveChallan", ref _OnApprovalSaveChallan, value); } }

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
            
            Role.GetCustomReferenceTables(referenceTables, tablePrefix);
            return referenceTables;
        }
        #endregion


        //Following code is Commented 
        //Please note it is exists in the Template


    }
}
