
  
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
    public partial class RoleController : ApiController
    {

        #region "Additional Services"

        [HttpGet]
        [Route("loadlist")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult LoadList()
        {
            try
            {
                //List<Role> oRoleList = new Role().LoadList().ToList();
                //List<RoleDTO> oRoleDTOList = Mapper.Map<List<Role>, List<RoleDTO>>(oRoleList);
                //return Ok(oRoleDTOList);
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
        public IHttpActionResult SaveList([FromBody] List<RoleDTO> oRoleDTOList)
        {
            try
            {
                if (oRoleDTOList == null || oRoleDTOList.Count <= 0) BadRequest("No DTO passed");
                List<Role> oRoleList = Mapper.Map<List<RoleDTO>, List<Role>>(oRoleDTOList); //Mapper code            
                oRoleList = new Role().SaveList(oRoleList);
                oRoleDTOList = Mapper.Map<List<Role>, List<RoleDTO>>(oRoleList);
                return Ok(oRoleDTOList);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        #endregion

        #region "Helper Functions"
        [HttpPost] //Validate
        [Route("Validate")]
        public IHttpActionResult Validate([FromBody] RoleDTO oRoleDTO)
        {
            try
            {
                ValidationObj oValidationObj = new ValidationObj() { IsValid = "Y", ErrorMessage = "" };
                if (oRoleDTO == null) BadRequest("No DTO passed");
                Role oRole = new Role().Load(where: "Name='" + oRoleDTO.Name + "'" + (oRoleDTO.RoleID.HasValue ? " And RoleID <> " + oRoleDTO.RoleID : ""));
                if (oRole != null) { oValidationObj.IsValid = "N"; oValidationObj.ErrorMessage = "Role Name already exists"; }
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
