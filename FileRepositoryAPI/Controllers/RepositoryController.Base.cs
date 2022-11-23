

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
    /// Repository
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/Repository")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AppAuthenticationFilter]
    public partial class RepositoryController : ApiController
    {
        static bool initialized = false;

        static RepositoryController()
        {
            if (!initialized)
            {
                Mapper.CreateMap<Repository, RepositoryDTO>();
                Mapper.CreateMap<RepositoryDTO, Repository>();
                // Child Object Mapping

                initialized = true;
            }
        }


        #region "Generated Services"

        // GET api/Repository/DTO
        /// <summary>
        /// Get Repository Model DTO
        /// </summary>
        /// <remarks>Repository Model</remarks>
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
                RepositoryDTO entityDTO = new RepositoryDTO();

                return Ok(entityDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Repository
        /// <summary>
        /// Get all Repositorys
        /// </summary>
        /// <remarks>List of all Repositorys</remarks>
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
                List<Repository> oRepositoryList = new Repository().LoadList().ToList();
                List<RepositoryDTO> oRepositoryDTOList = Mapper.Map<List<Repository>, List<RepositoryDTO>>(oRepositoryList);

                SetCalculatedProperties(oRepositoryDTOList);

                return Ok(new { Items = oRepositoryDTOList, Count = oRepositoryDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Repository/guid
        /// <summary>
        /// Get Repository
        /// </summary>
        /// <remarks>Get Repository by specified unique identifier</remarks>
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
                if (string.IsNullOrEmpty(guid)) return Ok(new RepositoryDTO());
                Repository oRepository = new Repository().Load(guid, true);
                if (oRepository == null) return NotFound();
                RepositoryDTO oRepositoryDTO = Mapper.Map<Repository, RepositoryDTO>(oRepository);

                List<RepositoryDTO> oRepositoryDTOList = new List<RepositoryDTO>();
                oRepositoryDTOList.Add(oRepositoryDTO);
                SetCalculatedProperties(oRepositoryDTOList);

                return Ok(oRepositoryDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // POST api/Repository
        /// <summary>
        /// Save Repository 
        /// </summary>
        /// <remarks>Add new Repository</remarks>
        /// <param name="oDTO">Repository object to be saved</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost] //Save
        [Route()]
        public IHttpActionResult Post([FromBody] RepositoryDTO oRepositoryDTO)
        {
            try
            {
                if (oRepositoryDTO == null) BadRequest("No DTO passed");
                Repository oRepository = Mapper.Map<RepositoryDTO, Repository>(oRepositoryDTO); //Mapper code

                if (!oRepository.IsValid) return BadRequest(oRepository.Errors.ToModelState());
                oRepository.Save();

                if (!oRepository.IsValid) return BadRequest(oRepository.Errors.ToModelState());
                oRepository = new Repository().Load(oRepository.RepositoryID, true);
                oRepositoryDTO = Mapper.Map<Repository, RepositoryDTO>(oRepository);

                List<RepositoryDTO> oRepositoryDTOList = new List<RepositoryDTO>();
                oRepositoryDTOList.Add(oRepositoryDTO);
                SetCalculatedProperties(oRepositoryDTOList);

                return Ok(oRepositoryDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // PUT api/Repository/guid
        /// <summary>
        /// Update Repository
        /// </summary>
        /// <remarks>Update existing Repository details</remarks>
        /// <param name="oRepositoryDTO">Repository object to be modified</param>        
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut] //Update
        [Route()]
        public IHttpActionResult Put([FromBody] RepositoryDTO oRepositoryDTO)
        {
            try
            {
                if (oRepositoryDTO == null) BadRequest("No DTO passed");
                Repository oRepository = new Repository().Load(oRepositoryDTO.RepositoryID);
                if (oRepository == null) return NotFound();
                oRepository = Mapper.Map<RepositoryDTO, Repository>(oRepositoryDTO); //Mapper code

                if (!oRepository.IsValid) return BadRequest(oRepository.Errors.ToModelState());
                oRepository.Update();
                oRepositoryDTO = Mapper.Map<Repository, RepositoryDTO>(oRepository);

                List<RepositoryDTO> oRepositoryDTOList = new List<RepositoryDTO>();
                oRepositoryDTOList.Add(oRepositoryDTO);
                SetCalculatedProperties(oRepositoryDTOList);

                return Ok(oRepositoryDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // DELETE api/Repository/guid
        /// <summary>
        /// Deletes Repository Record.
        /// </summary>
        /// <remarks>
        /// Deletes Repository by the specified unique identifier.
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
                Repository oRepository = new Repository().Load(guid);
                if (oRepository != null) oRepository.Delete();
                RepositoryDTO oRepositoryDTO = Mapper.Map<Repository, RepositoryDTO>(oRepository);

                return Ok(oRepositoryDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Repository/where
        /// <summary>
        /// Get Repositorys
        /// </summary>
        /// <remarks>Get list of Repositorys matching criteria</remarks>
        /// <param name="where">Where clause to get list of Repository</param>
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
                List<Repository> oRepositoryList = new Repository().LoadList(where: where).ToList();
                List<RepositoryDTO> oRepositoryDTOList = Mapper.Map<List<Repository>, List<RepositoryDTO>>(oRepositoryList);

                return Ok(new { Items = oRepositoryDTOList, Count = oRepositoryDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Repository/LoadList
        /// <summary>
        /// Get Repositorys
        /// </summary>
        /// <remarks>Get list of Repositorys by Repository criteria object. Ref Model/DTO structure.</remarks>
        /// <param name="oWhere">Repository DTO object with properties to included in where criteria</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("LoadList")]
        public IHttpActionResult LoadList([FromBody] RepositoryDTO oWhere)
        {
            try
            {
                var queryString = HttpContext.Current.Request.QueryString;
                int skip = Convert.ToInt32(queryString["$skip"]);
                int take = Convert.ToInt32(queryString["$top"]);
                Repository oRepositoryCriteria = Mapper.Map<RepositoryDTO, Repository>(oWhere);
                List<Repository> oRepositoryList = new Repository().LoadList(oRepositoryCriteria).ToList();
                List<RepositoryDTO> oRepositoryDTOList = Mapper.Map<List<Repository>, List<RepositoryDTO>>(oRepositoryList);

                SetCalculatedProperties(oRepositoryDTOList);

                return Ok(new { Items = oRepositoryDTOList, Count = oRepositoryDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Repository/Sum/column/where
        [HttpPost]
        [Route("Sum/{column}/{where}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum(string column, string where)
        {
            try
            {
                var result = new Repository().Sum(column: column, where: where);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Repository/Sum
        [HttpPost]
        [Route("Sum")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum([FromBody] RepositoryDTO oWhere)
        {
            try
            {
                Repository oRepositoryCriteria = Mapper.Map<RepositoryDTO, Repository>(oWhere);
                var result = new Repository().Sum(oWhere.ScalarColumn, oRepositoryCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Repository/Min
        [HttpPost]
        [Route("Min")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Min([FromBody] RepositoryDTO oWhere)
        {
            try
            {
                Repository oRepositoryCriteria = Mapper.Map<RepositoryDTO, Repository>(oWhere);
                var result = new Repository().Min(oWhere.ScalarColumn, oRepositoryCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Repository/Max
        [HttpPost]
        [Route("Max")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Max([FromBody] RepositoryDTO oWhere)
        {
            try
            {
                Repository oRepositoryCriteria = Mapper.Map<RepositoryDTO, Repository>(oWhere);
                var result = new Repository().Max(oWhere.ScalarColumn, oRepositoryCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Repository/Count
        [HttpPost]
        [Route("Count")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Count([FromBody] RepositoryDTO oWhere)
        {
            try
            {
                Repository oRepositoryCriteria = Mapper.Map<RepositoryDTO, Repository>(oWhere);
                var result = new Repository().Count(oRepositoryCriteria);
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

        // POST api/Repository/Insert
        /// <summary>
        /// Add Repository
        /// </summary>
        /// <remarks>Add Repository using JSON object in the format of Repository Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody] JObject jRepositoryDTO)
        {
            RepositoryDTO oRepositoryDTO = ((JObject)jRepositoryDTO.ToObject<JsonObject>().value).ToObject<RepositoryDTO>();
            return this.Post(oRepositoryDTO);
        }

        // POST api/Repository/Update
        /// <summary>
        /// Uodate Repository
        /// </summary>
        /// <remarks>Uodate Repository using JSON object in the format of Repository Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] JObject jRepositoryDTO)
        {
            RepositoryDTO oRepositoryDTO = ((JObject)jRepositoryDTO.ToObject<JsonObject>().value).ToObject<RepositoryDTO>();
            return this.Put(oRepositoryDTO);
        }

        // POST api/Repository/Remove
        /// <summary>
        /// Delete Repository
        /// </summary>
        /// <remarks>Delete Repository using JSON object in the format of Repository Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Remove")]
        public IHttpActionResult Remove([FromBody] JObject jRepositoryDTO)
        {
            string id = jRepositoryDTO.ToObject<JsonObject>().key.ToString();
            return this.Delete(id);
        }

        #endregion

        #region "Additional Functions"

        public void SetCalculatedProperties(List<RepositoryDTO> oRepositoryDTOList)
        {
            //List<NotificationTo> oNotificationToList = new NotificationTo().LoadList().ToList();
            //List<User> oUserList = new User().LoadList().ToList();
            //foreach (RepositoryDTO oRepositoryDTO in oRepositoryDTOList)
            //{
            //    //List<NotificationTo> oNotificationTo = oNotificationToList.FindAll(u => u.RepositoryID == oRepositoryDTO.RepositoryID);
            //    //string oNotificationToEmails = string.Join("', '", oNotificationTo.Select(o => o.Email));
            //    //List<User> oUsers = oUserList.FindAll(u => u.EMailAddress.Contains(oNotificationToEmails));
            //    //string oNotificationToUserIDs = string.Join(", ", oUsers.Select(o => o.UserID));
            //    //oRepositoryDTO.NotificationToUserIDs = oNotificationToUserIDs;
            //}
        }

        #endregion
    }
}
