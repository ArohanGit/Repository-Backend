
  
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
    public partial class User : Entity<User>
    {
        public static string TableName { get { return "User"; } }

        #region "static ctor"

        static User()
        {
            AppDb oAppdb = new AppDb();
            // User.Init(oAppdb, oAppdb.TablePrefix + TableName);
            List<ReferenceTable> _ReferenceTables = User.GetReferenceTables(oAppdb.TablePrefix);
            User.Init(oAppdb, oAppdb.TablePrefix + TableName, "UserID", _ReferenceTables);
        }

        #endregion

        #region "Public Propeties"

        //Actual Properties from Table
        private Int32? _UserID;
        public Int32? UserID { get { return _UserID; } set { SetProperty("UserID", ref _UserID, value); } }    //**PK
        private string _Code;
        public string Code { get { return _Code; } set { SetProperty("Code", ref _Code, value); } }
        private string _Name;
        public string Name { get { return _Name; } set { SetProperty("Name", ref _Name, value); } }
        private string _IsLeft;
        public string IsLeft { get { return _IsLeft; } set { SetProperty("IsLeft", ref _IsLeft, value); } }
        private string _EMailAddress;
        public string EMailAddress { get { return _EMailAddress; } set { SetProperty("EMailAddress", ref _EMailAddress, value); } }
        private string _WebUserID;
        public string WebUserID { get { return _WebUserID; } set { SetProperty("WebUserID", ref _WebUserID, value); } }
        private string _password;
        public string password { get { return _password; } set { SetProperty("password", ref _password, value); } }
        
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
            
            User.GetCustomReferenceTables(referenceTables, tablePrefix);
            return referenceTables;
        }
        #endregion


        //Following code is Commented 
        //Please note it is exists in the Template


    }
}
