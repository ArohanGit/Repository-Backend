using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ASCommon
{
    public class Utilities
    {
        #region "Crystal Report Generation"

        public static string GetLoginUserConnectionString()
        {
            string sConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            return sConnectionString;
        }

        private static CrystalDecisions.Web.CrystalReportSource GetCrystalReportSource(string rptFileName)
        {
            //AppManager.Configuration oAppConfig = new AppManager.Configuration(null);
            //oAppConfig = App.Configuration;
            //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(oAppConfig.ConnectionString);

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(GetLoginUserConnectionString());
            string sUID = builder.UserID;
            string sPWD = builder.Password;
            string sSER = builder.DataSource;
            string sDTB = builder.InitialCatalog;

            //Namespace Required
            //using CrystalDecisions.Web;
            //using CrystalDecisions.Shared;

            CrystalDecisions.Web.CrystalReportSource CRSource = new CrystalDecisions.Web.CrystalReportSource();
            CRSource.Report.FileName = rptFileName;
            CRSource.ReportDocument.Refresh();
            CRSource.Report.FileName = rptFileName;

            if (CRSource.ReportDocument.HasSavedData)
            {
                CRSource.ReportDocument.ReportOptions.EnableSaveDataWithReport = false;
                CRSource.ReportDocument.ReportOptions.EnableUseDummyData = false;
            }
            CRSource.ReportDocument.DataSourceConnections.Clear();
            CRSource.ReportDocument.SetDatabaseLogon(sUID, sPWD);

            return CRSource;
        }

        private static string Export(CrystalDecisions.Web.CrystalReportSource CRSource, string sParams)
        {
            //string ReportPath = "";
            try
            {
                CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                string rptName = null;
                CRSource.ReportDocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                string mTempDirPath;
                CRSource.ReportDocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;

                string DirPath = HttpContext.Current.Server.MapPath("~/Temp/");
                DirectoryInfo diFiles = new DirectoryInfo(DirPath);
                FileInfo[] fileInfo = diFiles.GetFiles();
                rptName = sParams + DateTime.Now.ToString("HH-mm-ss-mm.fff") + ".pdf";
                mTempDirPath = HttpContext.Current.Server.MapPath("~/Temp/");
                DiskOpts.DiskFileName = mTempDirPath + "\\" + rptName;
                CRSource.ReportDocument.ExportOptions.ExportDestinationOptions = DiskOpts;

                /**************************************/
                //Delete Existing Files From                
                foreach (FileInfo f in fileInfo) { if (f.CreationTime < DateTime.Now.Date) { f.Delete(); } }
                /**************************************/

                CRSource.ReportDocument.Export();
                //if (HttpContext.Current == null)
                //{
                //    ReportPath = DirPath + rptName;
                //}
                //else
                //{
                //    ReportPath = HttpContext.Current.Server.MapPath("~/Temp/" + rptName);
                //}
                return rptName;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string GeneratePrescriptionReport(string rptFileName, string[] sParams)
        {
            string ReportPath = "";
            try
            {
                //DateTime FromDate = Convert.ToDateTime(sParams[0]);
                //DateTime ToDate = Convert.ToDateTime(sParams[1]);
                int nPatientVisitID = Convert.ToInt32(sParams[0]);

                CrystalDecisions.Web.CrystalReportSource CRSource = GetCrystalReportSource(rptFileName);
                //CRSource.ReportDocument.SetParameterValue("FromDate", FromDate);
                //CRSource.ReportDocument.SetParameterValue("ToDate", ToDate);
                CRSource.ReportDocument.SetParameterValue("@PatientVisitID", nPatientVisitID);

                ReportPath = Export(CRSource, "Prescription");

                string DirPath = HttpContext.Current.Server.MapPath("~/Temp/");
                if (HttpContext.Current == null)
                {
                    ReportPath = DirPath + ReportPath;
                }
                else
                {
                    ReportPath = HttpContext.Current.Server.MapPath("~/Temp/" + ReportPath);
                }
                return ReportPath;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string GenerateFromToDateWiseReport(string rptFileName, string[] sParams, string sPDFName)
        {
            string ReportPath = "";
            try
            {
                DateTime FromDate = Convert.ToDateTime(sParams[0]);
                DateTime ToDate = Convert.ToDateTime(sParams[1]);

                CrystalDecisions.Web.CrystalReportSource CRSource = GetCrystalReportSource(rptFileName);
                CRSource.ReportDocument.SetParameterValue("@VisitFrom", FromDate);
                CRSource.ReportDocument.SetParameterValue("@VisitTo", ToDate);

                ReportPath = Export(CRSource, sPDFName);

                string DirPath = HttpContext.Current.Server.MapPath("~/Temp/");
                if (HttpContext.Current == null)
                {
                    ReportPath = DirPath + ReportPath;
                }
                else
                {
                    ReportPath = HttpContext.Current.Server.MapPath("~/Temp/" + ReportPath);
                }
                return ReportPath;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string GenerateReport(string rptFileName, string[] sParams, string sPDFName)
        {
            string ReportPath = "";
            try
            {
                CrystalDecisions.Web.CrystalReportSource CRSource = GetCrystalReportSource(rptFileName);
                ReportPath = Export(CRSource, sPDFName);

                string DirPath = HttpContext.Current.Server.MapPath("~/Temp/");
                if (HttpContext.Current == null)
                {
                    ReportPath = DirPath + ReportPath;
                }
                else
                {
                    ReportPath = HttpContext.Current.Server.MapPath("~/Temp/" + ReportPath);
                }
                return ReportPath;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string GenerateQueryReport(string rptFileName, string[] sParams)
        {
            string ReportPath = "";
            try
            {
                DateTime FromDate = Convert.ToDateTime(sParams[0]);
                DateTime ToDate = Convert.ToDateTime(sParams[1]);
                CrystalDecisions.Web.CrystalReportSource CRSource = GetCrystalReportSource(rptFileName);
                CRSource.ReportDocument.SetParameterValue("FromDate", FromDate);
                CRSource.ReportDocument.SetParameterValue("ToDate", ToDate);
                int fDate = 0;
                int tDate = 0;
                fDate = Convert.ToInt32((FromDate.Date.Year) * 10000 + (FromDate.Date.Month) * 100 + FromDate.Date.Day);
                tDate = Convert.ToInt32((ToDate.Date.Year) * 10000 + (ToDate.Date.Month) * 100 + ToDate.Date.Day);
                string sScholarshipYear = sParams[2];
                string sStudentName = sParams[3];
                string sDonorName = sParams[4];
                string sStreamName = sParams[5];
                string sCourseName = sParams[6];
                string sTrainer = sParams[7];
                string sSelFormula = "";
                sSelFormula = sSelFormula + "(year({vw_TrainingDet.TrainingStartDate})*10000+Month({vw_TrainingDet.TrainingStartDate})*100+Day({vw_TrainingDet.TrainingStartDate}) >= " + fDate;
                if (!string.IsNullOrEmpty(sSelFormula)) sSelFormula = sSelFormula + " And ";
                sSelFormula = sSelFormula + "year({vw_TrainingDet.TrainingStartDate})*10000+Month({vw_TrainingDet.TrainingStartDate})*100+Day({vw_TrainingDet.TrainingStartDate}) <= " + tDate + ")";
                if (!string.IsNullOrEmpty(sScholarshipYear))
                {
                    if (!string.IsNullOrEmpty(sSelFormula)) sSelFormula = sSelFormula + " And ";
                    sSelFormula = sSelFormula + "{vw_TrainingDet.ScholarshipYear}= '" + sScholarshipYear + "'";
                }
                if (!string.IsNullOrEmpty(sStudentName))
                {
                    if (!string.IsNullOrEmpty(sSelFormula)) sSelFormula = sSelFormula + " And ";
                    sSelFormula = sSelFormula + "{vw_TrainingDet.StudentName}= '" + sStudentName + "'";
                }
                if (!string.IsNullOrEmpty(sDonorName))
                {
                    if (!string.IsNullOrEmpty(sSelFormula)) sSelFormula = sSelFormula + " And ";
                    sSelFormula = sSelFormula + "{vw_TrainingDet.DonorName}= '" + sDonorName + "'";
                }
                if (!string.IsNullOrEmpty(sStreamName))
                {
                    if (!string.IsNullOrEmpty(sSelFormula)) sSelFormula = sSelFormula + " And ";
                    sSelFormula = sSelFormula + "{vw_TrainingDet.StreamName}= '" + sStreamName + "'";
                }
                if (!string.IsNullOrEmpty(sCourseName))
                {
                    if (!string.IsNullOrEmpty(sSelFormula)) sSelFormula = sSelFormula + " And ";
                    sSelFormula = sSelFormula + "{vw_TrainingDet.TrainingCourse}= '" + sCourseName + "'";
                }
                if (!string.IsNullOrEmpty(sTrainer))
                {
                    if (!string.IsNullOrEmpty(sSelFormula)) sSelFormula = sSelFormula + " And ";
                    sSelFormula = sSelFormula + "{vw_TrainingDet.Trainer}= '" + sTrainer + "'";
                }
                CRSource.ReportDocument.RecordSelectionFormula = sSelFormula;
                ReportPath = Export(CRSource, "StudentWiseTrainingReport");
                string DirPath = HttpContext.Current.Server.MapPath("~/Temp/");
                if (HttpContext.Current == null)
                {
                    ReportPath = DirPath + ReportPath;
                }
                else
                {
                    ReportPath = HttpContext.Current.Server.MapPath("~/Temp/" + ReportPath);
                }
                return ReportPath;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public static string GenerateGSTChallanReport(int nGSTChallanID)
        {
            string ReportPath = "";
            try
            {
                string rptFileName = "~/RPT/GSTChallanReport.rpt";
                CrystalDecisions.Web.CrystalReportSource CRSource = GetCrystalReportSource(rptFileName);
                CRSource.ReportDocument.SetParameterValue("GSTChallanID", nGSTChallanID);
                ReportPath = Export(CRSource, "GSTChallan");


                return ReportPath;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        #endregion
    }
}