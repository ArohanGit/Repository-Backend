

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

namespace FileRepositoryAPI.WebAPI
{
    public partial class DepartmentController : ApiController
    {
        #region "Additional Services"

        [HttpGet]
        [Route("loadlist")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult LoadList()
        {
            try
            {
                //List<Department> oDepartmentList = new Department().LoadList().ToList();
                //List<DepartmentDTO> oDepartmentDTOList = Mapper.Map<List<Department>, List<DepartmentDTO>>(oDepartmentList);
                //return Ok(oDepartmentDTOList);
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
        public IHttpActionResult SaveList([FromBody] List<DepartmentDTO> oDepartmentDTOList)
        {
            try
            {
                if (oDepartmentDTOList == null || oDepartmentDTOList.Count <= 0) BadRequest("No DTO passed");
                List<Department> oDepartmentList = Mapper.Map<List<DepartmentDTO>, List<Department>>(oDepartmentDTOList); //Mapper code            
                oDepartmentList = new Department().SaveList(oDepartmentList);
                oDepartmentDTOList = Mapper.Map<List<Department>, List<DepartmentDTO>>(oDepartmentList);
                return Ok(oDepartmentDTOList);
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
        public IHttpActionResult Validate([FromBody] DepartmentDTO oDepartmentDTO)
        {
            try
            {
                ValidationObj oValidationObj = new ValidationObj() { IsValid = "Y", ErrorMessage = "" };
                if (oDepartmentDTO == null) BadRequest("No DTO passed");
                Department DepartmentDTO = new Department().Load(where: "Name='" + oDepartmentDTO.Name + "'" + (oDepartmentDTO.DepartmentID.HasValue ? " And DepartmentID <> " + oDepartmentDTO.DepartmentID : ""));
                if (DepartmentDTO != null) { oValidationObj.IsValid = "N"; oValidationObj.ErrorMessage = "Department name already exists"; }
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
