
  
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
    public partial class Department : Entity<Department>
    {
        public static string TableName { get { return "Department"; } }

        #region "static ctor"

        static Department()
        {
            AppDb oAppdb = new AppDb();
            // Department.Init(oAppdb, oAppdb.TablePrefix + TableName);
            List<ReferenceTable> _ReferenceTables = Department.GetReferenceTables(oAppdb.TablePrefix);
            Department.Init(oAppdb, oAppdb.TablePrefix + TableName, "DepartmentID", _ReferenceTables);
        }

        #endregion

        #region "Public Propeties"

        //Actual Properties from Table
        private Int32? _DepartmentID;
        public Int32? DepartmentID { get { return _DepartmentID; } set { SetProperty("DepartmentID", ref _DepartmentID, value); } }    //**PK
        private string _Code;
        public string Code { get { return _Code; } set { SetProperty("Code", ref _Code, value); } }
        private string _Name;
        public string Name { get { return _Name; } set { SetProperty("Name", ref _Name, value); } }
               
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
            
            Department.GetCustomReferenceTables(referenceTables, tablePrefix);
            return referenceTables;
        }
        #endregion


        //Following code is Commented 
        //Please note it is exists in the Template
    }
}
