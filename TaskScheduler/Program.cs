using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Net.Mail;
using System.Net;
using System.IO;
using ASCommon;
using FileRepository.BusinessObjects;

namespace TaskScheduler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ExecuteSchedule();
            Application.Exit();
        }

        public static SqlCommand CreateSqlCommand(string sProcedureName, SqlConnection sqlCon, CommandType _CommandType)
        {
            SqlCommand command;
            command = new SqlCommand(sProcedureName, sqlCon);
            command.CommandType = _CommandType;
            command.CommandTimeout = 0;
            return command;
        }

        public static void ExecuteSchedule()
        {
            try
            {
                // Send Notification to Trustee
                SendNotification();
            }
            catch (Exception ex)
            {             
                throw new Exception(ex.Message);
            }
        }

        public static void SendNotification()
        {
            try
            {
                DataTable dt = new Document().DocumentsNearingExpiry_Email();
                if (dt == null || dt.Rows.Count <= 0) return;
                bool IsBodyHtml = true;
                string Path = ConfigurationManager.AppSettings["NotificationExpireRepoTemplatePath"];
                string ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
                string BodyHtml = File.ReadAllText(Path);

                foreach (DataRow dr in dt.Rows)
                {
                    string To = dr["EmailAddress"].ToString();
                    string Cc = dr["OwnerEmail"].ToString();
                    string MailSubject = "Approved And Document is active - " +  dr["RepositoryName"] + " Repository " + dr["Status"].ToString();

                    string Body = BodyHtml;
                    Body = Body.Replace("@RepositoryName", dr["RepositoryName"].ToString());
                    Body = Body.Replace("@RepositoryDescr", dr["RepositoryDescr"].ToString());
                    Body = Body.Replace("@Name", dr["Name"].ToString());
                    Body = Body.Replace("@OwnerName", dr["OwnerName"].ToString());
                    Body = Body.Replace("@Status", dr["Status"].ToString());
                    Body = Body.Replace("@DepartmentName", dr["DepartmentName"].ToString());


                    string SenderEmailAddress = ConfigurationManager.AppSettings["SenderEmailAddress"];
                    bool EnableSSL = ConfigurationManager.AppSettings["EnableSSL"] == "False" ? false : true;
                    string MailServer = ConfigurationManager.AppSettings["MailServer"];
                    int MailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);
                    string MailServerUsername = ConfigurationManager.AppSettings["MailServerUsername"];
                    string MailServerPassword = ConfigurationManager.AppSettings["MailServerPassword"];
                    Common.SendMail(SenderEmailAddress, To, MailSubject, Body, IsBodyHtml, EnableSSL, MailServer, MailPort, MailServerUsername, MailServerPassword, null);
                    Common.SendMail(SenderEmailAddress, Cc, MailSubject, Body, IsBodyHtml, EnableSSL, MailServer, MailPort, MailServerUsername, MailServerPassword, null);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}

