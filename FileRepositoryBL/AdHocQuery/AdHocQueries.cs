using Arohan.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace FileRepository.BusinessObjects
{
    public class AdHocQueries : QueryResult<AdHocQueries>
    {
        const bool NO_MOVEX = true;

        #region "Constuctor"

        public AdHocQueries()
        {
            SetDb();
        }

        #endregion

        #region "Properties"

        #endregion

        #region "Helper"

        private void SetDb()
        {
            AppDb oDB = new AppDb();
            this.SetDb(oDB);
        }

        #endregion

        #region "Ad Hoc Queries"

        public string WriteDataToExcel(string TemplateFilePath, string sCopyToFolderPath)
        {
            string sFileName = "";
            DataTable dt = new DataTable();
            string sSql = @"";
            dt = GetData(sSql);
            if (dt == null || dt.Rows.Count <= 0) return "";
            sFileName = ExcelUtils.WriteToExcel("Data", dt, TemplateFilePath, sCopyToFolderPath);
            return sFileName;
        }

        public DataTable GetData(string sSql)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = this.GetDataTable(sSql);
                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataTable GetQueryResult(string sSql)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = this.GetDataTable(sSql);
                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataTable SearchByKeyword(string keyword, string tableName, string schema = "dbo")
        {
            try
            {
                string sSql = @"EXEC sp_SearchKeyword @keyword, @tableName, @schema";

                DataTable dt = new DataTable();
                dt = this.GetDataTable(sSql, new object[] { "@keyword", keyword, "@tableName", tableName, "@schema", schema });
                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataTable GetNotificationRecords(int? nNoteID)
        {
            try
            {
                string sSql = @"SELECT * FROM vw_ATNNotifications WHERE IsNull(EmailAddress,'') <> '' AND (@NoteID Is Null Or NoteID = @NoteID)";
                DataTable dt = new DataTable();
                dt = this.GetDataTable(sSql, new object[] { "@NoteID", nNoteID });
                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string CreatePassword()
        {
            int length = 8;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        #endregion

        #region "Helper Functions"

        public static SqlCommand CreateSqlCommand(string sProcedureName, SqlConnection sqlCon, CommandType _CommandType)
        {
            SqlCommand command;
            command = new SqlCommand(sProcedureName, sqlCon);
            command.CommandType = _CommandType;
            command.CommandTimeout = 0;
            return command;
        }

        #endregion        
    }
}