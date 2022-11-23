using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Cors;
using System.Web.Http.Cors;
using AutoMapper;
using System.Web.Http.Description;
using FileRepository.BusinessObjects;
using System.IO;
using System.Data;
using System.Web;

namespace FileRepositoryAPI.WebAPI
{
    public partial class FilesController : ApiController
    {
        #region "Additional Services"

        [HttpGet]
        [Route("loadlist")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult LoadList()
        {
            try
            {
                //List<Files> oFilesList = new Files().LoadList().ToList();
                //List<FilesDTO> oFilesDTOList = Mapper.Map<List<Files>, List<FilesDTO>>(oFilesList);
                //return Ok(oFilesDTOList);
                return Ok();
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }


        [HttpPost] //SaveList
        [Route("SaveList")]
        public IHttpActionResult SaveList([FromBody] List<FilesDTO> oFilesDTOList)
        {
            try
            {
                if (oFilesDTOList == null || oFilesDTOList.Count <= 0) BadRequest("No DTO passed");
                List<Files> oFilesList = Mapper.Map<List<FilesDTO>, List<Files>>(oFilesDTOList); //Mapper code            
                oFilesList = new Files().SaveList(oFilesList);
                oFilesDTOList = Mapper.Map<List<Files>, List<FilesDTO>>(oFilesList);
                return Ok(oFilesDTOList);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // POST api/Files
        /// <summary>
        /// Save Files
        /// </summary>
        /// <remarks>Add new Files</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost] //Save
        [Route("UploadRepositoryFiles")]
        [AppAuthenticationFilter(false)]
        public IHttpActionResult UploadRepositoryFiles()
        {
            try
            {
                FilesDTO oFilesDTO = new FilesDTO();

                RepositoryDTO oRepositoryDTO = new RepositoryDTO();

                string sRepositoryId = (System.Web.HttpContext.Current.Request.Form.GetValues("RepositoryID") != null && System.Web.HttpContext.Current.Request.Form.GetValues("RepositoryID")[0] != "null" ? System.Web.HttpContext.Current.Request.Form.GetValues("RepositoryID")[0] : null);
                Repository oRepository = new Repository();
                if (!string.IsNullOrEmpty(sRepositoryId)) oRepository = new Repository().Load(sRepositoryId, false);
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

                int? nCreatedBy, nUpdatedBy;
                nCreatedBy = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form.GetValues("CreatedBy")[0]);
                nUpdatedBy = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form.GetValues("UpdtedBy")[0]);

                List<Files> oFileList = new List<Files>();
                string uploadPath = HttpContext.Current.Server.MapPath("~/Files/");

                // Upload File
                if (hfc.Count > 0)
                {
                    //foreach (System.Web.HttpPostedFile hpf in hfc)
                    //{
                        System.Web.HttpPostedFile hpf = hfc[0];
                        string sFileName = hpf.FileName;

                        // Max Srl
                        int? nFileSrl = 0;
                        Object objFileSrl = new Files().Max("FileSrl");
                        nFileSrl = (objFileSrl == null ? 1 : Convert.ToInt32(objFileSrl) + 1);

                        // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
                        string sUploadedFile = uploadPath + nFileSrl.ToString() + Path.GetExtension(sFileName);
                        hpf.SaveAs(sUploadedFile);

                        string sExtension = Path.GetExtension(sUploadedFile);
                        //string _fileName = Path.GetFileName(sUploadedFile);
                        string fileName = sFileName.Replace(sExtension.ToLower(), "");
                        string filePath = "Files/" + nFileSrl.ToString() + sExtension;

                        // Files
                        Files oFiles = new Files();
                        oFiles.RepositoryID = oRepository.RepositoryID;
                        oFiles.FileName = fileName;
                        oFiles.FileDescr = fileName;
                        oFiles.Extension = sExtension;
                        oFiles.FilePath = filePath;
                        oFiles.FileSrl = nFileSrl;
                        oFiles.IsDelete = "N";
                        oFiles.CreatedBy = nCreatedBy;
                        oFiles.CreatedOn = DateTime.Now;
                        oFiles.UpdtedBy = nUpdatedBy;
                        oFiles.UpdatedOn = DateTime.Now;
                        oFileList.Add(oFiles);
                    //}
                }

                oFileList = new Files().SaveList(oFileList);
                List<FilesDTO> oFilesDTOList = new List<FilesDTO>();
                oFilesDTOList = Mapper.Map<List<Files>, List<FilesDTO>>(oFileList);
                return Ok(oFilesDTOList);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*. =>" + ex.Message, ex);
                throw new HttpResponseException(msg);
            }
        }

        #endregion

        #region "Helper Functions"

        [HttpPost]
        [Route("GetByUniqueKey")]
        public IHttpActionResult GetByUniqueKey([FromBody] LogInfoDTO loginInfoDTO)
        {
            try
            {
                //if (string.IsNullOrEmpty(loginInfoDTO.File)) return Ok(new FilesDTO());

                //Files oFiles = new Files().Load(where: "WebFilesID = '" + loginInfoDTO.FilesName + "'", withChildren: true);
                //if (oFiles == null) return NotFound();

                //FilesDTO oFilesDTO = Mapper.Map<Files, FilesDTO>(oFiles);                
                return Ok();
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpPost] //Validate
        [Route("Validate")]
        public IHttpActionResult Validate([FromBody] FilesDTO oFilesDTO)
        {
            try
            {
                ValidationObj oValidationObj = new ValidationObj() { IsValid = "Y", ErrorMessage = "" };
                //if (oFilesDTO == null) BadRequest("No DTO passed");
                //Files oFiles = new Files().Load(where: "WebFilesID='" + oFilesDTO.FilesID + "'" + (oFilesDTO.FilesID.HasValue ? " And FilesID <> " + oFilesDTO.FilesID : ""));
                //if (oFiles != null) { oValidationObj.IsValid = "N"; oValidationObj.ErrorMessage = "AD ID already exists"; }
                return Ok(oValidationObj);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Repository/GetRepositoryFiles/id
        [HttpGet]
        [Route("GetRepositoryFiles/{repositoryid:int}")]
        [AppAuthenticationFilter]
        public IHttpActionResult GetRepositoryFiles(int repositoryid)
        {
            try
            {
                List<Files> oFileList = new Files().LoadList(where: "RepositoryID=" + repositoryid).ToList();
                List<FilesDTO> oFilesToDTOList = Mapper.Map<List<Files>, List<FilesDTO>>(oFileList);
                return Ok(new { Items = oFilesToDTOList, Count = oFilesToDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        #endregion

        #region "Remarks"
        #endregion
    }
}
