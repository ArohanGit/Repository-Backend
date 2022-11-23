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
    public partial class RepositoryController : ApiController
    {
        #region "Additional Services"

        [HttpGet]
        [Route("loadlist")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult LoadList()
        {
            try
            {
                //List<Repository> oRepositoryList = new Repository().LoadList().ToList();
                //List<RepositoryDTO> oRepositoryDTOList = Mapper.Map<List<Repository>, List<RepositoryDTO>>(oRepositoryList);
                //return Ok(oRepositoryDTOList);
                return Ok();
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpPost]
        [Route("IsDuplicateRepositoryName")]
        public IHttpActionResult IsDuplicateRepositoryName([FromBody] RepositoryDTO oRepositoryDTO)
        {
            try
            {
                Repository oRepository = new Repository().Load(where:
                    "RepositoryName='" + oRepositoryDTO.RepositoryName + "'" +
                    (oRepositoryDTO.RepositoryID.HasValue ? " And RepositoryID <> " + oRepositoryDTO.RepositoryID : ""));
                if (oRepository != null) return Ok("Y");
                return Ok("N");
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpGet]
        [Route("GetRepositoryList/{mode}")]
        public IHttpActionResult GetRepositoryList(string mode)
        {
            try
            {
                DataTable dt = new Repository().GetRepositoryList(mode);
                return Ok(new { Items = dt, Count = dt.Rows.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpGet]
        [Route("GetRepositoryCounts")]
        public IHttpActionResult GetRepositoryCounts()
        {
            try
            {
                DataTable dt = new Repository().GetRepositoryCounts();
                return Ok(new { Items = dt, Count = dt.Rows.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpGet]
        [Route("GetRepositorysNearingExpiry")]
        public IHttpActionResult GetRepositorysNearingExpiry()
        {
            try
            {
                DataTable dt = new Repository().RepositorysNearingExpiry();
                return Ok(new { Items = dt, Count = dt.Rows.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpPost]
        [Route("CopyRepository")]
        public IHttpActionResult CopyRepository([FromBody] RepositoryDTO oRepositoryDTO)
        {
            try
            {
                Repository oRepository = Mapper.Map<RepositoryDTO, Repository>(oRepositoryDTO);
                oRepository = new Repository().CopyRepository(oRepository);
                oRepositoryDTO = Mapper.Map<Repository, RepositoryDTO>(oRepository);
                return Ok(oRepositoryDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpPost] //SaveList
        [Route("SaveList")]
        public IHttpActionResult SaveList([FromBody] List<RepositoryDTO> oRepositoryDTOList)
        {
            try
            {
                if (oRepositoryDTOList == null || oRepositoryDTOList.Count <= 0) BadRequest("No DTO passed");
                List<Repository> oRepositoryList = Mapper.Map<List<RepositoryDTO>, List<Repository>>(oRepositoryDTOList); //Mapper code            
                oRepositoryList = new Repository().SaveList(oRepositoryList);
                oRepositoryDTOList = Mapper.Map<List<Repository>, List<RepositoryDTO>>(oRepositoryList);
                return Ok(oRepositoryDTOList);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpPost] //Approve
        [Route("Approve")]
        public IHttpActionResult Approve([FromBody] RepositoryDTO oRepositoryDTO)
        {
            try
            {
                if (oRepositoryDTO == null) BadRequest("No DTO passed");
                Repository oRepository = Mapper.Map<RepositoryDTO, Repository>(oRepositoryDTO); //Mapper code            

                object NoOfLvl = new NotificationTo().Count(new NotificationTo() { RepositoryID = oRepository.RepositoryID });
                if (Convert.ToInt32(NoOfLvl) == oRepository.ApprovalLevel)
                {
                    oRepository.IsApproved = "Y";
                }
                else
                {
                    oRepository.ApprovalLevel += 1;
                }

                object AppLvl = new Repository().Scalar("AVG", "ApprovalLevel", new Repository() { RepositoryID = oRepository.RepositoryID });
                oRepository.Save();

                // Send Mail to Next Level.
                if (Convert.ToInt32(AppLvl) != oRepository.ApprovalLevel)
                {
                    Repository.SendNotification(oRepository);
                }
                else
                // Send Final Approval Mail
                if (oRepository.ApprovalLevel == Convert.ToInt32(NoOfLvl))
                {
                    Repository.SendFinalApprovalNotification(oRepository);
                }

                oRepositoryDTO = Mapper.Map<Repository, RepositoryDTO>(oRepository);
                return Ok(oRepositoryDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpPost] //Reject
        [Route("Reject")]
        public IHttpActionResult Reject([FromBody] RepositoryDTO oRepositoryDTO)
        {
            try
            {
                if (oRepositoryDTO == null) BadRequest("No DTO passed");
                Repository oRepository = Mapper.Map<RepositoryDTO, Repository>(oRepositoryDTO); //Mapper code

                oRepository.ApprovalLevel = 0;
                oRepository.Save();

                // Send Notification
                NotificationTo oNotificationTo = new NotificationTo().Load(where: "RepositoryID=" + oRepository.RepositoryID + " And ApproverLevel=" + oRepositoryDTO.ApprovalLevel);
                if (oNotificationTo == null) { oRepositoryDTO = Mapper.Map<Repository, RepositoryDTO>(oRepository); return Ok(oRepositoryDTO); }
                Repository.SendRejectNotification(oRepository, oNotificationTo.WebUserID);

                oRepositoryDTO = Mapper.Map<Repository, RepositoryDTO>(oRepository);
                return Ok(oRepositoryDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // POST api/Repository
        /// <summary>
        /// Save Repository
        /// </summary>
        /// <remarks>Add new Repository</remarks>
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
                RepositoryDTO oRepositoryDTO = new RepositoryDTO();

                string sRepositoryId = (System.Web.HttpContext.Current.Request.Form.GetValues("RepositoryID") != null && System.Web.HttpContext.Current.Request.Form.GetValues("RepositoryID")[0] != "null" ? System.Web.HttpContext.Current.Request.Form.GetValues("RepositoryID")[0] : null);
                Repository oRepository = new Repository();
                if (!string.IsNullOrEmpty(sRepositoryId)) oRepository = new Repository().Load(sRepositoryId, false);
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                return Ok();

                oRepository.RepositoryName = System.Web.HttpContext.Current.Request.Form.GetValues("RepositoryName")[0];
                oRepository.RepositoryDescr = System.Web.HttpContext.Current.Request.Form.GetValues("RepositoryDescr")[0];
                if (System.Web.HttpContext.Current.Request.Form.GetValues("ValidFrom")[0] != null)
                {
                    DateTime FromDt = Convert.ToDateTime(DateTime.ParseExact(System.Web.HttpContext.Current.Request.Form.GetValues("ValidFrom")[0].Substring(0, 24), "ddd MMM dd yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
                    oRepository.ValidFrom = FromDt;
                }

                if (System.Web.HttpContext.Current.Request.Form.GetValues("ValidTo")[0] != null)
                {
                    DateTime ToDt = Convert.ToDateTime(DateTime.ParseExact(System.Web.HttpContext.Current.Request.Form.GetValues("ValidTo")[0].Substring(0, 24), "ddd MMM dd yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
                    oRepository.ValidTo = ToDt;
                }
                oRepository.CreatedBy = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form.GetValues("CreatedBy")[0]);
                oRepository.UpdtedBy = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form.GetValues("UpdtedBy")[0]);
                oRepository.NotificationDays = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form.GetValues("NotificationDays")[0]);

                /*
                string NotificationToUserIDs = System.Web.HttpContext.Current.Request.Form.GetValues("NotificationToUserIDs")[0];
                //oRepository.ValidFrom = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form.GetValues("ValidFrom")[0]) ? (DateTime?)Convert.ToDateTime(System.Web.HttpContext.Current.Request.Form.GetValues("ValidFrom")[0]) : null;
                //oRepository.ValidTo = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form.GetValues("ValidTo")[0]) ? (DateTime?)Convert.ToDateTime(System.Web.HttpContext.Current.Request.Form.GetValues("ValidTo")[0]) : null;
                //oRepository.CreatedOn = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form.GetValues("CreatedOn")[0]) ? (DateTime?)Convert.ToDateTime(System.Web.HttpContext.Current.Request.Form.GetValues("CreatedOn")[0]) : null;
                //oRepository.UpdatedOn = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form.GetValues("UpdatedOn")[0]) ? (DateTime?)Convert.ToDateTime(System.Web.HttpContext.Current.Request.Form.GetValues("UpdatedOn")[0]) : null;

                // Upload File
                if (hfc.Count > 0)
                {
                    string uploadPath = HttpContext.Current.Server.MapPath("~/Repositorys/");
                    System.Web.HttpPostedFile hpf = hfc[0];
                    string sFileName = hpf.FileName;

                    int? nFileSrl = 0;
                    if (!nFileSrl.HasValue)
                    {
                        Object objFileSrl = new Repository().Max("FileSrl");
                        nFileSrl = (objFileSrl == null ? 1 : Convert.ToInt32(objFileSrl) + 1);
                    }

                    // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
                    string sUploadedFile = uploadPath + nFileSrl.ToString() + Path.GetExtension(sFileName);
                    hpf.SaveAs(sUploadedFile);

                    // Repository Info
                    string sExtension = Path.GetExtension(sUploadedFile);
                    string _fileName = Path.GetFileName(sUploadedFile);
                    string fileName = sFileName.Replace(sExtension.ToLower(), "");
                    string filePath = "Repositorys/" + nFileSrl.ToString() + sExtension;
                    //oRepository.version = 0;
                    //oRepository.Extension = sExtension;
                    //oRepository.FilePath = filePath;
                    //oRepository.FileSrl = nFileSrl;
                    oRepository.IsDelete = "N";
                }

                oRepository.Save();

                // Save Childrens
                oRepository.SaveChildren(NotificationToUserIDs);

                if (!oRepository.IsValid) return BadRequest(oRepository.Errors.ToModelState());
                oRepository = new Repository().Load(oRepository.RepositoryID, true);
                oRepositoryDTO = Mapper.Map<Repository, RepositoryDTO>(oRepository);
                */

                return Ok(oRepositoryDTO);
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
                //if (string.IsNullOrEmpty(loginInfoDTO.File)) return Ok(new RepositoryDTO());

                //Repository oRepository = new Repository().Load(where: "WebRepositoryID = '" + loginInfoDTO.RepositoryName + "'", withChildren: true);
                //if (oRepository == null) return NotFound();

                //RepositoryDTO oRepositoryDTO = Mapper.Map<Repository, RepositoryDTO>(oRepository);                
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
        public IHttpActionResult Validate([FromBody] RepositoryDTO oRepositoryDTO)
        {
            try
            {
                ValidationObj oValidationObj = new ValidationObj() { IsValid = "Y", ErrorMessage = "" };
                //if (oRepositoryDTO == null) BadRequest("No DTO passed");
                //Repository oRepository = new Repository().Load(where: "WebRepositoryID='" + oRepositoryDTO.RepositoryID + "'" + (oRepositoryDTO.RepositoryID.HasValue ? " And RepositoryID <> " + oRepositoryDTO.RepositoryID : ""));
                //if (oRepository != null) { oValidationObj.IsValid = "N"; oValidationObj.ErrorMessage = "AD ID already exists"; }
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
