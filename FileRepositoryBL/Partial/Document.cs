
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using Arohan.Data;
using FileRepository.BusinessObjects;
using System.Data;
using System.Web.Configuration;

namespace FileRepository.BusinessObjects
{
    public partial class Document : Entity<Document>
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

        public Document(bool defaults) : base(defaults) { }
        public Document()
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

        public override void OnAudit(string eventType, Entity<Document> oldEntity, List<AuditLog> log)
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
                //User oUser = new User().Load(where: "WebUserID='" + System.Web.HttpContext.Current.User.Identity.Name + "'");
                //this.CreatedBy = oUser.UserID;
                // Please note will not get identity informtion here because we are sending information in FormData and AppAuthenticationFilter is not applied.
                this.CreatedOn = DateTime.Today;
                this.UpdatedOn = DateTime.Today;
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
                //User oUser = new User().Load(where: "WebUserID='" + System.Web.HttpContext.Current.User.Identity.Name + "'");
                //this.UpdtedBy = oUser.UserID;
                // Please note will not get identity informtion here because we are sending information in FormData and AppAuthenticationFilter is not applied.
                this.UpdatedOn = DateTime.Today;
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

            //CreateDocumentItem();
        }

        protected override void OnUpdated()
        {
            base.OnUpdated();
            //CreateDocumentItem();
        }

        protected override void OnDeleted()
        {
            base.OnDeleted();
        }

        #endregion

        #region "Helper Functions"

        public List<Document> SaveList(List<Document> oDocumentList)
        {
            try
            {
                if (oDocumentList == null || oDocumentList.Count <= 0) return oDocumentList;
                foreach (Document oDocument in oDocumentList)
                {
                    oDocument.Save();
                }
                return oDocumentList;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void SaveChildren(string NotificationToUserIDs)
        {
            try
            {               
                List<NotificationTo> oExistingNotificationToList = new NotificationTo().LoadList(where: "DocumentID=" + this.DocumentID).ToList();

                // Delete Existing
                foreach (NotificationTo oNotificationTo in oExistingNotificationToList)
                {
                    oNotificationTo.Delete(); 
                }

                // Return incase Users not selected
                if(string.IsNullOrEmpty(NotificationToUserIDs)) return;

                // Insert Notification To Users
                List<User> oUserList = new User().LoadList(where: "UserID In (" + NotificationToUserIDs + ")").ToList();
                foreach (User oUser in oUserList)
                {
                    NotificationTo oNotificationTo = new NotificationTo();
                    oNotificationTo.DocumentID = this.DocumentID;
                    oNotificationTo.Email = oUser.EMailAddress;
                    oNotificationTo.Save();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataTable DocumentsNearingExpiry()
        {
            try
            {
                DataTable dt = new DataTable();
                string sWebUserID = System.Web.HttpContext.Current.User.Identity.Name;
                // string sWhere = @" WHERE DATEADD(DAY, -NotificationDays, ValidTo) <= GetDate() AND WebUserID = '" +  sWebUserID + "' ORDER BY ValidTo";
                //string sSql = GetNearingExpirySQL() + sWhere;
                string sSql = "Exec pr_GetDocumentsNearingExpiry @WebUserID";
                dt = new AppDb().GetDataTable(sSql, new object[] { "@WebUserID", sWebUserID });
                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataTable DocumentsNearingExpiry_Email()
        {
            try
            {
                DataTable dt = new DataTable();
                //int? nNotificationNoOfDays = (WebConfigurationManager.AppSettings["NotificationNoOfDays"] != null ? Convert.ToInt32(WebConfigurationManager.AppSettings["NotificationNoOfDays"]) : 0);
                //string sWhere = @" WHERE ExpiresInDays >= 1 AND ExpiresInDays <= NotificationDays";
                //string sSql = GetNearingExpirySQL() + sWhere;
                string sSql = "Exec pr_GetDocumentsNearingExpiryEmail";
                dt = new AppDb().GetDataTable(sSql);
                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public string GetNearingExpirySQL()
        {
            return @"SELECT
	                    *
	                    ,(Case When ExpiresInDays <= 0 Then
			                    'Expired before ' + Cast(ABS(ExpiresInDays) As nvarchar) + ' Days'
			                    Else 'Expires in ' + Cast(ExpiresInDays As nvarchar) + ' Days.'
	                    End) As Status
                    FROM
                    (
	                    SELECT 
		                     d.DocumentID
		                    ,FileName
		                    ,FileDescr
		                    ,version
		                    ,Extension
		                    ,FilePath
		                    ,FileSrl
		                    ,ValidFrom
		                    ,ValidTo
		                    ,IsDelete
		                    ,CreatedBy
		                    ,CreatedOn
		                    ,UpdtedBy
		                    ,UpdatedOn
		                    ,NotificationDays
		                    ,RefTableID
		                    ,DATEDIFF(DAY, GETDATE(), ValidTo) As ExpiresInDays
		                    ,Email
		                    ,Name
		                    ,WebUserID
	                    FROM Document d
	                    INNER JOIN NotificationTo n On n.DocumentID = d.DocumentID
	                    INNER JOIN [User] u On u.EMailAddress = n.Email
                    ) t";
        }

        #endregion

        //Following code is Commented 
        //Please note it is exists in the Template
    }
}
