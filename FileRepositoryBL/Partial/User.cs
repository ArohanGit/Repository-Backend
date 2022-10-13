
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using Arohan.Data;
using FileRepository.BusinessObjects;

namespace FileRepository.BusinessObjects
{
    public partial class User : Entity<User>
    {

        private bool IsPropertyChangeEvent = false;
        private string tablePrefix;

        #region "Database context"
        protected override void SetDb(Database dbQualified, string tableNameQualified)
        {
            AppDb oAppDb = new AppDb();
            tablePrefix = oAppDb.TablePrefix;
            base.SetDb(oAppDb, tablePrefix + TableName);
        }
        #endregion

        #region "Settings"

        private void Settings()
        {
            this.AuditThis = false;
            this.IsPropertyChangeEvent = false;
        }

        #endregion

        #region "public ctor"

        public User(bool defaults) : base(defaults) { }
        public User()
        {
            Settings();
            if (this.IsPropertyChangeEvent) PropertyChanged += PropertyChange;
        }

        #endregion

        #region "Custom Validation Rules"

        protected override void SetValidationRules(List<BusinessRule> ValidationRules)
        {
            // These rules will flow to UI.
            // ValidationRules.Add(new ValidateCompare("PODate", "", ValidationOperator.GreaterThan, ValidationDataType.Date, DateTime.Today));
            base.SetValidationRules(ValidationRules);
        }

        protected override void Validate()
        {
            try
            {
                // Code for custom validation     
                // Theese Rules will be server side only. (will not flow to UI)
                // if (this.PODate > DateTime.Today) Errors.Add(new ValidationError("PODate", string.Format("PO Date should be less than or equal to {0}", DateTime.Today)));
                base.Validate();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region "Custom Reference Table Definitions, if any"

        private static void GetCustomReferenceTables(List<ReferenceTable> referenceTables, string tablePrefix)
        {
        }

        #endregion

        #region "On Property Change"

        private void PropertyChange(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == "<PropertyName>")
            //{
            //    PropertyAuditEventArgs<int?> e1 = (PropertyAuditEventArgs<int?>)e;
            //    return;
            //}
        }

        #endregion

        #region "Audit Related"

        public override void OnAudit(string eventType, Entity<User> oldEntity, List<AuditLog> log)
        {
            base.OnAudit(eventType, oldEntity, log);
        }

        #endregion

        #region "Override Base methods"

        protected override void OnSelecting(ref string sql)
        {
            try
            {
                // sql = @"
                //    CustomSql {0}
                // ";
                // 
                // sql = string.Format(sql, "ABC");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        protected override void OnInserting(ref string sql)
        {
            try
            {
                //if (<DateField> == null) <DateField> = DateTime.Now;
                //sql += "; UPDATE [YYY] Set ItemCount = ItemCount + 1 WHERE Id = " + this.<PrimaryKey> + ";";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        protected override void OnUpdating(ref string sql)
        {
            try
            {
                //if (<DateField> == null) <DateField> = DateTime.Now;
                //sql += "; UPDATE [YYY] Set ItemCount = ItemCount + 1 WHERE Id = " + this.<PrimaryKey> + ";";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        protected override void OnDeleting(ref string sql)
        {
            try
            {
                //sql += "; UPDATE [YYY] Set ItemCount = ItemCount - 1 WHERE Id = " + this.<PrimaryKey> + ";";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        protected override void OnSelected()
        {
        }

        protected override void OnInserted()
        {
            base.OnInserted();
            //CreateUserItem();
        }

        protected override void OnUpdated()
        {
            base.OnUpdated();
            //CreateUserItem();
        }

        protected override void OnDeleted()
        {
            base.OnDeleted();
        }

        #endregion

        #region "Helper Functions"

        public List<User> SaveList(List<User> oUserList)
        {
            try
            {
                if (oUserList == null || oUserList.Count <= 0) return oUserList;
                foreach (User oUser in oUserList)
                {
                    oUser.Save();
                }
                return oUserList;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region "Authentication Methods"

        public bool IsValidUser(string UserId, string Password)
        {
            int cnt = 0;
            try
            {
                string sSql = @"Select count(*) From [User]
                Where WebUserId= @UserId
                And [Password] = @Password
                And IsNull(IsLeft, 'N') <> 'Y'";
                cnt = (int)new AppDb().Scalar(sSql, new object[] { "@UserId", UserId, "@Password", Password });
            }
            catch (Exception ex)
            {
            }
            return (cnt > 0);
        }

        public string GetRoles(string UserId, string Delimiter)
        {
            string result = "";
            try
            {
                string sSql = @"Select TOP 1
	                                r.[Name]
                                From [Role] r 
                                Inner Join UserRole er On r.RoleId = er.RoleId 
                                Inner Join [User] e on e.UserID = er.UserID And e.WebUserId = @UserId
                                Where IsNull(e.IsLeft, 'N') <> 'Y'";

                result = (string)new AppDb().Scalar(sSql, new object[] { "@UserId", UserId });
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        #endregion

        //Following code is Commented 
        //Please note it is exists in the Template
    }
}
