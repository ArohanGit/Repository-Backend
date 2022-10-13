
  
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
    /// Role
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/Role")]    
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AppAuthenticationFilter]
    public partial class RoleController : ApiController
    {
        static bool initialized = false;

        static RoleController()
        {
            if (!initialized)
            {
                Mapper.CreateMap<Role, RoleDTO>();
                Mapper.CreateMap<RoleDTO, Role>();
                // Child Object Mapping
                
                initialized = true;
            }
        }


        #region "Generated Services"

        // GET api/Role/DTO
        /// <summary>
        /// Get Role Model DTO
        /// </summary>
        /// <remarks>Role Model</remarks>
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
                RoleDTO entityDTO = new RoleDTO();
                
                return Ok(entityDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Role
        /// <summary>
        /// Get all Roles
        /// </summary>
        /// <remarks>List of all Roles</remarks>
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
                List<Role> oRoleList = new Role().LoadList().ToList();
                List<RoleDTO> oRoleDTOList = Mapper.Map<List<Role>, List<RoleDTO>>(oRoleList);
                
                return Ok(new { Items = oRoleDTOList, Count = oRoleDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Role/guid
        /// <summary>
        /// Get Role
        /// </summary>
        /// <remarks>Get Role by specified unique identifier</remarks>
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
                if (string.IsNullOrEmpty(guid)) return Ok(new RoleDTO());
                Role oRole = new Role().Load(guid, true);
                if (oRole == null) return NotFound();                
                RoleDTO oRoleDTO = Mapper.Map<Role, RoleDTO>(oRole);
                
                return Ok(oRoleDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // POST api/Role
        /// <summary>
        /// Save Role 
        /// </summary>
        /// <remarks>Add new Role</remarks>
        /// <param name="oDTO">Role object to be saved</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost] //Save
        [Route()]
        public IHttpActionResult Post([FromBody]RoleDTO oRoleDTO)
        {
            try
            {
                if (oRoleDTO == null) BadRequest("No DTO passed");
                Role oRole = Mapper.Map<RoleDTO, Role>(oRoleDTO); //Mapper code
                
                if (!oRole.IsValid) return BadRequest(oRole.Errors.ToModelState());
                oRole.Save();
                if (!oRole.IsValid) return BadRequest(oRole.Errors.ToModelState());
                oRole = new Role().Load(oRole.RoleID, true);
                oRoleDTO = Mapper.Map<Role, RoleDTO>(oRole);
                
                return Ok(oRoleDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // PUT api/Role/guid
        /// <summary>
        /// Update Role
        /// </summary>
        /// <remarks>Update existing Role details</remarks>
        /// <param name="oRoleDTO">Role object to be modified</param>        
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut] //Update
        [Route()]
        public IHttpActionResult Put([FromBody]RoleDTO oRoleDTO)
        {
            try
            {
                if (oRoleDTO == null) BadRequest("No DTO passed");
                Role oRole = new Role().Load(oRoleDTO.RoleID);
                if (oRole == null) return NotFound();
                oRole = Mapper.Map<RoleDTO, Role>(oRoleDTO); //Mapper code
                
                if (!oRole.IsValid) return BadRequest(oRole.Errors.ToModelState());
                oRole.Update();
                oRoleDTO = Mapper.Map<Role, RoleDTO>(oRole);
                
                return Ok(oRoleDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // DELETE api/Role/guid
        /// <summary>
        /// Deletes Role Record.
        /// </summary>
        /// <remarks>
        /// Deletes Role by the specified unique identifier.
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
                Role oRole = new Role().Load(guid);
                if (oRole != null) oRole.Delete();
                RoleDTO oRoleDTO = Mapper.Map<Role, RoleDTO>(oRole);
                return Ok(oRoleDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Role/where
        /// <summary>
        /// Get Roles
        /// </summary>
        /// <remarks>Get list of Roles matching criteria</remarks>
        /// <param name="where">Where clause to get list of Role</param>
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
                List<Role> oRoleList = new Role().LoadList(where: where).ToList();
                List<RoleDTO> oRoleDTOList = Mapper.Map<List<Role>, List<RoleDTO>>(oRoleList);
                
                return Ok(new { Items = oRoleDTOList, Count = oRoleDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Role/LoadList
        /// <summary>
        /// Get Roles
        /// </summary>
        /// <remarks>Get list of Roles by Role criteria object. Ref Model/DTO structure.</remarks>
        /// <param name="oWhere">Role DTO object with properties to included in where criteria</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("LoadList")]
        public IHttpActionResult LoadList([FromBody]RoleDTO oWhere)
        {
            try
            {
                var queryString = HttpContext.Current.Request.QueryString;
                int skip = Convert.ToInt32(queryString["$skip"]);
                int take = Convert.ToInt32(queryString["$top"]);
                Role oRoleCriteria = Mapper.Map<RoleDTO, Role>(oWhere);
                List<Role> oRoleList = new Role().LoadList(oRoleCriteria).ToList();
                List<RoleDTO> oRoleDTOList = Mapper.Map<List<Role>, List<RoleDTO>>(oRoleList);
                
                return Ok(new { Items = oRoleDTOList, Count = oRoleDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Role/Sum/column/where
        [HttpPost]
        [Route("Sum/{column}/{where}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum(string column, string where)
        {
            try
            {
                var result = new Role().Sum(column: column, where: where);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Role/Sum
        [HttpPost]
        [Route("Sum")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum([FromBody]RoleDTO oWhere)
        {
            try
            {
                Role oRoleCriteria = Mapper.Map<RoleDTO, Role>(oWhere);
                var result = new Role().Sum(oWhere.ScalarColumn, oRoleCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Role/Min
        [HttpPost]
        [Route("Min")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Min([FromBody]RoleDTO oWhere)
        {
            try
            {
                Role oRoleCriteria = Mapper.Map<RoleDTO, Role>(oWhere);
                var result = new Role().Min(oWhere.ScalarColumn, oRoleCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Role/Max
        [HttpPost]
        [Route("Max")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Max([FromBody]RoleDTO oWhere)
        {
            try
            {
                Role oRoleCriteria = Mapper.Map<RoleDTO, Role>(oWhere);
                var result = new Role().Max(oWhere.ScalarColumn, oRoleCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Role/Count
        [HttpPost]
        [Route("Count")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Count([FromBody]RoleDTO oWhere)
        {
            try
            {
                Role oRoleCriteria = Mapper.Map<RoleDTO, Role>(oWhere);
                var result = new Role().Count(oRoleCriteria);
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

        // POST api/Role/Insert
        /// <summary>
        /// Add Role
        /// </summary>
        /// <remarks>Add Role using JSON object in the format of Role Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody] JObject jRoleDTO)
        {
            RoleDTO oRoleDTO = ((JObject)jRoleDTO.ToObject<JsonObject>().value).ToObject<RoleDTO>();
            return this.Post(oRoleDTO);
        }

        // POST api/Role/Update
        /// <summary>
        /// Uodate Role
        /// </summary>
        /// <remarks>Uodate Role using JSON object in the format of Role Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] JObject jRoleDTO)
        {
            RoleDTO oRoleDTO = ((JObject)jRoleDTO.ToObject<JsonObject>().value).ToObject<RoleDTO>();
            return this.Put(oRoleDTO);
        }

        // POST api/Role/Remove
        /// <summary>
        /// Delete Role
        /// </summary>
        /// <remarks>Delete Role using JSON object in the format of Role Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Remove")]
        public IHttpActionResult Remove([FromBody] JObject jRoleDTO)
        {
            string id = jRoleDTO.ToObject<JsonObject>().key.ToString();
            return this.Delete(id);
        }

        #endregion

        #region "Additional Functions"

        #endregion

    }
}
