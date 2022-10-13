using System;
using System.Configuration;
using System.Web;
using Arohan.Data;
//using Microsoft.AspNetCore.Http;

namespace FileRepository.BusinessObjects
{
    public partial class AppDb : Database
    {
        const string TABLEPREFIX = ""; // ""; 

        public string CompanyCode { get { return GetCompanyCode(); } }
        string GetCompanyCode()
        {
            string companyCode = "";
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["CompanyCode"] != null)
                companyCode = HttpContext.Current.Session["CompanyCode"].ToString(); // ThisCompany.CompanyCode;
            return companyCode;
        }

        public string TablePrefix { get { return string.IsNullOrEmpty(CompanyCode) ? TABLEPREFIX : "C" + CompanyCode; } }

        ////**New Code : For change connection string depend on Company
        //public string CompanyID { get { return GetCompanyID(); } }
        //string GetCompanyID()
        //{
        //    string CompanyID = "";
        //    if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["CompanyID"] != null)
        //        CompanyID = HttpContext.Current.Session["CompanyID"].ToString(); // ThisCompany.CompanyID;
        //    return CompanyID;
        //}
        ////**

        public string ConnectionString { get { return GetConnectionString(); } }
        string GetConnectionString()
        {
            try
            {
                //return ThisCompany.ConnectionString;
                return ConfigurationManager.ConnectionStrings[CompanyCode + "ConnectionString"].ConnectionString; //+ CompanyID
            }
            catch (Exception ex)
            {
                throw new DbException("Error locating connection string in Web.Config", ex);
            }
        }

        public AppDb() : base() { connectionString = ConnectionString; tablePrefix = string.IsNullOrEmpty(CompanyCode) ? TABLEPREFIX : "C" + CompanyCode; }
    }

    public partial class LinkedDb : Database
    {
        const string TABLEPREFIX = ""; //"C01"

        public string CompanyCode { get { return GetCompanyCode(); } }
        string GetCompanyCode()
        {
            string companyCode = "";
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["CompanyCode"] != null)
                companyCode = HttpContext.Current.Session["CompanyCode"].ToString(); // ThisCompany.CompanyCode;
            return companyCode;
        }

        public string TablePrefix { get { return string.IsNullOrEmpty(CompanyCode) ? TABLEPREFIX : "C" + CompanyCode; } }

        public string ConnectionString { get { return GetConnectionString(); } }
        string GetConnectionString()
        {
            try
            {
                //return ThisCompany.ConnectionString;
                return ConfigurationManager.ConnectionStrings[CompanyCode + "ALConnectionString"].ConnectionString;
            }
            catch (Exception ex)
            {
                throw new DbException("Error locating connection string in Web.Config", ex);
            }
        }


        public LinkedDb() : base() { connectionString = ConnectionString; tablePrefix = string.IsNullOrEmpty(CompanyCode) ? TABLEPREFIX : "C" + CompanyCode; }

    }

    public partial class EmpDb : Database
    {
        const string TABLEPREFIX = ""; //"C01"

        public string CompanyCode { get { return GetCompanyCode(); } }
        string GetCompanyCode()
        {
            string companyCode = "";
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["CompanyCode"] != null)
                companyCode = HttpContext.Current.Session["CompanyCode"].ToString(); // ThisCompany.CompanyCode;
            return companyCode;
        }

        public string TablePrefix { get { return string.IsNullOrEmpty(CompanyCode) ? TABLEPREFIX : "C" + CompanyCode; } }

        public string ConnectionString { get { return GetConnectionString(); } }
        string GetConnectionString()
        {
            try
            {
                //return ThisCompany.ConnectionString;
                return ConfigurationManager.ConnectionStrings[CompanyCode + "EMPIREConnectionString"].ConnectionString;
            }
            catch (Exception ex)
            {
                throw new DbException("Error locating connection string in Web.Config", ex);
            }
        }


        public EmpDb() : base() { connectionString = ConnectionString; tablePrefix = string.IsNullOrEmpty(CompanyCode) ? TABLEPREFIX : "C" + CompanyCode; }

    }

    public partial class AppTransactions : Transaction
    {
        public AppTransactions() : base(new AppDb()) { }
    }

}
