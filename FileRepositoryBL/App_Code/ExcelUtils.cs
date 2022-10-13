using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using Excel;
using System.Configuration;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace FileRepository.BusinessObjects
{
    public partial class ExcelUtils
    {
        public static DataSet GetDataTableFromExcel(string filePath)
        {
            try
            {
                //Read Data From Excel Used : Excel.dll
                //Ref : https://exceldatareader.codeplex.com/

                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
                IExcelDataReader excelReader;

                //string extension = Path.GetExtension(filePath);
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                //excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                DataSet result;
                excelReader.IsFirstRowAsColumnNames = true;
                result = excelReader.AsDataSet();

                ////4. DataSet - Create column names from first row
                //excelReader.IsFirstRowAsColumnNames = true;
                //result = excelReader.AsDataSet();
                ////5. Data Reader methods
                //while (excelReader.Read())
                //{
                //    //excelReader.GetInt32(0);
                //}

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string WriteToExcel(string sDataFileName, DataTable dt, string TemplateFilePath, string sGenFileFolderPath)
        {
            try
            {
                //Data Cell
                string sStartCell = "A2";

                //If Template file not exists then return empty path
                if (!File.Exists(TemplateFilePath)) { return ""; }

                //Copy Template in temporary folder with different name.
                string sFileName = sDataFileName + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xlsx";
                string sDestFilePath = sGenFileFolderPath + sFileName;
                File.Copy(TemplateFilePath, sDestFilePath, true);

                //Open File from Temporary folder and Update data.
                FileInfo file = new FileInfo(sDestFilePath);
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.First();
                    excelWorksheet.Cells[sStartCell].LoadFromDataTable(dt, false, TableStyles.None);
                    excelWorksheet.Cells[excelWorksheet.Dimension.Address].AutoFitColumns();
                    excelPackage.Save();
                }

                //Return update file path.
                return sFileName;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string WriteColumnsToExcel(string sDataFileName, DataTable dt, string TemplateFilePath, string sGenFileFolderPath)
        {
            try
            {
                //Data Cell
                string sStartCell = "A1";

                //If Template file not exists then return empty path
                if (!File.Exists(TemplateFilePath)) { return ""; }

                //Copy Template in temporary folder with different name.
                string sFileName = sDataFileName + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xlsx";
                string sDestFilePath = sGenFileFolderPath + sFileName;
                File.Copy(TemplateFilePath, sDestFilePath, true);

                //Open File from Temporary folder and Update data.
                FileInfo file = new FileInfo(sDestFilePath);
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.First();
                    excelWorksheet.Cells[sStartCell].LoadFromDataTable(dt, true, TableStyles.None);
                    excelWorksheet.Cells[excelWorksheet.Dimension.Address].AutoFitColumns();
                    excelPackage.Save();
                }

                //Return update file path.
                return sFileName;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void WriteDataTableToExcelFile(string outputPath, DataTable dt)
        {
            FileInfo fi = new FileInfo(outputPath);
            //if (fi.Exists) { fi.Delete(); }
            //if (!fi.Exists) { fi.Create(); }

            //Delete Existing Files From
            string uploadPath = HttpContext.Current.Server.MapPath("~/temp/");
            DirectoryInfo diFiles = new DirectoryInfo(uploadPath);
            FileInfo[] fileInfo = diFiles.GetFiles();
            try { foreach (FileInfo f in fileInfo) { if (f.CreationTime < DateTime.Now.Date.AddHours(-1)) { f.Delete(); } } }
            catch { }

            using (ExcelPackage pck = new ExcelPackage(fi))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Errors");
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                pck.Save();
            }
        }

        public static void WriteDataTableToExcelFile11(string outputPath, DataTable dt)
        {
            FileInfo fi = new FileInfo(outputPath);
            if (fi.Exists) { fi.Delete(); }
            if (!fi.Exists) { fi.Create(); }

            using (ExcelPackage pck = new ExcelPackage(fi))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Errors");
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                pck.Save();
            }
        }

        /*
        public static string PrepareExcelFile(string sDataFileName, DataTable dt, string TemplateFilePath, string sGenFileFolderPath, facilityModel ofacilityModel)
        {
            try
            {
                //Data Cell
                string sStartCell = "A2";

                //If Template file not exists then return empty path
                if (!File.Exists(TemplateFilePath)) { return ""; }

                //Copy Template in temporary folder with different name.
                string sFileName = sDataFileName + "_" + ofacilityModel.description + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xlsx";
                string sDestFilePath = sGenFileFolderPath + sFileName;
                File.Copy(TemplateFilePath, sDestFilePath, true);

                //Open File from Temporary folder and Update data.
                FileInfo file = new FileInfo(sDestFilePath);
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.First();
                    excelWorksheet.Cells[sStartCell].LoadFromDataTable(dt, true, TableStyles.None);

                    excelWorksheet.Cells[1, 1].Value = "facilityId";
                    excelWorksheet.Cells[1, 2].Value = ofacilityModel.facilityId;

                    excelWorksheet.Cells[1, 3].Value = "modelId";
                    excelWorksheet.Cells[1, 4].Value = ofacilityModel.modelId;

                    excelWorksheet.Cells[1, 5].Value = "dataSourceId";
                    excelWorksheet.Cells[1, 6].Value = ofacilityModel.dataSourceId;
                    
                    excelWorksheet.Cells[excelWorksheet.Dimension.Address].AutoFitColumns();

                    excelWorksheet.Row(1).Hidden = true;
                    excelWorksheet.Row(2).Hidden = true;

                    excelPackage.Save();
                }

                //Return update file path.
                return sFileName;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        */
    }
}
