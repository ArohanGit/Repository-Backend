

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
    /// NotificationTo
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/NotificationTo")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AppAuthenticationFilter]
    public partial class NotificationToController : ApiController
    {
        static bool initialized = false;

        static NotificationToController()
        {
            if (!initialized)
            {
                Mapper.CreateMap<NotificationTo, NotificationToDTO>();
                Mapper.CreateMap<NotificationToDTO, NotificationTo>();
                // Child Object Mapping

                initialized = true;
            }
        }


        #region "Generated Services"

        // GET api/NotificationTo/DTO
        /// <summary>
        /// Get NotificationTo Model DTO
        /// </summary>
        /// <remarks>NotificationTo Model</remarks>
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
                NotificationToDTO entityDTO = new NotificationToDTO();

                return Ok(entityDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/NotificationTo
        /// <summary>
        /// Get all NotificationTos
        /// </summary>
        /// <remarks>List of all NotificationTos</remarks>
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
                List<NotificationTo> oNotificationToList = new NotificationTo().LoadList().ToList();
                List<NotificationToDTO> oNotificationToDTOList = Mapper.Map<List<NotificationTo>, List<NotificationToDTO>>(oNotificationToList);

                SetCalculatedProperties(oNotificationToDTOList);

                return Ok(new { Items = oNotificationToDTOList, Count = oNotificationToDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/NotificationTo/guid
        /// <summary>
        /// Get NotificationTo
        /// </summary>
        /// <remarks>Get NotificationTo by specified unique identifier</remarks>
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
                if (string.IsNullOrEmpty(guid)) return Ok(new NotificationToDTO());
                NotificationTo oNotificationTo = new NotificationTo().Load(guid, true);
                if (oNotificationTo == null) return NotFound();
                NotificationToDTO oNotificationToDTO = Mapper.Map<NotificationTo, NotificationToDTO>(oNotificationTo);

                List<NotificationToDTO> oNotificationToDTOList = new List<NotificationToDTO>();
                oNotificationToDTOList.Add(oNotificationToDTO);
                SetCalculatedProperties(oNotificationToDTOList);

                return Ok(oNotificationToDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // POST api/NotificationTo
        /// <summary>
        /// Save NotificationTo 
        /// </summary>
        /// <remarks>Add new NotificationTo</remarks>
        /// <param name="oDTO">NotificationTo object to be saved</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost] //Save
        [Route()]
        public IHttpActionResult Post([FromBody] NotificationToDTO oNotificationToDTO)
        {
            try
            {
                if (oNotificationToDTO == null) BadRequest("No DTO passed");
                
                NotificationTo oNotificationTo = new NotificationTo().Load(where: "RepositoryID=" + oNotificationToDTO.RepositoryID + " And Email = '" + oNotificationToDTO.Email + "' And WebUserID = '" + oNotificationToDTO.WebUserID + "'");
                if(oNotificationTo == null)
                {
                    oNotificationTo = Mapper.Map<NotificationToDTO, NotificationTo>(oNotificationToDTO); //Mapper code
                }
               
                if (!oNotificationTo.IsValid) return BadRequest(oNotificationTo.Errors.ToModelState());
                oNotificationTo.AllowUpload = oNotificationToDTO.AllowUpload;
                oNotificationTo.Save();

                if (!oNotificationTo.IsValid) return BadRequest(oNotificationTo.Errors.ToModelState());
                oNotificationTo = new NotificationTo().Load(oNotificationTo.NotificationToID, true);
                oNotificationToDTO = Mapper.Map<NotificationTo, NotificationToDTO>(oNotificationTo);

                List<NotificationToDTO> oNotificationToDTOList = new List<NotificationToDTO>();
                oNotificationToDTOList.Add(oNotificationToDTO);
                SetCalculatedProperties(oNotificationToDTOList);

                return Ok(oNotificationToDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // PUT api/NotificationTo/guid
        /// <summary>
        /// Update NotificationTo
        /// </summary>
        /// <remarks>Update existing NotificationTo details</remarks>
        /// <param name="oNotificationToDTO">NotificationTo object to be modified</param>        
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut] //Update
        [Route()]
        public IHttpActionResult Put([FromBody] NotificationToDTO oNotificationToDTO)
        {
            try
            {
                if (oNotificationToDTO == null) BadRequest("No DTO passed");
                NotificationTo oNotificationTo = new NotificationTo().Load(oNotificationToDTO.NotificationToID);
                if (oNotificationTo == null) return NotFound();
                oNotificationTo = Mapper.Map<NotificationToDTO, NotificationTo>(oNotificationToDTO); //Mapper code

                if (!oNotificationTo.IsValid) return BadRequest(oNotificationTo.Errors.ToModelState());
                oNotificationTo.Update();
                oNotificationToDTO = Mapper.Map<NotificationTo, NotificationToDTO>(oNotificationTo);

                List<NotificationToDTO> oNotificationToDTOList = new List<NotificationToDTO>();
                oNotificationToDTOList.Add(oNotificationToDTO);
                SetCalculatedProperties(oNotificationToDTOList);

                return Ok(oNotificationToDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // DELETE api/NotificationTo/guid
        /// <summary>
        /// Deletes NotificationTo Record.
        /// </summary>
        /// <remarks>
        /// Deletes NotificationTo by the specified unique identifier.
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
                NotificationTo oNotificationTo = new NotificationTo().Load(guid);
                if (oNotificationTo != null) oNotificationTo.Delete();
                NotificationToDTO oNotificationToDTO = Mapper.Map<NotificationTo, NotificationToDTO>(oNotificationTo);

                return Ok(oNotificationToDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/NotificationTo/where
        /// <summary>
        /// Get NotificationTos
        /// </summary>
        /// <remarks>Get list of NotificationTos matching criteria</remarks>
        /// <param name="where">Where clause to get list of NotificationTo</param>
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
                List<NotificationTo> oNotificationToList = new NotificationTo().LoadList(where: where).ToList();
                List<NotificationToDTO> oNotificationToDTOList = Mapper.Map<List<NotificationTo>, List<NotificationToDTO>>(oNotificationToList);

                return Ok(new { Items = oNotificationToDTOList, Count = oNotificationToDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/NotificationTo/LoadList
        /// <summary>
        /// Get NotificationTos
        /// </summary>
        /// <remarks>Get list of NotificationTos by NotificationTo criteria object. Ref Model/DTO structure.</remarks>
        /// <param name="oWhere">NotificationTo DTO object with properties to included in where criteria</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("LoadList")]
        public IHttpActionResult LoadList([FromBody] NotificationToDTO oWhere)
        {
            try
            {
                var queryString = HttpContext.Current.Request.QueryString;
                int skip = Convert.ToInt32(queryString["$skip"]);
                int take = Convert.ToInt32(queryString["$top"]);
                NotificationTo oNotificationToCriteria = Mapper.Map<NotificationToDTO, NotificationTo>(oWhere);
                List<NotificationTo> oNotificationToList = new NotificationTo().LoadList(oNotificationToCriteria).ToList();
                List<NotificationToDTO> oNotificationToDTOList = Mapper.Map<List<NotificationTo>, List<NotificationToDTO>>(oNotificationToList);

                SetCalculatedProperties(oNotificationToDTOList);

                return Ok(new { Items = oNotificationToDTOList, Count = oNotificationToDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/NotificationTo/Sum/column/where
        [HttpPost]
        [Route("Sum/{column}/{where}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum(string column, string where)
        {
            try
            {
                var result = new NotificationTo().Sum(column: column, where: where);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/NotificationTo/Sum
        [HttpPost]
        [Route("Sum")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum([FromBody] NotificationToDTO oWhere)
        {
            try
            {
                NotificationTo oNotificationToCriteria = Mapper.Map<NotificationToDTO, NotificationTo>(oWhere);
                var result = new NotificationTo().Sum(oWhere.ScalarColumn, oNotificationToCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/NotificationTo/Min
        [HttpPost]
        [Route("Min")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Min([FromBody] NotificationToDTO oWhere)
        {
            try
            {
                NotificationTo oNotificationToCriteria = Mapper.Map<NotificationToDTO, NotificationTo>(oWhere);
                var result = new NotificationTo().Min(oWhere.ScalarColumn, oNotificationToCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/NotificationTo/Max
        [HttpPost]
        [Route("Max")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Max([FromBody] NotificationToDTO oWhere)
        {
            try
            {
                NotificationTo oNotificationToCriteria = Mapper.Map<NotificationToDTO, NotificationTo>(oWhere);
                var result = new NotificationTo().Max(oWhere.ScalarColumn, oNotificationToCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/NotificationTo/Count
        [HttpPost]
        [Route("Count")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Count([FromBody] NotificationToDTO oWhere)
        {
            try
            {
                NotificationTo oNotificationToCriteria = Mapper.Map<NotificationToDTO, NotificationTo>(oWhere);
                var result = new NotificationTo().Count(oNotificationToCriteria);
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

        // POST api/NotificationTo/Insert
        /// <summary>
        /// Add NotificationTo
        /// </summary>
        /// <remarks>Add NotificationTo using JSON object in the format of NotificationTo Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody] JObject jNotificationToDTO)
        {
            NotificationToDTO oNotificationToDTO = ((JObject)jNotificationToDTO.ToObject<JsonObject>().value).ToObject<NotificationToDTO>();
            return this.Post(oNotificationToDTO);
        }

        // POST api/NotificationTo/Update
        /// <summary>
        /// Uodate NotificationTo
        /// </summary>
        /// <remarks>Uodate NotificationTo using JSON object in the format of NotificationTo Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] JObject jNotificationToDTO)
        {
            NotificationToDTO oNotificationToDTO = ((JObject)jNotificationToDTO.ToObject<JsonObject>().value).ToObject<NotificationToDTO>();
            return this.Put(oNotificationToDTO);
        }

        // POST api/NotificationTo/Remove
        /// <summary>
        /// Delete NotificationTo
        /// </summary>
        /// <remarks>Delete NotificationTo using JSON object in the format of NotificationTo Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Remove")]
        public IHttpActionResult Remove([FromBody] JObject jNotificationToDTO)
        {
            string id = jNotificationToDTO.ToObject<JsonObject>().key.ToString();
            return this.Delete(id);
        }

        #endregion

        #region "Additional Functions"

        public void SetCalculatedProperties(List<NotificationToDTO> oNotificationToDTOList)
        {
            List<NotificationTo> oNotificationToList = new NotificationTo().LoadList().ToList();
            List<User> oUserList = new User().LoadList().ToList();

            foreach (NotificationToDTO oNotificationToDTO in oNotificationToDTOList)
            {
                //List<NotificationTo> oNotificationTo = oNotificationToList.FindAll(u => u.NotificationToID == oNotificationToDTO.NotificationToID);
                //string oNotificationToEmails = string.Join("', '", oNotificationTo.Select(o => o.Email));
                //List<User> oUsers = oUserList.FindAll(u => u.EMailAddress.Contains(oNotificationToEmails));
                //string oNotificationToUserIDs = string.Join(", ", oUsers.Select(o => o.UserID));
                //oNotificationToDTO.NotificationToUserIDs = oNotificationToUserIDs;
            }
        }

        #endregion
    }
}
