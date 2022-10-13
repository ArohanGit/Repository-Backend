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
    public partial class DocumentController : ApiController
    {
        #region "Additional Services"

        [HttpGet]
        [Route("loadlist")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult LoadList()
        {
            try
            {
                //List<Document> oDocumentList = new Document().LoadList().ToList();
                //List<DocumentDTO> oDocumentDTOList = Mapper.Map<List<Document>, List<DocumentDTO>>(oDocumentList);
                //return Ok(oDocumentDTOList);
                return Ok();
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpGet]
        [Route("GetDocumentsNearingExpiry")]
        public IHttpActionResult GetDocumentsNearingExpiry()
        {
            try
            {
                DataTable dt = new Document().DocumentsNearingExpiry();
                return Ok(new { Items = dt, Count = dt.Rows.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpPost] //SaveList
        [Route("SaveList")]
        public IHttpActionResult SaveList([FromBody] List<DocumentDTO> oDocumentDTOList)
        {
            try
            {
                if (oDocumentDTOList == null || oDocumentDTOList.Count <= 0) BadRequest("No DTO passed");
                List<Document> oDocumentList = Mapper.Map<List<DocumentDTO>, List<Document>>(oDocumentDTOList); //Mapper code            
                oDocumentList = new Document().SaveList(oDocumentList);
                oDocumentDTOList = Mapper.Map<List<Document>, List<DocumentDTO>>(oDocumentList);
                return Ok(oDocumentDTOList);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // POST api/document
        /// <summary>
        /// Save document
        /// </summary>
        /// <remarks>Add new document</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost] //Save
        [Route("SaveFormData")]
        [AppAuthenticationFilter(false)]
        public IHttpActionResult SaveFormData()
        {
            try
            {
                DocumentDTO oDocumentDTO = new DocumentDTO();
                string NotificationToUserIDs = System.Web.HttpContext.Current.Request.Form.GetValues("NotificationToUserIDs")[0];
                string sDocumentId = (System.Web.HttpContext.Current.Request.Form.GetValues("DocumentID") != null && System.Web.HttpContext.Current.Request.Form.GetValues("DocumentID")[0] != "null" ? System.Web.HttpContext.Current.Request.Form.GetValues("DocumentID")[0] : null);
                Document oDocument = new Document();
                if (!string.IsNullOrEmpty(sDocumentId)) oDocument = new Document().Load(sDocumentId, false);
                // if (oDocument == null) return NotFound();
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                oDocument.FileName = System.Web.HttpContext.Current.Request.Form.GetValues("FileName")[0];
                oDocument.FileDescr = System.Web.HttpContext.Current.Request.Form.GetValues("FileDescr")[0];

                if (System.Web.HttpContext.Current.Request.Form.GetValues("ValidFrom")[0] != null)
                {
                    DateTime FromDt = Convert.ToDateTime(DateTime.ParseExact(System.Web.HttpContext.Current.Request.Form.GetValues("ValidFrom")[0].Substring(0, 24), "ddd MMM dd yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
                    oDocument.ValidFrom = FromDt;
                }

                if (System.Web.HttpContext.Current.Request.Form.GetValues("ValidTo")[0] != null)
                {
                    DateTime ToDt = Convert.ToDateTime(DateTime.ParseExact(System.Web.HttpContext.Current.Request.Form.GetValues("ValidTo")[0].Substring(0, 24), "ddd MMM dd yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
                    oDocument.ValidTo = ToDt;
                }
                //oDocument.ValidFrom = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form.GetValues("ValidFrom")[0]) ? (DateTime?)Convert.ToDateTime(System.Web.HttpContext.Current.Request.Form.GetValues("ValidFrom")[0]) : null;
                //oDocument.ValidTo = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form.GetValues("ValidTo")[0]) ? (DateTime?)Convert.ToDateTime(System.Web.HttpContext.Current.Request.Form.GetValues("ValidTo")[0]) : null;
                //oDocument.CreatedOn = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form.GetValues("CreatedOn")[0]) ? (DateTime?)Convert.ToDateTime(System.Web.HttpContext.Current.Request.Form.GetValues("CreatedOn")[0]) : null;
                //oDocument.UpdatedOn = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form.GetValues("UpdatedOn")[0]) ? (DateTime?)Convert.ToDateTime(System.Web.HttpContext.Current.Request.Form.GetValues("UpdatedOn")[0]) : null;

                oDocument.CreatedBy = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form.GetValues("CreatedBy")[0]);
                oDocument.UpdtedBy = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form.GetValues("UpdtedBy")[0]);
                oDocument.NotificationDays = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form.GetValues("NotificationDays")[0]);

                // Upload File
                if (hfc.Count > 0)
                {
                    string uploadPath = HttpContext.Current.Server.MapPath("~/Documents/");
                    System.Web.HttpPostedFile hpf = hfc[0];
                    string sFileName = hpf.FileName;

                    int? nFileSrl = oDocument.FileSrl;
                    if (!nFileSrl.HasValue)
                    {
                        Object objFileSrl = new Document().Max("FileSrl");
                        nFileSrl = (objFileSrl == null ? 1 : Convert.ToInt32(objFileSrl) + 1);
                    }

                    // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
                    string sUploadedFile = uploadPath + nFileSrl.ToString() + Path.GetExtension(sFileName);
                    hpf.SaveAs(sUploadedFile);

                    // Document Info
                    string sExtension = Path.GetExtension(sUploadedFile);
                    string _fileName = Path.GetFileName(sUploadedFile);
                    string fileName = sFileName.Replace(sExtension.ToLower(), "");
                    string filePath = "Documents/" + nFileSrl.ToString() + sExtension;
                    oDocument.version = 0;
                    oDocument.Extension = sExtension;
                    oDocument.FilePath = filePath;
                    oDocument.FileSrl = nFileSrl;
                    oDocument.IsDelete = "N";
                }

                oDocument.Save();

                // Save Childrens
                oDocument.SaveChildren(NotificationToUserIDs);

                if (!oDocument.IsValid) return BadRequest(oDocument.Errors.ToModelState());
                oDocument = new Document().Load(oDocument.DocumentID, true);
                oDocumentDTO = Mapper.Map<Document, DocumentDTO>(oDocument);

                return Ok(oDocumentDTO);
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
                //if (string.IsNullOrEmpty(loginInfoDTO.File)) return Ok(new DocumentDTO());

                //Document oDocument = new Document().Load(where: "WebDocumentID = '" + loginInfoDTO.DocumentName + "'", withChildren: true);
                //if (oDocument == null) return NotFound();

                //DocumentDTO oDocumentDTO = Mapper.Map<Document, DocumentDTO>(oDocument);                
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
        public IHttpActionResult Validate([FromBody] DocumentDTO oDocumentDTO)
        {
            try
            {
                ValidationObj oValidationObj = new ValidationObj() { IsValid = "Y", ErrorMessage = "" };
                //if (oDocumentDTO == null) BadRequest("No DTO passed");
                //Document oDocument = new Document().Load(where: "WebDocumentID='" + oDocumentDTO.DocumentID + "'" + (oDocumentDTO.DocumentID.HasValue ? " And DocumentID <> " + oDocumentDTO.DocumentID : ""));
                //if (oDocument != null) { oValidationObj.IsValid = "N"; oValidationObj.ErrorMessage = "AD ID already exists"; }
                return Ok(oValidationObj);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        #endregion

        #region "Remarks"
        #endregion
    }
}
