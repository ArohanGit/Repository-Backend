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
    public partial class NotificationToController : ApiController
    {
        #region "Additional Services"

        [HttpGet]
        [Route("loadlist")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult LoadList()
        {
            try
            {
                //List<NotificationTo> oNotificationToList = new NotificationTo().LoadList().ToList();
                //List<NotificationToDTO> oNotificationToDTOList = Mapper.Map<List<NotificationTo>, List<NotificationToDTO>>(oNotificationToList);
                //return Ok(oNotificationToDTOList);
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
        public IHttpActionResult SaveList([FromBody] List<NotificationToDTO> oNotificationToDTOList)
        {
            try
            {
                if (oNotificationToDTOList == null || oNotificationToDTOList.Count <= 0) BadRequest("No DTO passed");
                List<NotificationTo> oNotificationToList = Mapper.Map<List<NotificationToDTO>, List<NotificationTo>>(oNotificationToDTOList); //Mapper code            
                oNotificationToList = new NotificationTo().SaveList(oNotificationToList);
                oNotificationToDTOList = Mapper.Map<List<NotificationTo>, List<NotificationToDTO>>(oNotificationToList);
                return Ok(oNotificationToDTOList);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
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
                //if (string.IsNullOrEmpty(loginInfoDTO.File)) return Ok(new NotificationToDTO());
                //NotificationTo oNotificationTo = new NotificationTo().Load(where: "WebNotificationToID = '" + loginInfoDTO.NotificationToName + "'", withChildren: true);
                //if (oNotificationTo == null) return NotFound();
                //NotificationToDTO oNotificationToDTO = Mapper.Map<NotificationTo, NotificationToDTO>(oNotificationTo);                
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
        public IHttpActionResult Validate([FromBody] NotificationToDTO oNotificationToDTO)
        {
            try
            {
                ValidationObj oValidationObj = new ValidationObj() { IsValid = "Y", ErrorMessage = "" };
                //if (oNotificationToDTO == null) BadRequest("No DTO passed");
                //NotificationTo oNotificationTo = new NotificationTo().Load(where: "WebNotificationToID='" + oNotificationToDTO.NotificationToID + "'" + (oNotificationToDTO.NotificationToID.HasValue ? " And NotificationToID <> " + oNotificationToDTO.NotificationToID : ""));
                //if (oNotificationTo != null) { oValidationObj.IsValid = "N"; oValidationObj.ErrorMessage = "AD ID already exists"; }
                return Ok(oValidationObj);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Repository/GetNotificationToList/id
        [HttpGet]
        [Route("GetNotificationToList/{repositoryid:int}")]
        [AppAuthenticationFilter]
        public IHttpActionResult GetNotificationToList(int repositoryid)
        {
            try
            {
                List<NotificationTo> oNotificationToList = new NotificationTo().LoadList(where: "RepositoryID=" + repositoryid).ToList();
                List<NotificationToDTO> oNotificationToDTOList = Mapper.Map<List<NotificationTo>, List<NotificationToDTO>>(oNotificationToList);
                oNotificationToList = oNotificationToList.OrderBy(x => x.ApproverLevel).ToList();
                return Ok(new { Items = oNotificationToList, Count = oNotificationToList.Count });
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
