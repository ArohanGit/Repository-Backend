
  
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
    public partial class NotificationTo : Entity<NotificationTo>
    {
        public static string TableName { get { return "NotificationTo"; } }

        #region "static ctor"

        static NotificationTo()
        {
            AppDb oAppdb = new AppDb();
            // NotificationTo.Init(oAppdb, oAppdb.TablePrefix + TableName);
            List<ReferenceTable> _ReferenceTables = NotificationTo.GetReferenceTables(oAppdb.TablePrefix);
            NotificationTo.Init(oAppdb, oAppdb.TablePrefix + TableName, "NotificationToID", _ReferenceTables);
        }

        #endregion

        #region "Public Propeties"

        //Actual Properties from Table
        private Int32? _NotificationToID;
        public Int32? NotificationToID { get { return _NotificationToID; } set { SetProperty("NotificationToID", ref _NotificationToID, value); } }    //**PK
        private Int32? _DocumentID;
        public Int32? DocumentID { get { return _DocumentID; } set { SetProperty("DocumentID", ref _DocumentID, value); } }
        private string _Email;
        public string Email { get { return _Email; } set { SetProperty("Email", ref _Email, value); } }
        
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
            
            NotificationTo.GetCustomReferenceTables(referenceTables, tablePrefix);
            return referenceTables;
        }
        #endregion


        //Following code is Commented 
        //Please note it is exists in the Template
    }
}
