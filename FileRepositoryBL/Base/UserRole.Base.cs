
  
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
    public partial class UserRole : Entity<UserRole>
    {
        public static string TableName { get { return "UserRole"; } }

        #region "static ctor"

        static UserRole()
        {
            AppDb oAppdb = new AppDb();
            // UserRole.Init(oAppdb, oAppdb.TablePrefix + TableName);
            List<ReferenceTable> _ReferenceTables = UserRole.GetReferenceTables(oAppdb.TablePrefix);
            UserRole.Init(oAppdb, oAppdb.TablePrefix + TableName, "UserRoleID", _ReferenceTables);
        }

        #endregion

        #region "Public Propeties"

        //Actual Properties from Table
        private Int32? _UserRoleID;
        public Int32? UserRoleID { get { return _UserRoleID; } set { SetProperty("UserRoleID", ref _UserRoleID, value); } }    //**PK

        private Int32? _UserID;
        public Int32? UserID { get { return _UserID; } set { SetProperty("UserID", ref _UserID, value); } }
        private Int32? _RoleID;
        public Int32? RoleID { get { return _RoleID; } set { SetProperty("RoleID", ref _RoleID, value); } }
        

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
            
            UserRole.GetCustomReferenceTables(referenceTables, tablePrefix);
            return referenceTables;
        }
        #endregion


        //Following code is Commented 
        //Please note it is exists in the Template


    }
}
