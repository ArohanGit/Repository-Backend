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
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                // Send Notification to Trustee
                SendNotification();
            }
            catch (Exception ex)
            {             
                throw new Exception(ex.Message);
            }
        }

        public static DataTable getDataTable()
        { 

        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        string storedProcedureName = "pr_GetDocumentsNearingExpiryEmail";

            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
               
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {

                    command.CommandType = CommandType.StoredProcedure; 

                    connection.Open();


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Load the results into the DataTable
                        dataTable.Load(reader);
                    }

                    connection.Close();

                }

            }
            return dataTable;
        }

        public static void SendNotification()
        {
            try
            {
                //MessageBox.Show("Before BL");

               
                DataTable dt = getDataTable();

                //MessageBox.Show("After BL");

                //int? nNotificationNoOfDays = (WebConfigurationManager.AppSettings["NotificationNoOfDays"] != null ? Convert.ToInt32(WebConfigurationManager.AppSettings["NotificationNoOfDays"]) : 0);
                //string sWhere = @" WHERE ExpiresInDays >= 1 AND ExpiresInDays <= NotificationDays";
                //string sSql = GetNearingExpirySQL() + sWhere;
                if (dt == null || dt.Rows.Count <= 0) return;
                bool IsBodyHtml = true;
                string Path = ConfigurationManager.AppSettings["NotificationExpireRepoTemplatePath"];
                string ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];
                //MessageBox.Show(Path + "-" + ApplicationUrl);
                string BodyHtml = File.ReadAllText(Path);

                foreach (DataRow dr in dt.Rows)
                {
                    string To = dr["EmailAddress"].ToString();
                    string Cc = dr["OwnerEmail"].ToString();
                    string MailSubject = " Repository " + dr["Status"].ToString() + "-" + dr["RepositoryNo"].ToString() + "-" + dr["RepositoryName"] ;

                    string Body = BodyHtml;
                    Body = Body.Replace("@RepositoryNo", dr["RepositoryNo"].ToString());
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
                MessageBox.Show(ex.Message);
                throw (ex);
            }
        }
    }
}

