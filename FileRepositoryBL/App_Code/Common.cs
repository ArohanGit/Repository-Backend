using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Globalization;
using System.Reflection;

//using ClosedXML.Excel;
//using ASLib.InternetTools;

namespace ASCommon
{
    public class Common
    {
        #region "Send Mail"

        public static void SendMail()
        {
            try
            {
                string SenderEmailAddress = "noreply@arohansystems.in";
                string SendTo = "prakash.shinde@arohansystems.in";

                string Subject = "Test Mail";
                string Message = "This is test mail";

                bool IsBodyHtml = true;
                bool EnableSSL = ConfigurationManager.AppSettings["EnableSSL"] == "False" ? false : true;
                string MailServer = ConfigurationManager.AppSettings["MailServer"];
                int MailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);
                string MailServerUsername = ConfigurationManager.AppSettings["MailServerUsername"];
                string MailServerPassword = ConfigurationManager.AppSettings["MailServerPassword"];

                SendMail(SenderEmailAddress, SendTo, Subject, Message, IsBodyHtml, EnableSSL, MailServer, MailPort, MailServerUsername, MailServerPassword, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMail(string _to, string _body)
        {
            try
            {
                string SendTo = _to;
                string Message = _body;
                bool IsBodyHtml = true;

                string SenderEmailAddress = ConfigurationManager.AppSettings["SenderEmailAddress"];
                string MailSubject = ConfigurationManager.AppSettings["MailSubject"];
                bool EnableSSL = ConfigurationManager.AppSettings["EnableSSL"] == "False" ? false : true;
                string MailServer = ConfigurationManager.AppSettings["MailServer"];
                int MailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);
                string MailServerUsername = ConfigurationManager.AppSettings["MailServerUsername"];
                string MailServerPassword = ConfigurationManager.AppSettings["MailServerPassword"];
                SendMail(SenderEmailAddress, SendTo, MailSubject, Message, IsBodyHtml, EnableSSL, MailServer, MailPort, MailServerUsername, MailServerPassword, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMail(string _to, string _mailSubject, string _body)
        {
            try
            {
                string SendTo = _to;
                string Message = _body;
                bool IsBodyHtml = true;

                string SenderEmailAddress = ConfigurationManager.AppSettings["SenderEmailAddress"];
                // string MailSubject = ConfigurationManager.AppSettings["MailSubject"];
                bool EnableSSL = ConfigurationManager.AppSettings["EnableSSL"] == "False" ? false : true;
                string MailServer = ConfigurationManager.AppSettings["MailServer"];
                int MailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);
                string MailServerUsername = ConfigurationManager.AppSettings["MailServerUsername"];
                string MailServerPassword = ConfigurationManager.AppSettings["MailServerPassword"];
                SendMail(SenderEmailAddress, SendTo, _mailSubject, Message, IsBodyHtml, EnableSSL, MailServer, MailPort, MailServerUsername, MailServerPassword, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMail(string _from, string _to, string _subject, string _body, bool _IsBodyHtml, bool _EnableSSL, string _hostname, int _port, string _UserName, string _Password, string _AttachmentPath)
        {
            try
            {
                Attachment MyAttachment;

                using (MailMessage mm = new MailMessage(_from, _to))
                {
                    //Attachements
                    if (!string.IsNullOrEmpty(_AttachmentPath))
                    {
                        string[] sFiles = _AttachmentPath.Split('|');
                        foreach (string path in sFiles)
                        {
                            MyAttachment = new Attachment(path);
                            MyAttachment.Name = path.Substring(path.LastIndexOf("\\") + 1);
                            mm.Attachments.Add(MyAttachment);
                        }
                    }

                    mm.Subject = _subject;
                    mm.Body = _body;
                    mm.IsBodyHtml = _IsBodyHtml;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = _hostname;
                    if (_EnableSSL)
                    {
                        smtp.EnableSsl = true;
                    }

                    if (!string.IsNullOrEmpty(_UserName) && !string.IsNullOrEmpty(_Password))
                    {
                        NetworkCredential NetworkCred = new NetworkCredential(_UserName, _Password);
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                    }
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = _port; //25; // 587; //587;
                    smtp.Send(mm);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending email.!!!\n" + ex);
            }
        }

        #endregion

        #region "Date Formating"

        public static bool IsEqual(DateTime? _dt1, DateTime? _dt2)
        {
            if (!_dt1.HasValue && !_dt2.HasValue) return true;
            if (_dt1.HasValue && !_dt2.HasValue) return false;
            if (!_dt1.HasValue && _dt2.HasValue) return false;

            DateTime dt1 = Convert.ToDateTime(Common.ToDate(_dt1));
            DateTime dt2 = Convert.ToDateTime(Common.ToDate(_dt2));

            if (dt1.Year.ToString() + dt1.Month.ToString().PadLeft(2, '0') + dt1.Day.ToString().PadLeft(2, '0') ==
                dt2.Year.ToString() + dt2.Month.ToString().PadLeft(2, '0') + dt2.Day.ToString().PadLeft(2, '0'))
                return true;

            return false;
        }

        public static DateTime? ToDate(DateTime? _dt)
        {
            if (!_dt.HasValue) return null;
            DateTime dt = Convert.ToDateTime(_dt);
            return Convert.ToDateTime(dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0'));
        }

        #endregion

        #region "Error DataTable related."

        //Create Errors DataTable
        public static DataTable CreateErrorsDataTable()
        {
            DataTable dt = new DataTable("Errors");
            dt.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
            DataColumn dc = null;

            //Row Number
            dc = new DataColumn();
            dc.ColumnName = "RowNo";
            dc.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(dc);

            //Unique ID
            dc = new DataColumn();
            dc.ColumnName = "UniqueID";
            dc.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(dc);

            //Column Name 
            dc = new DataColumn();
            dc.ColumnName = "ColumnName";
            dc.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(dc);

            //Code 
            dc = new DataColumn();
            dc.ColumnName = "Value";
            dc.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "Error";
            dc.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(dc);

            return dt;
        }

        //Add Errors to DataTable
        public static DataTable AddErrorIntoDt(DataTable dt, int RowNo, string sUniqueIDValue, string sColumnName, string sFieldValue, string sError)
        {
            DataRow ErrorsRow;
            ErrorsRow = dt.NewRow();
            ErrorsRow["RowNo"] = RowNo;
            ErrorsRow["UniqueID"] = sUniqueIDValue;
            ErrorsRow["ColumnName"] = sColumnName;
            ErrorsRow["Value"] = sFieldValue;
            ErrorsRow["Error"] = sError;
            dt.Rows.Add(ErrorsRow);
            return dt;
        }

        #endregion

        #region "JSON"     
        #endregion
    }
}

public static class Helper
{
    /// <summary>
    /// Converts a DataTable to a list with generic objects
    /// </summary>
    /// <typeparam name="T">Generic object</typeparam>
    /// <param name="table">DataTable</param>
    /// <returns>List with generic objects</returns>
    public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
    {
        try
        {
            List<T> list = new List<T>();

            foreach (var row in table.AsEnumerable())
            {
                T obj = new T();

                foreach (var prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                    }
                    catch
                    {
                        continue;
                    }
                }

                list.Add(obj);
            }

            return list;
        }
        catch
        {
            return null;
        }
    }
}