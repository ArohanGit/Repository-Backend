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
using System.Configuration;
using System.IO;
using ASCommon;
using System.Web;

namespace FileRepository.BusinessObjects
{
    public partial class Repository : Entity<Repository>
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

        public Repository(bool defaults) : base(defaults) { }
        public Repository()
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

        public override void OnAudit(string eventType, Entity<Repository> oldEntity, List<AuditLog> log)
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

                this.RepositoryNo = GetRepoNo(this.DepartmentID);
                this.CreatedOn = DateTime.Today;
                this.UpdatedOn = DateTime.Today;
                this.version = 0;
                this.ApprovalLevel = 0;
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

                //// Send Notification
                //if (this.ApprovalLevel > 0)
                //{
                //    object AppLvl = new Repository().Scalar("AVG", "ApprovalLevel", new Repository() { RepositoryID = this.RepositoryID });
                //    if (Convert.ToInt32(AppLvl) != this.ApprovalLevel)
                //    {
                //        SendNotification(this);
                //    }
                //}
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

            //CreateRepositoryItem();
        }

        protected override void OnUpdated()
        {
            base.OnUpdated();

            //CreateRepositoryItem();
        }

        protected override void OnDeleted()
        {
            base.OnDeleted();
        }

        #endregion

        #region "Helper Functions"

        public List<Repository> SaveList(List<Repository> oRepositoryList)
        {
            try
            {
                if (oRepositoryList == null || oRepositoryList.Count <= 0) return oRepositoryList;
                foreach (Repository oRepository in oRepositoryList)
                {
                    oRepository.Save();
                }
                return oRepositoryList;
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
                List<NotificationTo> oExistingNotificationToList = new NotificationTo().LoadList(where: "RepositoryID=" + this.RepositoryID).ToList();

                // Delete Existing
                foreach (NotificationTo oNotificationTo in oExistingNotificationToList)
                {
                    oNotificationTo.Delete();
                }

                // Return incase Users not selected
                if (string.IsNullOrEmpty(NotificationToUserIDs)) return;

                // Insert Notification To Users
                List<User> oUserList = new User().LoadList(where: "UserID In (" + NotificationToUserIDs + ")").ToList();
                foreach (User oUser in oUserList)
                {
                    NotificationTo oNotificationTo = new NotificationTo();
                    oNotificationTo.RepositoryID = this.RepositoryID;
                    oNotificationTo.Email = oUser.EMailAddress;
                    oNotificationTo.ApprovedOn = DateTime.Now;
                    oNotificationTo.Save();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public Repository CopyRepository(Repository oRepository)
        {
            try
            {
                // Load User Info 
                User oUser = new User().GetLoggedInUserInfo();
                if (oUser == null) return oRepository;

                // Load Notification TO
                List<NotificationTo> NotificationToList = new NotificationTo().LoadList(where: "RepositoryID=" + oRepository.RepositoryID).ToList();

                // Remove RepositoryID & Create new record
                oRepository.RepositoryID = null;
                oRepository.RepositoryName = oRepository.RepositoryName + "- Copy" + DateTime.Now.ToString("HH_MM_ss");
                oRepository.RepositoryDescr = oRepository.RepositoryDescr;
                oRepository.CreatedBy = oUser.UserID;
                oRepository.CreatedOn = DateTime.Now;
                oRepository.UpdtedBy = oUser.UserID;
                oRepository.UpdatedOn = DateTime.Now;
                oRepository.IsApproved = "N";
                oRepository.RejectRemark = "";
                oRepository.Remark = "";
                oRepository.RepoDuration = "One Time";
                oRepository.Save();

                // Save Notification To List
                foreach (NotificationTo oNotificationTo in NotificationToList)
                {
                    oNotificationTo.NotificationToID = null;
                    oNotificationTo.RepositoryID = oRepository.RepositoryID;
                    oNotificationTo.Save();
                }

                return oRepository;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private string GetRepoNo(Int32? DepartmentID)
        {
            string sMaxRepositoryNo = "";
            string sSql = @"Declare @MaxRepositoryNo int = 0
                            Declare @RepoNoWithPading nvarchar(4)
                            Declare @Department nvarchar(10)
                            SET @MaxRepositoryNo = IsNull((Select Top 1 Max(Right(RepositoryNo,4)) As RepositoryNo From dbo.Repository),0) + 1
                            Set @RepoNoWithPading = Right('0000' + CONVERT(VARCHAR(4), @MaxRepositoryNo),4)
                            SET @Department = (Select Top 1 [Code] From Department Where DepartmentID = " + DepartmentID ;
                    sSql = sSql + " ) SELECT @Department + '-' + CONVERT(nvarchar(8),GETDATE(),112) + @RepoNoWithPading As RepositoryNo";
            object obj = new AppDb().Scalar(sSql);
            if (obj != null) { sMaxRepositoryNo = Convert.ToString(obj); }
            return sMaxRepositoryNo;
        }

        public DataTable GetRepositoryList(string sMode)
        {
            try
            {
                DataTable dt = new DataTable();
                string sWebUserID = System.Web.HttpContext.Current.User.Identity.Name;
                string sSql = "Exec pr_GetRepositoryList @Mode, @WebUserID";
                dt = new AppDb().GetDataTable(sSql, new object[] { "@Mode", sMode, "@WebUserID", sWebUserID });
                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataTable GetRepositoryCounts()
        {
            try
            {
                DataTable dt = new DataTable();
                string sWebUserID = System.Web.HttpContext.Current.User.Identity.Name;
                string sSql = "Exec pr_GetRepositoryCounts @WebUserID";
                dt = new AppDb().GetDataTable(sSql, new object[] { "@WebUserID", sWebUserID });
                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void ApproveRepository(Repository oRepository, string sWebUserID)
        {
            try
            {
                User oUser = new User().Load(where: "WebUserID='" + sWebUserID + "'");
                int? nNoOfAppLevels = new NotificationTo().Count(where: "RepositoryID=" + oRepository.RepositoryID);
                oRepository.UpdtedBy = oUser.UserID;
                oRepository.UpdatedOn = DateTime.Now;

                object NoOfLvl = new NotificationTo().Count(new NotificationTo() { RepositoryID = oRepository.RepositoryID });
                if (oRepository.ApprovalLevel == nNoOfAppLevels)
                {
                    oRepository.IsApproved = "Y";
                }
                else
                {
                    oRepository.ApprovalLevel += 1;
                }

                object AppLvl = new Repository().Scalar("AVG", "ApprovalLevel", new Repository() { RepositoryID = oRepository.RepositoryID });
                oRepository.Save();

                // Send Mail to Next Level.
                if (Convert.ToInt32(AppLvl) != oRepository.ApprovalLevel)
                {
                    Repository.SendNotification(oRepository);
                }
                else
                // Send Final Approval Mail
                if (oRepository.ApprovalLevel == Convert.ToInt32(NoOfLvl))
                {
                    Repository.SendFinalApprovalNotification(oRepository);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void SendNotification(Repository oRepository)
        {
            try
            {
                if (oRepository == null) return;
                NotificationTo oNotificationTo = new NotificationTo().Load(where: "RepositoryID=" + oRepository.RepositoryID + " And ApproverLevel=" + oRepository.ApprovalLevel);
                if (oNotificationTo == null) return;
                User oUser = new User().Load(where: "WebUserID='" + oNotificationTo.WebUserID + "'");
                Department oDepartment = new Department().Load(where: "DepartmentID =" + oRepository.DepartmentID);
                DataTable dt = new Repository().RepositoryUsers(oRepository.RepositoryID);

                bool IsBodyHtml = true;
                string Path = ConfigurationManager.AppSettings["NotificationTemplatePath"];
                string ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
                string BodyHtml = File.ReadAllText(Path);

                string To = oNotificationTo.Email;
                string MailSubject = "";

                int? nCnt = new Files().Count(where: "RepositoryID=" + oRepository.RepositoryID);

                if (oRepository.ApprovalLevel == oNotificationTo.ApproverLevel && oNotificationTo.AllowUpload == "Y" && nCnt <= 0)
                {
                     MailSubject = "Please attached document - " + oRepository.RepositoryName + " repository.";
                }
                else
                {
                     MailSubject = "Waiting for your apporval - " + oRepository.RepositoryName + " repository.";
                }                    

                string ApproveUrl = ApplicationUrl + "ApproveReject/ApproveRepository/" + oRepository.RepositoryID.ToString() + "|" + oUser.WebUserID;
                string RejectUrl = ApplicationUrl + "ApproveReject/RejectRepository/" + oRepository.RepositoryID.ToString() + "|" + oUser.WebUserID;
                string Body = BodyHtml;

                Body = Body.Replace("@RepositoryNo", oRepository.RepositoryNo);
                Body = Body.Replace("@RepositoryName", oRepository.RepositoryName);
                Body = Body.Replace("@RepositoryDescr", oRepository.RepositoryDescr);
                Body = Body.Replace("@UserName", oUser.Name);
                Body = Body.Replace("@DepartmentName", oDepartment.Name); 
                foreach (DataRow dr in dt.Rows)
                {
                    Body = Body.Replace("@NCreater", dr["NCreater"].ToString());
                    Body = Body.Replace("@RCrater", dr["RCrater"].ToString());
                    Body = Body.Replace("@AppName1", dr["AppName1"].ToString());
                    Body = Body.Replace("@RName1", dr["RName1"].ToString());
                    Body = Body.Replace("@AppName2", dr["AppName2"].ToString());
                    Body = Body.Replace("@RName2", dr["RName2"].ToString());
                    Body = Body.Replace("@AppName3", dr["AppName3"].ToString());
                    Body = Body.Replace("@RName3", dr["RName3"].ToString());
                }
                   
                //Body = Body.Replace("@ApproveUrl", ApproveUrl);
                //Body = Body.Replace("@RejectUrl", RejectUrl);

                // Buttons Show only if Files Uploaded
                
                string sButtonHtml = @"<p class=MsoNormal> <a id='btnAccept' href='" + ApproveUrl + "' style='-webkit-appearance: button; -moz-appearance: button; appearance: button; text-decoration: none; background-color: Green; color: White; border: 2px; border-color: 2px; padding: 10px; font-weight: bold;'> Approve </ a > &nbsp; <a href = '" + RejectUrl + "' style = '-webkit-appearance: button; -moz-appearance: button; appearance: button; text-decoration: none; background-color: Red; color: White; border: 2px; border-color: 2px; padding: 5px; font-weight: bold;' > Reject </ a > </ p >";
                if (oRepository.ApprovalLevel == oNotificationTo.ApproverLevel && oNotificationTo.AllowUpload == "Y" && nCnt <= 0)
                {
                    sButtonHtml = "";
                }
                Body = Body.Replace("@Buttons", sButtonHtml);

                string SenderEmailAddress = ConfigurationManager.AppSettings["SenderEmailAddress"];
                bool EnableSSL = ConfigurationManager.AppSettings["EnableSSL"] == "False" ? false : true;
                string MailServer = ConfigurationManager.AppSettings["MailServer"];
                int MailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);
                string MailServerUsername = ConfigurationManager.AppSettings["MailServerUsername"];
                string MailServerPassword = ConfigurationManager.AppSettings["MailServerPassword"];

                string _AttachmentPath = GetAttachements(oRepository.RepositoryID);
                Common.SendMail(SenderEmailAddress, To, MailSubject, Body, IsBodyHtml, EnableSSL, MailServer, MailPort, MailServerUsername, MailServerPassword, _AttachmentPath);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private static string GetAttachements(int? nRepositoryID)
        {
            string attachmentPaths = "";
            string DirPath = HttpContext.Current.Server.MapPath("~/Files/"); ;
            List<Files> oFilesList = new Files().LoadList(where: "RepositoryID=" + nRepositoryID).ToList();

            foreach (Files oFiles in oFilesList)
            {
                if (!string.IsNullOrEmpty(attachmentPaths)) attachmentPaths += "|";
                string sPath = string.Concat(DirPath, oFiles.FileSrl.ToString(), oFiles.Extension);
                attachmentPaths += sPath;
            }
            return attachmentPaths;
        }

        public static void SendFinalApprovalNotification(Repository oRepository)
        {
            try
            {
                if (oRepository == null) return;
                NotificationTo oNotificationTo = new NotificationTo().Load(where: "RepositoryID=" + oRepository.RepositoryID + " And AllowUpload='Y'");
                // if (oNotificationTo == null) return;
                User oUserOwner = new User().Load(where: "WebUserID='" + oNotificationTo.WebUserID + "'");
                User oUser = new User().Load(oRepository.CreatedBy);
                Department oDepartment = new Department().Load(where: "DepartmentID =" + oRepository.DepartmentID);

                bool IsBodyHtml = true;
                string Path = ConfigurationManager.AppSettings["FinalApprovalNotificationTemplatePath"];
                string ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
                string BodyHtml = File.ReadAllText(Path);

                string To = oUser.EMailAddress;
                string Cc = oUserOwner.EMailAddress;
                string MailSubject = oRepository.RepositoryName + " Repository Approved And Document is active.";

                string Body = BodyHtml;
                Body = Body.Replace("@RepositoryNo", oRepository.RepositoryNo);
                Body = Body.Replace("@RepositoryName", oRepository.RepositoryName);
                Body = Body.Replace("@RepositoryDescr", oRepository.RepositoryDescr);
                Body = Body.Replace("@UserName", oUser.Name);
                Body = Body.Replace("@OwnerName", oUserOwner.Name);
                Body = Body.Replace("@DepartmentName", oDepartment.Name);

                string SenderEmailAddress = ConfigurationManager.AppSettings["SenderEmailAddress"];
                bool EnableSSL = ConfigurationManager.AppSettings["EnableSSL"] == "False" ? false : true;
                string MailServer = ConfigurationManager.AppSettings["MailServer"];
                int MailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);
                string MailServerUsername = ConfigurationManager.AppSettings["MailServerUsername"];
                string MailServerPassword = ConfigurationManager.AppSettings["MailServerPassword"];
                Common.SendMail(SenderEmailAddress, To, MailSubject, Body, IsBodyHtml, EnableSSL, MailServer, MailPort, MailServerUsername, MailServerPassword, null);
                Common.SendMail(SenderEmailAddress, Cc, MailSubject, Body, IsBodyHtml, EnableSSL, MailServer, MailPort, MailServerUsername, MailServerPassword, null);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void SendRejectNotification(Repository oRepository, string sRejectedByWebUserID)
        {
            try
            {
                if (oRepository == null) return;
                User oRejectedByUser = new User().Load(where: "WebUserID='" + sRejectedByWebUserID + "'");
                User oCreatorUser = new User().Load(oRepository.CreatedBy);
                Department oDepartment = new Department().Load(where: "DepartmentID =" + oRepository.DepartmentID);

                bool IsBodyHtml = true;
                string Path = ConfigurationManager.AppSettings["RejectNotificationTemplatePath"];
                string ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
                string BodyHtml = File.ReadAllText(Path);

                string To = oCreatorUser.EMailAddress;
                string MailSubject = "Rejected " + oRepository.RepositoryName + " repository.";

                string Body = BodyHtml;

                Body = Body.Replace("@RepositoryNo", oRepository.RepositoryNo);
                Body = Body.Replace("@RepositoryName", oRepository.RepositoryName);
                Body = Body.Replace("@RepositoryDescr", oRepository.RepositoryDescr);
                Body = Body.Replace("@UserName", oCreatorUser.Name);
                Body = Body.Replace("@RejectedBy", oRejectedByUser.Name);
                Body = Body.Replace("@DepartmentName", oDepartment.Name);

                string SenderEmailAddress = ConfigurationManager.AppSettings["SenderEmailAddress"];
                bool EnableSSL = ConfigurationManager.AppSettings["EnableSSL"] == "False" ? false : true;
                string MailServer = ConfigurationManager.AppSettings["MailServer"];
                int MailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);
                string MailServerUsername = ConfigurationManager.AppSettings["MailServerUsername"];
                string MailServerPassword = ConfigurationManager.AppSettings["MailServerPassword"];

                Common.SendMail(SenderEmailAddress, To, MailSubject, Body, IsBodyHtml, EnableSSL, MailServer, MailPort, MailServerUsername, MailServerPassword, null);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataTable RepositorysNearingExpiry()
        {
            try
            {
                DataTable dt = new DataTable();
                string sWebUserID = System.Web.HttpContext.Current.User.Identity.Name;
                // string sWhere = @" WHERE DATEADD(DAY, -NotificationDays, ValidTo) <= GetDate() AND WebUserID = '" +  sWebUserID + "' ORDER BY ValidTo";
                //string sSql = GetNearingExpirySQL() + sWhere;
                string sSql = "Exec pr_GetRepositorysNearingExpiry @WebUserID";
                dt = new AppDb().GetDataTable(sSql, new object[] { "@WebUserID", sWebUserID });
                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private DataTable RepositoryUsers(int? RepositoryID)
        {
            try
            {
                DataTable dt = new DataTable();
                //string sWebUserID = System.Web.HttpContext.Current.User.Identity.Name;
                // string sWhere = @" WHERE DATEADD(DAY, -NotificationDays, ValidTo) <= GetDate() AND WebUserID = '" +  sWebUserID + "' ORDER BY ValidTo";
                //string sSql = GetNearingExpirySQL() + sWhere;
                string sSql = "Exec pr_RepositoryUsers @RepositoryID";
                dt = new AppDb().GetDataTable(sSql, new object[] { "@RepositoryID", RepositoryID });
                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataTable RepositorysNearingExpiry_Email()
        {
            try
            {
                DataTable dt = new DataTable();
                //int? nNotificationNoOfDays = (WebConfigurationManager.AppSettings["NotificationNoOfDays"] != null ? Convert.ToInt32(WebConfigurationManager.AppSettings["NotificationNoOfDays"]) : 0);
                //string sWhere = @" WHERE ExpiresInDays >= 1 AND ExpiresInDays <= NotificationDays";
                //string sSql = GetNearingExpirySQL() + sWhere;
                string sSql = "Exec pr_GetRepositorysNearingExpiryEmail";
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
		                     d.RepositoryID
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
	                    FROM Repository d
	                    INNER JOIN NotificationTo n On n.RepositoryID = d.RepositoryID
	                    INNER JOIN [User] u On u.EMailAddress = n.Email
                    ) t";
        }

        #endregion

        //Following code is Commented 
        //Please note it is exists in the Template
    }
}
