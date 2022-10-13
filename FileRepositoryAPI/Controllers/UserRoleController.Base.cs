
  
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
    /// UserRole
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/UserRole")]    
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AppAuthenticationFilter]
    public partial class UserRoleController : ApiController
    {
        static bool initialized = false;

        static UserRoleController()
        {
            if (!initialized)
            {
                Mapper.CreateMap<UserRole, UserRoleDTO>();
                Mapper.CreateMap<UserRoleDTO, UserRole>();
                // Child Object Mapping
                
                initialized = true;
            }
        }


        #region "Generated Services"

        // GET api/UserRole/DTO
        /// <summary>
        /// Get UserRole Model DTO
        /// </summary>
        /// <remarks>UserRole Model</remarks>
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
                UserRoleDTO entityDTO = new UserRoleDTO();
                
                return Ok(entityDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/UserRole
        /// <summary>
        /// Get all UserRoles
        /// </summary>
        /// <remarks>List of all UserRoles</remarks>
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
                List<UserRole> oUserRoleList = new UserRole().LoadList().ToList();
                List<UserRoleDTO> oUserRoleDTOList = Mapper.Map<List<UserRole>, List<UserRoleDTO>>(oUserRoleList);
                
                return Ok(new { Items = oUserRoleDTOList, Count = oUserRoleDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/UserRole/guid
        /// <summary>
        /// Get UserRole
        /// </summary>
        /// <remarks>Get UserRole by specified unique identifier</remarks>
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
                if (string.IsNullOrEmpty(guid)) return Ok(new UserRoleDTO());
                UserRole oUserRole = new UserRole().Load(guid, true);
                if (oUserRole == null) return NotFound();                
                UserRoleDTO oUserRoleDTO = Mapper.Map<UserRole, UserRoleDTO>(oUserRole);
                
                return Ok(oUserRoleDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // POST api/UserRole
        /// <summary>
        /// Save UserRole 
        /// </summary>
        /// <remarks>Add new UserRole</remarks>
        /// <param name="oDTO">UserRole object to be saved</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost] //Save
        [Route()]
        public IHttpActionResult Post([FromBody]UserRoleDTO oUserRoleDTO)
        {
            try
            {
                if (oUserRoleDTO == null) BadRequest("No DTO passed");
                UserRole oUserRole = Mapper.Map<UserRoleDTO, UserRole>(oUserRoleDTO); //Mapper code
                
                if (!oUserRole.IsValid) return BadRequest(oUserRole.Errors.ToModelState());
                oUserRole.Save();
                if (!oUserRole.IsValid) return BadRequest(oUserRole.Errors.ToModelState());
                oUserRole = new UserRole().Load(oUserRole.UserRoleID, true);
                oUserRoleDTO = Mapper.Map<UserRole, UserRoleDTO>(oUserRole);
                
                return Ok(oUserRoleDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // PUT api/UserRole/guid
        /// <summary>
        /// Update UserRole
        /// </summary>
        /// <remarks>Update existing UserRole details</remarks>
        /// <param name="oUserRoleDTO">UserRole object to be modified</param>        
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut] //Update
        [Route()]
        public IHttpActionResult Put([FromBody]UserRoleDTO oUserRoleDTO)
        {
            try
            {
                if (oUserRoleDTO == null) BadRequest("No DTO passed");
                UserRole oUserRole = new UserRole().Load(oUserRoleDTO.UserRoleID);
                if (oUserRole == null) return NotFound();
                oUserRole = Mapper.Map<UserRoleDTO, UserRole>(oUserRoleDTO); //Mapper code
                
                if (!oUserRole.IsValid) return BadRequest(oUserRole.Errors.ToModelState());
                oUserRole.Update();
                oUserRoleDTO = Mapper.Map<UserRole, UserRoleDTO>(oUserRole);
                
                return Ok(oUserRoleDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // DELETE api/UserRole/guid
        /// <summary>
        /// Deletes UserRole Record.
        /// </summary>
        /// <remarks>
        /// Deletes UserRole by the specified unique identifier.
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
                UserRole oUserRole = new UserRole().Load(guid);
                if (oUserRole != null) oUserRole.Delete();
                UserRoleDTO oUserRoleDTO = Mapper.Map<UserRole, UserRoleDTO>(oUserRole);
                return Ok(oUserRoleDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/UserRole/where
        /// <summary>
        /// Get UserRoles
        /// </summary>
        /// <remarks>Get list of UserRoles matching criteria</remarks>
        /// <param name="where">Where clause to get list of UserRole</param>
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
                List<UserRole> oUserRoleList = new UserRole().LoadList(where: where).ToList();
                List<UserRoleDTO> oUserRoleDTOList = Mapper.Map<List<UserRole>, List<UserRoleDTO>>(oUserRoleList);
                
                return Ok(new { Items = oUserRoleDTOList, Count = oUserRoleDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/UserRole/LoadList
        /// <summary>
        /// Get UserRoles
        /// </summary>
        /// <remarks>Get list of UserRoles by UserRole criteria object. Ref Model/DTO structure.</remarks>
        /// <param name="oWhere">UserRole DTO object with properties to included in where criteria</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("LoadList")]
        public IHttpActionResult LoadList([FromBody]UserRoleDTO oWhere)
        {
            try
            {
                var queryString = HttpContext.Current.Request.QueryString;
                int skip = Convert.ToInt32(queryString["$skip"]);
                int take = Convert.ToInt32(queryString["$top"]);
                UserRole oUserRoleCriteria = Mapper.Map<UserRoleDTO, UserRole>(oWhere);
                List<UserRole> oUserRoleList = new UserRole().LoadList(oUserRoleCriteria).ToList();
                List<UserRoleDTO> oUserRoleDTOList = Mapper.Map<List<UserRole>, List<UserRoleDTO>>(oUserRoleList);
                
                return Ok(new { Items = oUserRoleDTOList, Count = oUserRoleDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/UserRole/Sum/column/where
        [HttpPost]
        [Route("Sum/{column}/{where}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum(string column, string where)
        {
            try
            {
                var result = new UserRole().Sum(column: column, where: where);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/UserRole/Sum
        [HttpPost]
        [Route("Sum")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum([FromBody]UserRoleDTO oWhere)
        {
            try
            {
                UserRole oUserRoleCriteria = Mapper.Map<UserRoleDTO, UserRole>(oWhere);
                var result = new UserRole().Sum(oWhere.ScalarColumn, oUserRoleCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/UserRole/Min
        [HttpPost]
        [Route("Min")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Min([FromBody]UserRoleDTO oWhere)
        {
            try
            {
                UserRole oUserRoleCriteria = Mapper.Map<UserRoleDTO, UserRole>(oWhere);
                var result = new UserRole().Min(oWhere.ScalarColumn, oUserRoleCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/UserRole/Max
        [HttpPost]
        [Route("Max")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Max([FromBody]UserRoleDTO oWhere)
        {
            try
            {
                UserRole oUserRoleCriteria = Mapper.Map<UserRoleDTO, UserRole>(oWhere);
                var result = new UserRole().Max(oWhere.ScalarColumn, oUserRoleCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/UserRole/Count
        [HttpPost]
        [Route("Count")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Count([FromBody]UserRoleDTO oWhere)
        {
            try
            {
                UserRole oUserRoleCriteria = Mapper.Map<UserRoleDTO, UserRole>(oWhere);
                var result = new UserRole().Count(oUserRoleCriteria);
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

        // POST api/UserRole/Insert
        /// <summary>
        /// Add UserRole
        /// </summary>
        /// <remarks>Add UserRole using JSON object in the format of UserRole Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody] JObject jUserRoleDTO)
        {
            UserRoleDTO oUserRoleDTO = ((JObject)jUserRoleDTO.ToObject<JsonObject>().value).ToObject<UserRoleDTO>();
            return this.Post(oUserRoleDTO);
        }

        // POST api/UserRole/Update
        /// <summary>
        /// Uodate UserRole
        /// </summary>
        /// <remarks>Uodate UserRole using JSON object in the format of UserRole Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] JObject jUserRoleDTO)
        {
            UserRoleDTO oUserRoleDTO = ((JObject)jUserRoleDTO.ToObject<JsonObject>().value).ToObject<UserRoleDTO>();
            return this.Put(oUserRoleDTO);
        }

        // POST api/UserRole/Remove
        /// <summary>
        /// Delete UserRole
        /// </summary>
        /// <remarks>Delete UserRole using JSON object in the format of UserRole Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Remove")]
        public IHttpActionResult Remove([FromBody] JObject jUserRoleDTO)
        {
            string id = jUserRoleDTO.ToObject<JsonObject>().key.ToString();
            return this.Delete(id);
        }

        #endregion

        #region "Additional Functions"

        #endregion

    }
}
