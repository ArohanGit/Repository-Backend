

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Cors;
using System.Web.Http.Cors;
using AutoMapper;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Web.Http.Description;
using FileRepository.BusinessObjects;

namespace FileRepositoryAPI.WebAPI
{

    /// <summary>
    /// User
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/User")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public partial class UserController : ApiController
    {
        static bool initialized = false;

        static UserController()
        {
            if (!initialized)
            {
                Mapper.CreateMap<User, UserDTO>();
                Mapper.CreateMap<UserDTO, User>();
                // Child Object Mapping

                initialized = true;
            }
        }


        #region "Generated Services"

        // GET api/User/DTO
        /// <summary>
        /// Get User Model DTO
        /// </summary>
        /// <remarks>User Model</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("DTO")]
        public IHttpActionResult GetDTO()
        {
            try
            {
                UserDTO entityDTO = new UserDTO();

                return Ok(entityDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/User
        /// <summary>
        /// Get all Users
        /// </summary>
        /// <remarks>List of all Users</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route()]
        public IHttpActionResult Get()
        {
            try
            {
                var queryString = HttpContext.Current.Request.QueryString;
                int skip = Convert.ToInt32(queryString["$skip"]);
                int take = Convert.ToInt32(queryString["$top"]);
                List<User> oUserList = new User().LoadList().ToList();
                List<UserDTO> oUserDTOList = Mapper.Map<List<User>, List<UserDTO>>(oUserList);

                // Set RoleID & Name
                foreach (UserDTO oUserDTO in oUserDTOList) { SetRoleName(oUserDTO); }

                return Ok(new { Items = oUserDTOList, Count = oUserDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/User/guid
        /// <summary>
        /// Get User
        /// </summary>
        /// <remarks>Get User by specified unique identifier</remarks>
        /// <param name="guid">The unique identifier.</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("{guid}")]
        public IHttpActionResult Get(string guid)
        {
            try
            {
                if (string.IsNullOrEmpty(guid)) return Ok(new UserDTO());
                User oUser = new User().Load(guid, true);
                if (oUser == null) return NotFound();
                UserDTO oUserDTO = Mapper.Map<User, UserDTO>(oUser);

                // Set RoleID & Name
                SetRoleName(oUserDTO);

                return Ok(oUserDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // POST api/User
        /// <summary>
        /// Save User 
        /// </summary>
        /// <remarks>Add new User</remarks>
        /// <param name="oDTO">User object to be saved</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost] //Save
        [Route()]
        public IHttpActionResult Post([FromBody] UserDTO oUserDTO)
        {
            try
            {
                if (oUserDTO == null) BadRequest("No DTO passed");
                User oUser = Mapper.Map<UserDTO, User>(oUserDTO); //Mapper code

                if (!oUser.IsValid) return BadRequest(oUser.Errors.ToModelState());
                oUser.Save();

                // Update Role
                UserRole oUserRole = new UserRole().Load(where: "UserID= " + oUser.UserID);
                if (oUserRole == null) oUserRole = new UserRole();
                oUserRole.UserID = oUser.UserID;
                oUserRole.RoleID = oUserDTO.RoleID;
                oUserRole.Save();

                if (!oUser.IsValid) return BadRequest(oUser.Errors.ToModelState());
                oUser = new User().Load(oUser.UserID, true);
                oUserDTO = Mapper.Map<User, UserDTO>(oUser);

                // Set RoleID & Name
                SetRoleName(oUserDTO); 

                return Ok(oUserDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // PUT api/User/guid
        /// <summary>
        /// Update User
        /// </summary>
        /// <remarks>Update existing User details</remarks>
        /// <param name="oUserDTO">User object to be modified</param>        
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut] //Update
        [Route()]
        public IHttpActionResult Put([FromBody] UserDTO oUserDTO)
        {
            try
            {
                if (oUserDTO == null) BadRequest("No DTO passed");
                User oUser = new User().Load(oUserDTO.UserID);
                if (oUser == null) return NotFound();
                oUser = Mapper.Map<UserDTO, User>(oUserDTO); //Mapper code

                if (!oUser.IsValid) return BadRequest(oUser.Errors.ToModelState());
                oUser.Update();
                oUserDTO = Mapper.Map<User, UserDTO>(oUser);

                // Set RoleID & Name
                SetRoleName(oUserDTO);

                return Ok(oUserDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // DELETE api/User/guid
        /// <summary>
        /// Deletes User Record.
        /// </summary>
        /// <remarks>
        /// Deletes User by the specified unique identifier.
        /// </remarks>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpDelete] //Delete
        [Route("{guid}")]
        public IHttpActionResult Delete(string guid)
        {
            try
            {
                User oUser = new User().Load(guid);
                if (oUser != null) oUser.Delete();
                UserDTO oUserDTO = Mapper.Map<User, UserDTO>(oUser);

                // Set RoleID & Name
                SetRoleName(oUserDTO);

                return Ok(oUserDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/User/where
        /// <summary>
        /// Get Users
        /// </summary>
        /// <remarks>Get list of Users matching criteria</remarks>
        /// <param name="where">Where clause to get list of User</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("GetByCriteria/{where}")]
        public IHttpActionResult GetByCriteria(string where)
        {
            try
            {
                var queryString = HttpContext.Current.Request.QueryString;
                int skip = Convert.ToInt32(queryString["$skip"]);
                int take = Convert.ToInt32(queryString["$top"]);
                List<User> oUserList = new User().LoadList(where: where).ToList();
                List<UserDTO> oUserDTOList = Mapper.Map<List<User>, List<UserDTO>>(oUserList);

                // Set RoleID & Name
                foreach (UserDTO oUserDTO in oUserDTOList) { SetRoleName(oUserDTO); }

                return Ok(new { Items = oUserDTOList, Count = oUserDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/User/LoadList
        /// <summary>
        /// Get Users
        /// </summary>
        /// <remarks>Get list of Users by User criteria object. Ref Model/DTO structure.</remarks>
        /// <param name="oWhere">User DTO object with properties to included in where criteria</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("LoadList")]
        public IHttpActionResult LoadList([FromBody] UserDTO oWhere)
        {
            try
            {
                var queryString = HttpContext.Current.Request.QueryString;
                int skip = Convert.ToInt32(queryString["$skip"]);
                int take = Convert.ToInt32(queryString["$top"]);
                User oUserCriteria = Mapper.Map<UserDTO, User>(oWhere);
                List<User> oUserList = new User().LoadList(oUserCriteria).ToList();
                List<UserDTO> oUserDTOList = Mapper.Map<List<User>, List<UserDTO>>(oUserList);

                // Set RoleID & Name
                foreach (UserDTO oUserDTO in oUserDTOList) { SetRoleName(oUserDTO); }

                return Ok(new { Items = oUserDTOList, Count = oUserDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/User/Sum/column/where
        [HttpPost]
        [Route("Sum/{column}/{where}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum(string column, string where)
        {
            try
            {
                var result = new User().Sum(column: column, where: where);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/User/Sum
        [HttpPost]
        [Route("Sum")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum([FromBody] UserDTO oWhere)
        {
            try
            {
                User oUserCriteria = Mapper.Map<UserDTO, User>(oWhere);
                var result = new User().Sum(oWhere.ScalarColumn, oUserCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/User/Min
        [HttpPost]
        [Route("Min")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Min([FromBody] UserDTO oWhere)
        {
            try
            {
                User oUserCriteria = Mapper.Map<UserDTO, User>(oWhere);
                var result = new User().Min(oWhere.ScalarColumn, oUserCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/User/Max
        [HttpPost]
        [Route("Max")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Max([FromBody] UserDTO oWhere)
        {
            try
            {
                User oUserCriteria = Mapper.Map<UserDTO, User>(oWhere);
                var result = new User().Max(oWhere.ScalarColumn, oUserCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/User/Count
        [HttpPost]
        [Route("Count")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Count([FromBody] UserDTO oWhere)
        {
            try
            {
                User oUserCriteria = Mapper.Map<UserDTO, User>(oWhere);
                var result = new User().Count(oUserCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        #endregion

        #region "Helper Functions"

        // POST api/User/Insert
        /// <summary>
        /// Add User
        /// </summary>
        /// <remarks>Add User using JSON object in the format of User Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody] JObject jUserDTO)
        {
            UserDTO oUserDTO = ((JObject)jUserDTO.ToObject<JsonObject>().value).ToObject<UserDTO>();
            return this.Post(oUserDTO);
        }

        // POST api/User/Update
        /// <summary>
        /// Uodate User
        /// </summary>
        /// <remarks>Uodate User using JSON object in the format of User Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] JObject jUserDTO)
        {
            UserDTO oUserDTO = ((JObject)jUserDTO.ToObject<JsonObject>().value).ToObject<UserDTO>();
            return this.Put(oUserDTO);
        }

        // POST api/User/Remove
        /// <summary>
        /// Delete User
        /// </summary>
        /// <remarks>Delete User using JSON object in the format of User Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Remove")]
        public IHttpActionResult Remove([FromBody] JObject jUserDTO)
        {
            string id = jUserDTO.ToObject<JsonObject>().key.ToString();
            return this.Delete(id);
        }

        #endregion

        #region "Additional Functions"

        #endregion

    }
}
