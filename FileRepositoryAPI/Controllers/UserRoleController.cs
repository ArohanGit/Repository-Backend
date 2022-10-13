
  
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

namespace FileRepositoryAPI.WebAPI
{
    public partial class UserRoleController : ApiController
    {

        #region "Additional Services"

        [HttpGet]
        [Route("loadlist")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult LoadList()
        {
            try
            {
                //List<UserRole> oUserRoleList = new UserRole().LoadList().ToList();
                //List<UserRoleDTO> oUserRoleDTOList = Mapper.Map<List<UserRole>, List<UserRoleDTO>>(oUserRoleList);
                //return Ok(oUserRoleDTOList);
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
        public IHttpActionResult SaveList([FromBody] List<UserRoleDTO> oUserRoleDTOList)
        {
            try
            {
                if (oUserRoleDTOList == null || oUserRoleDTOList.Count <= 0) BadRequest("No DTO passed");
                List<UserRole> oUserRoleList = Mapper.Map<List<UserRoleDTO>, List<UserRole>>(oUserRoleDTOList); //Mapper code            
                oUserRoleList = new UserRole().SaveList(oUserRoleList);
                oUserRoleDTOList = Mapper.Map<List<UserRole>, List<UserRoleDTO>>(oUserRoleList);
                return Ok(oUserRoleDTOList);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        #endregion

        #region "Helper Functions"
        #endregion

        #region "Remarks"
        #endregion

    }
}
