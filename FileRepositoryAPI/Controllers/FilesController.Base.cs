

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
    /// Files
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/Files")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AppAuthenticationFilter]
    public partial class FilesController : ApiController
    {
        static bool initialized = false;

        static FilesController()
        {
            if (!initialized)
            {
                Mapper.CreateMap<Files, FilesDTO>();
                Mapper.CreateMap<FilesDTO, Files>();
                // Child Object Mapping

                initialized = true;
            }
        }


        #region "Generated Services"

        // GET api/Files/DTO
        /// <summary>
        /// Get Files Model DTO
        /// </summary>
        /// <remarks>Files Model</remarks>
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
                FilesDTO entityDTO = new FilesDTO();

                return Ok(entityDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Files
        /// <summary>
        /// Get all Filess
        /// </summary>
        /// <remarks>List of all Filess</remarks>
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
                List<Files> oFilesList = new Files().LoadList().ToList();
                List<FilesDTO> oFilesDTOList = Mapper.Map<List<Files>, List<FilesDTO>>(oFilesList);

                SetCalculatedProperties(oFilesDTOList);

                return Ok(new { Items = oFilesDTOList, Count = oFilesDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Files/guid
        /// <summary>
        /// Get Files
        /// </summary>
        /// <remarks>Get Files by specified unique identifier</remarks>
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
                if (string.IsNullOrEmpty(guid)) return Ok(new FilesDTO());
                Files oFiles = new Files().Load(guid, true);
                if (oFiles == null) return NotFound();
                FilesDTO oFilesDTO = Mapper.Map<Files, FilesDTO>(oFiles);

                List<FilesDTO> oFilesDTOList = new List<FilesDTO>();
                oFilesDTOList.Add(oFilesDTO);
                SetCalculatedProperties(oFilesDTOList);

                return Ok(oFilesDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // POST api/Files
        /// <summary>
        /// Save Files 
        /// </summary>
        /// <remarks>Add new Files</remarks>
        /// <param name="oDTO">Files object to be saved</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost] //Save
        [Route()]
        public IHttpActionResult Post([FromBody] FilesDTO oFilesDTO)
        {
            try
            {
                if (oFilesDTO == null) BadRequest("No DTO passed");
                Files oFiles = Mapper.Map<FilesDTO, Files>(oFilesDTO); //Mapper code

                if (!oFiles.IsValid) return BadRequest(oFiles.Errors.ToModelState());
                oFiles.Save();

                if (!oFiles.IsValid) return BadRequest(oFiles.Errors.ToModelState());
                oFiles = new Files().Load(oFiles.FilesID, true);
                oFilesDTO = Mapper.Map<Files, FilesDTO>(oFiles);

                List<FilesDTO> oFilesDTOList = new List<FilesDTO>();
                oFilesDTOList.Add(oFilesDTO);
                SetCalculatedProperties(oFilesDTOList);

                return Ok(oFilesDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // PUT api/Files/guid
        /// <summary>
        /// Update Files
        /// </summary>
        /// <remarks>Update existing Files details</remarks>
        /// <param name="oFilesDTO">Files object to be modified</param>        
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut] //Update
        [Route()]
        public IHttpActionResult Put([FromBody] FilesDTO oFilesDTO)
        {
            try
            {
                if (oFilesDTO == null) BadRequest("No DTO passed");
                Files oFiles = new Files().Load(oFilesDTO.FilesID);
                if (oFiles == null) return NotFound();
                oFiles = Mapper.Map<FilesDTO, Files>(oFilesDTO); //Mapper code

                if (!oFiles.IsValid) return BadRequest(oFiles.Errors.ToModelState());
                oFiles.Update();
                oFilesDTO = Mapper.Map<Files, FilesDTO>(oFiles);

                List<FilesDTO> oFilesDTOList = new List<FilesDTO>();
                oFilesDTOList.Add(oFilesDTO);
                SetCalculatedProperties(oFilesDTOList);

                return Ok(oFilesDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // DELETE api/Files/guid
        /// <summary>
        /// Deletes Files Record.
        /// </summary>
        /// <remarks>
        /// Deletes Files by the specified unique identifier.
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
                Files oFiles = new Files().Load(guid);
                if (oFiles != null) oFiles.Delete();
                FilesDTO oFilesDTO = Mapper.Map<Files, FilesDTO>(oFiles);

                return Ok(oFilesDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Files/where
        /// <summary>
        /// Get Filess
        /// </summary>
        /// <remarks>Get list of Filess matching criteria</remarks>
        /// <param name="where">Where clause to get list of Files</param>
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
                List<Files> oFilesList = new Files().LoadList(where: where).ToList();
                List<FilesDTO> oFilesDTOList = Mapper.Map<List<Files>, List<FilesDTO>>(oFilesList);

                return Ok(new { Items = oFilesDTOList, Count = oFilesDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Files/LoadList
        /// <summary>
        /// Get Filess
        /// </summary>
        /// <remarks>Get list of Filess by Files criteria object. Ref Model/DTO structure.</remarks>
        /// <param name="oWhere">Files DTO object with properties to included in where criteria</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("LoadList")]
        public IHttpActionResult LoadList([FromBody] FilesDTO oWhere)
        {
            try
            {
                var queryString = HttpContext.Current.Request.QueryString;
                int skip = Convert.ToInt32(queryString["$skip"]);
                int take = Convert.ToInt32(queryString["$top"]);
                Files oFilesCriteria = Mapper.Map<FilesDTO, Files>(oWhere);
                List<Files> oFilesList = new Files().LoadList(oFilesCriteria).ToList();
                List<FilesDTO> oFilesDTOList = Mapper.Map<List<Files>, List<FilesDTO>>(oFilesList);

                SetCalculatedProperties(oFilesDTOList);

                return Ok(new { Items = oFilesDTOList, Count = oFilesDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Files/Sum/column/where
        [HttpPost]
        [Route("Sum/{column}/{where}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum(string column, string where)
        {
            try
            {
                var result = new Files().Sum(column: column, where: where);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Files/Sum
        [HttpPost]
        [Route("Sum")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum([FromBody] FilesDTO oWhere)
        {
            try
            {
                Files oFilesCriteria = Mapper.Map<FilesDTO, Files>(oWhere);
                var result = new Files().Sum(oWhere.ScalarColumn, oFilesCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Files/Min
        [HttpPost]
        [Route("Min")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Min([FromBody] FilesDTO oWhere)
        {
            try
            {
                Files oFilesCriteria = Mapper.Map<FilesDTO, Files>(oWhere);
                var result = new Files().Min(oWhere.ScalarColumn, oFilesCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Files/Max
        [HttpPost]
        [Route("Max")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Max([FromBody] FilesDTO oWhere)
        {
            try
            {
                Files oFilesCriteria = Mapper.Map<FilesDTO, Files>(oWhere);
                var result = new Files().Max(oWhere.ScalarColumn, oFilesCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Files/Count
        [HttpPost]
        [Route("Count")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Count([FromBody] FilesDTO oWhere)
        {
            try
            {
                Files oFilesCriteria = Mapper.Map<FilesDTO, Files>(oWhere);
                var result = new Files().Count(oFilesCriteria);
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

        // POST api/Files/Insert
        /// <summary>
        /// Add Files
        /// </summary>
        /// <remarks>Add Files using JSON object in the format of Files Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody] JObject jFilesDTO)
        {
            FilesDTO oFilesDTO = ((JObject)jFilesDTO.ToObject<JsonObject>().value).ToObject<FilesDTO>();
            return this.Post(oFilesDTO);
        }

        // POST api/Files/Update
        /// <summary>
        /// Uodate Files
        /// </summary>
        /// <remarks>Uodate Files using JSON object in the format of Files Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] JObject jFilesDTO)
        {
            FilesDTO oFilesDTO = ((JObject)jFilesDTO.ToObject<JsonObject>().value).ToObject<FilesDTO>();
            return this.Put(oFilesDTO);
        }

        // POST api/Files/Remove
        /// <summary>
        /// Delete Files
        /// </summary>
        /// <remarks>Delete Files using JSON object in the format of Files Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Remove")]
        public IHttpActionResult Remove([FromBody] JObject jFilesDTO)
        {
            string id = jFilesDTO.ToObject<JsonObject>().key.ToString();
            return this.Delete(id);
        }

        #endregion

        #region "Additional Functions"

        public void SetCalculatedProperties(List<FilesDTO> oFilesDTOList)
        {
            List<NotificationTo> oNotificationToList = new NotificationTo().LoadList().ToList();
            List<User> oUserList = new User().LoadList().ToList();

            foreach (FilesDTO oFilesDTO in oFilesDTOList)
            {
                //List<NotificationTo> oNotificationTo = oNotificationToList.FindAll(u => u.FilesID == oFilesDTO.FilesID);
                //string oNotificationToEmails = string.Join("', '", oNotificationTo.Select(o => o.Email));
                //List<User> oUsers = oUserList.FindAll(u => u.EMailAddress.Contains(oNotificationToEmails));
                //string oNotificationToUserIDs = string.Join(", ", oUsers.Select(o => o.UserID));
                //oFilesDTO.NotificationToUserIDs = oNotificationToUserIDs;
            }
        }

        #endregion
    }
}
