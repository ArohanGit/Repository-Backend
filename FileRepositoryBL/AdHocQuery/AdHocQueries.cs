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

        public DataTable GetAsset()
        {
            try
            {
                string sWebUserID = System.Web.HttpContext.Current.User.Identity.Name;
                string str = "Exec SP_GetAsset '" + sWebUserID + "'";
                if (NO_MOVEX) str = "Select * From MVX_Asset";
                DataTable dataTable = new DataTable("Assets");
                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CreateChallanData(string NoteID)
        {
            try
            {
                string sApprovedBy = System.Web.HttpContext.Current.User.Identity.Name;

                string str = "Exec pr_CreateChallanData '" + NoteID + "', '" + sApprovedBy + "'";
                DataTable dataTable = new DataTable("ChallanData");
                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetUserInfo()
        {
            try
            {
                string sUserID = System.Web.HttpContext.Current.User.Identity.Name;
                //string str = "Select * From vw_UserApprovalMatrix Where WebUserID = '" + sUserID + "'";
                string str = "Select * From vw_UserApprovalMatrix Where WebUserID = 'INPOMSRC'";
                DataTable dataTable = new DataTable("UserInfo");
                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable LoadListByUser()
        {
            try
            {
                string sWebUserID = System.Web.HttpContext.Current.User.Identity.Name;
                string str = "Exec pr_GetGSTChallanListByUser null, '" + sWebUserID + "'";
                DataTable dataTable = new DataTable("GstChallanByUser");
                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable LoadAtnListByUser()
        {
            try
            {
                string sWebUserID = System.Web.HttpContext.Current.User.Identity.Name;
                string str = "Exec pr_GetATNListByUser null, '" + sWebUserID + "'";
                DataTable dataTable = new DataTable("GstAtnByUser");
                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable LoadMaterialReturnListByUser()
        {
            try
            {
                string sWebUserID = System.Web.HttpContext.Current.User.Identity.Name;
                string str = "Exec pr_GetMaterialReturnListByUser null, '" + sWebUserID + "'";
                DataTable dataTable = new DataTable("GstMaterialReturnByUser");
                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable LoadDebitNoteListByUser()
        {
            try
            {
                string sWebUserID = System.Web.HttpContext.Current.User.Identity.Name;
                string str = "Exec pr_GetDebitNoteListByUser null, '" + sWebUserID + "'";
                DataTable dataTable = new DataTable("GstDebitNoteByUser");
                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetSuppliersFromMovex()
        {
            try
            {
                string str = "Exec SP_GetSuppliers";
                if (NO_MOVEX) str = "Select * From MVX_Supplier";
                DataTable dataTable = new DataTable("Suppliers");
                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomersFromMovex()
        {
            try
            {
                string str = "Exec SP_GetCustomers";
                if (NO_MOVEX) str = "Select * From MVX_Customer";
                DataTable dataTable = new DataTable("Customers");
                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetSuppliersAddress(string sSupplierCode)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string str = "Exec SP_GetSuppliersAddr '" + sSupplierCode + "'";
                if (NO_MOVEX) str = "Select * From MVX_SupplierAddress Where SupplierCode = '" + sSupplierCode + "'";


                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomersAddress(string sCustomerCode)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string str = "Exec SP_GetCustomersAddr '" + sCustomerCode + "'";
                if (NO_MOVEX) str = "Select * From MVX_CustomerAddress Where CustomerCode = '" + sCustomerCode + "'";

                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetSupplierByCode(string sSupplierCode)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string str = "Exec pr_GetSupplierName '" + sSupplierCode + "'";
                if (NO_MOVEX) str = "Select * From MVX_Supplier Where SupplierCode = '" + sSupplierCode + "'";

                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerByCode(string sCustomerCode)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string str = "Exec pr_GetCustomerName '" + sCustomerCode + "'";
                if (NO_MOVEX) str = "Select * From MVX_Customer Where CustomerCode = '" + sCustomerCode + "'";

                return this.GetDataTable(str, new object[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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