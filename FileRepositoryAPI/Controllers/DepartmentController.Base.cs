

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
    /// Department
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/Department")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public partial class DepartmentController : ApiController
    {
        static bool initialized = false;

        static DepartmentController()
        {
            if (!initialized)
            {
                Mapper.CreateMap<Department, DepartmentDTO>();
                Mapper.CreateMap<DepartmentDTO, Department>();
                // Child Object Mapping

                initialized = true;
            }
        }


        #region "Generated Services"

        // GET api/Department/DTO
        /// <summary>
        /// Get Department Model DTO
        /// </summary>
        /// <remarks>Department Model</remarks>
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
                DepartmentDTO entityDTO = new DepartmentDTO();

                return Ok(entityDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Department
        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <remarks>List of all Departments</remarks>
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
                List<Department> oDepartmentList = new Department().LoadList().ToList();
                List<DepartmentDTO> oDepartmentDTOList = Mapper.Map<List<Department>, List<DepartmentDTO>>(oDepartmentList);
                            
                return Ok(new { Items = oDepartmentDTOList, Count = oDepartmentDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Department/guid
        /// <summary>
        /// Get Department
        /// </summary>
        /// <remarks>Get Department by specified unique identifier</remarks>
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
                if (string.IsNullOrEmpty(guid)) return Ok(new DepartmentDTO());
                Department oDepartment = new Department().Load(guid, true);
                if (oDepartment == null) return NotFound();
                DepartmentDTO oDepartmentDTO = Mapper.Map<Department, DepartmentDTO>(oDepartment);
                             
                return Ok(oDepartmentDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // POST api/Department
        /// <summary>
        /// Save Department 
        /// </summary>
        /// <remarks>Add new Department</remarks>
        /// <param name="oDTO">Department object to be saved</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost] //Save
        [Route()]
        public IHttpActionResult Post([FromBody] DepartmentDTO oDepartmentDTO)
        {
            try
            {
                if (oDepartmentDTO == null) BadRequest("No DTO passed");
                Department oDepartment = Mapper.Map<DepartmentDTO, Department>(oDepartmentDTO); //Mapper code

                if (!oDepartment.IsValid) return BadRequest(oDepartment.Errors.ToModelState());
                oDepartment.Save();

               
                if (!oDepartment.IsValid) return BadRequest(oDepartment.Errors.ToModelState());
                oDepartment = new Department().Load(oDepartment.DepartmentID, true);
                oDepartmentDTO = Mapper.Map<Department, DepartmentDTO>(oDepartment);

                return Ok(oDepartmentDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // PUT api/Department/guid
        /// <summary>
        /// Update Department
        /// </summary>
        /// <remarks>Update existing Department details</remarks>
        /// <param name="oDepartmentDTO">Department object to be modified</param>        
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut] //Update
        [Route()]
        public IHttpActionResult Put([FromBody] DepartmentDTO oDepartmentDTO)
        {
            try
            {
                if (oDepartmentDTO == null) BadRequest("No DTO passed");
                Department oDepartment = new Department().Load(oDepartmentDTO.DepartmentID);
                if (oDepartment == null) return NotFound();
                oDepartment = Mapper.Map<DepartmentDTO, Department>(oDepartmentDTO); //Mapper code

                if (!oDepartment.IsValid) return BadRequest(oDepartment.Errors.ToModelState());
                oDepartment.Update();
                oDepartmentDTO = Mapper.Map<Department, DepartmentDTO>(oDepartment);

                return Ok(oDepartmentDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // DELETE api/Department/guid
        /// <summary>
        /// Deletes Department Record.
        /// </summary>
        /// <remarks>
        /// Deletes Department by the specified unique identifier.
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
                Department oDepartment = new Department().Load(guid);
                if (oDepartment != null) oDepartment.Delete();
                DepartmentDTO oDepartmentDTO = Mapper.Map<Department, DepartmentDTO>(oDepartment);

                return Ok(oDepartmentDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Department/where
        /// <summary>
        /// Get Departments
        /// </summary>
        /// <remarks>Get list of Departments matching criteria</remarks>
        /// <param name="where">Where clause to get list of Department</param>
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
                List<Department> oDepartmentList = new Department().LoadList(where: where).ToList();
                List<DepartmentDTO> oDepartmentDTOList = Mapper.Map<List<Department>, List<DepartmentDTO>>(oDepartmentList);

                return Ok(new { Items = oDepartmentDTOList, Count = oDepartmentDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Department/LoadList
        /// <summary>
        /// Get Departments
        /// </summary>
        /// <remarks>Get list of Departments by Department criteria object. Ref Model/DTO structure.</remarks>
        /// <param name="oWhere">Department DTO object with properties to included in where criteria</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("LoadList")]
        public IHttpActionResult LoadList([FromBody] DepartmentDTO oWhere)
        {
            try
            {
                var queryString = HttpContext.Current.Request.QueryString;
                int skip = Convert.ToInt32(queryString["$skip"]);
                int take = Convert.ToInt32(queryString["$top"]);
                Department oDepartmentCriteria = Mapper.Map<DepartmentDTO, Department>(oWhere);
                List<Department> oDepartmentList = new Department().LoadList(oDepartmentCriteria).ToList();
                List<DepartmentDTO> oDepartmentDTOList = Mapper.Map<List<Department>, List<DepartmentDTO>>(oDepartmentList);

                return Ok(new { Items = oDepartmentDTOList, Count = oDepartmentDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Department/Sum/column/where
        [HttpPost]
        [Route("Sum/{column}/{where}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum(string column, string where)
        {
            try
            {
                var result = new Department().Sum(column: column, where: where);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Department/Sum
        [HttpPost]
        [Route("Sum")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum([FromBody] DepartmentDTO oWhere)
        {
            try
            {
                Department oDepartmentCriteria = Mapper.Map<DepartmentDTO, Department>(oWhere);
                var result = new Department().Sum(oWhere.ScalarColumn, oDepartmentCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Department/Min
        [HttpPost]
        [Route("Min")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Min([FromBody] DepartmentDTO oWhere)
        {
            try
            {
                Department oDepartmentCriteria = Mapper.Map<DepartmentDTO, Department>(oWhere);
                var result = new Department().Min(oWhere.ScalarColumn, oDepartmentCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Department/Max
        [HttpPost]
        [Route("Max")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Max([FromBody] DepartmentDTO oWhere)
        {
            try
            {
                Department oDepartmentCriteria = Mapper.Map<DepartmentDTO, Department>(oWhere);
                var result = new Department().Max(oWhere.ScalarColumn, oDepartmentCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Department/Count
        [HttpPost]
        [Route("Count")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Count([FromBody] DepartmentDTO oWhere)
        {
            try
            {
                Department oDepartmentCriteria = Mapper.Map<DepartmentDTO, Department>(oWhere);
                var result = new Department().Count(oDepartmentCriteria);
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

        // POST api/Department/Insert
        /// <summary>
        /// Add Department
        /// </summary>
        /// <remarks>Add Department using JSON object in the format of Department Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody] JObject jDepartmentDTO)
        {
            DepartmentDTO oDepartmentDTO = ((JObject)jDepartmentDTO.ToObject<JsonObject>().value).ToObject<DepartmentDTO>();
            return this.Post(oDepartmentDTO);
        }

        // POST api/Department/Update
        /// <summary>
        /// Uodate Department
        /// </summary>
        /// <remarks>Uodate Department using JSON object in the format of Department Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] JObject jDepartmentDTO)
        {
            DepartmentDTO oDepartmentDTO = ((JObject)jDepartmentDTO.ToObject<JsonObject>().value).ToObject<DepartmentDTO>();
            return this.Put(oDepartmentDTO);
        }

        // POST api/Department/Remove
        /// <summary>
        /// Delete Department
        /// </summary>
        /// <remarks>Delete Department using JSON object in the format of Department Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Remove")]
        public IHttpActionResult Remove([FromBody] JObject jDepartmentDTO)
        {
            string id = jDepartmentDTO.ToObject<JsonObject>().key.ToString();
            return this.Delete(id);
        }

        #endregion

        #region "Additional Functions"

        #endregion

    }
}
