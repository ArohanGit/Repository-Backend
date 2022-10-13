

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
    public partial class UserController : ApiController
    {

        #region "Additional Services"

        [HttpGet]
        [Route("loadlist")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult LoadList()
        {
            try
            {
                //List<User> oUserList = new User().LoadList().ToList();
                //List<UserDTO> oUserDTOList = Mapper.Map<List<User>, List<UserDTO>>(oUserList);
                //return Ok(oUserDTOList);
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
        public IHttpActionResult SaveList([FromBody] List<UserDTO> oUserDTOList)
        {
            try
            {
                if (oUserDTOList == null || oUserDTOList.Count <= 0) BadRequest("No DTO passed");
                List<User> oUserList = Mapper.Map<List<UserDTO>, List<User>>(oUserDTOList); //Mapper code            
                oUserList = new User().SaveList(oUserList);
                oUserDTOList = Mapper.Map<List<User>, List<UserDTO>>(oUserList);
                return Ok(oUserDTOList);
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
                if (string.IsNullOrEmpty(loginInfoDTO.userName)) return Ok(new UserDTO());

                User oUser = new User().Load(where: "WebUserID = '" + loginInfoDTO.userName + "'", withChildren: true);
                if (oUser == null) return NotFound();

                UserRole oUserRole = new UserRole().Load(where: "UserID=" + oUser.UserID);
                Role oRole = new Role().Load(oUserRole.RoleID);
                UserDTO oUserDTO = Mapper.Map<User, UserDTO>(oUser);
                oUserDTO.RoleID = oRole.RoleID;
                oUserDTO.RoleName = oRole.Name;
                oUserDTO.password = null;
                return Ok(oUserDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody] LogInfoDTO loginInfoDTO)
        {
            try
            {
                if (loginInfoDTO == null) return BadRequest("Login credentials note received.");
                string ticket = Authenticate.Login(loginInfoDTO.userName, loginInfoDTO.Password);
                if (ticket == null) return BadRequest("UserName and Password does not match. Please re-enter your credentials");

                try
                {
                }
                catch (Exception ex)
                {
                    string sErrorLog = System.Web.HttpContext.Current.Server.MapPath("~/Docs/ErrorLog.txt");
                    if (!File.Exists(sErrorLog)) { File.Create(sErrorLog); }
                    StreamWriter sw = new StreamWriter(sErrorLog);
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.InnerException.Message);
                    sw.Close();
                }

                return Ok(ticket);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/User/GetPassword/id
        [HttpGet]
        [Route("GetPassword/{id:int}")]
        public IHttpActionResult GetPassword(int id)
        {
            try
            {
                if (id == -1) return Ok(new UserDTO());
                User oUser = new User().Load(id, true);
                if (oUser == null) return NotFound();
                UserDTO oUserDTO = Mapper.Map<User, UserDTO>(oUser);
                SetRoleName(oUserDTO);
                return Ok(oUserDTO.password);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        private void SetRoleName(UserDTO oUserDTO)
        {
            UserRole oUserRole = new UserRole().Load(where: "UserID=" + oUserDTO.UserID);
            Role oRole = new Role().Load(oUserRole.RoleID);
            oUserDTO.RoleID = oRole.RoleID;
            oUserDTO.RoleName = oRole.Name;
        }

        [HttpGet]
        [Route("GetUserInfo")]
        public IHttpActionResult GetUserInfo()
        {
            try
            {
                DataTable userInfo = new AdHocQueries().GetUserInfo();
                return (IHttpActionResult)this.Ok(new
                {
                    Items = userInfo
                });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpPost] //Validate
        [Route("Validate")]
        public IHttpActionResult Validate([FromBody] UserDTO oUserDTO)
        {
            try
            {
                ValidationObj oValidationObj = new ValidationObj() { IsValid = "Y", ErrorMessage = "" };
                if (oUserDTO == null) BadRequest("No DTO passed");
                User oUser = new User().Load(where: "WebUserID='" + oUserDTO.WebUserID + "'" + (oUserDTO.UserID.HasValue ? " And UserID <> " + oUserDTO.UserID : ""));
                if (oUser != null) { oValidationObj.IsValid = "N"; oValidationObj.ErrorMessage = "AD ID already exists"; }
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

public class ValidationObj
{
    public string IsValid { get; set; }
    public string ErrorMessage { get; set; }
}