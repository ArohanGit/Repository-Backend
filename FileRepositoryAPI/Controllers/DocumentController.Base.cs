

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
    /// Document
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/Document")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AppAuthenticationFilter]
    public partial class DocumentController : ApiController
    {
        static bool initialized = false;

        static DocumentController()
        {
            if (!initialized)
            {
                Mapper.CreateMap<Document, DocumentDTO>();
                Mapper.CreateMap<DocumentDTO, Document>();
                // Child Object Mapping

                initialized = true;
            }
        }


        #region "Generated Services"

        // GET api/Document/DTO
        /// <summary>
        /// Get Document Model DTO
        /// </summary>
        /// <remarks>Document Model</remarks>
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
                DocumentDTO entityDTO = new DocumentDTO();

                return Ok(entityDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Document
        /// <summary>
        /// Get all Documents
        /// </summary>
        /// <remarks>List of all Documents</remarks>
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
                List<Document> oDocumentList = new Document().LoadList().ToList();
                List<DocumentDTO> oDocumentDTOList = Mapper.Map<List<Document>, List<DocumentDTO>>(oDocumentList);

                SetCalculatedProperties(oDocumentDTOList);

                return Ok(new { Items = oDocumentDTOList, Count = oDocumentDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Document/guid
        /// <summary>
        /// Get Document
        /// </summary>
        /// <remarks>Get Document by specified unique identifier</remarks>
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
                if (string.IsNullOrEmpty(guid)) return Ok(new DocumentDTO());
                Document oDocument = new Document().Load(guid, true);
                if (oDocument == null) return NotFound();
                DocumentDTO oDocumentDTO = Mapper.Map<Document, DocumentDTO>(oDocument);

                List<DocumentDTO> oDocumentDTOList = new List<DocumentDTO>();
                oDocumentDTOList.Add(oDocumentDTO);
                SetCalculatedProperties(oDocumentDTOList);

                return Ok(oDocumentDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // POST api/Document
        /// <summary>
        /// Save Document 
        /// </summary>
        /// <remarks>Add new Document</remarks>
        /// <param name="oDTO">Document object to be saved</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost] //Save
        [Route()]
        public IHttpActionResult Post([FromBody] DocumentDTO oDocumentDTO)
        {
            try
            {
                if (oDocumentDTO == null) BadRequest("No DTO passed");
                Document oDocument = Mapper.Map<DocumentDTO, Document>(oDocumentDTO); //Mapper code

                if (!oDocument.IsValid) return BadRequest(oDocument.Errors.ToModelState());
                oDocument.Save();

                if (!oDocument.IsValid) return BadRequest(oDocument.Errors.ToModelState());
                oDocument = new Document().Load(oDocument.DocumentID, true);
                oDocumentDTO = Mapper.Map<Document, DocumentDTO>(oDocument);

                List<DocumentDTO> oDocumentDTOList = new List<DocumentDTO>();
                oDocumentDTOList.Add(oDocumentDTO);
                SetCalculatedProperties(oDocumentDTOList);

                return Ok(oDocumentDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "*Unhandled Exception Occured*.", ex);
                throw new HttpResponseException(msg);
            }
        }

        // PUT api/Document/guid
        /// <summary>
        /// Update Document
        /// </summary>
        /// <remarks>Update existing Document details</remarks>
        /// <param name="oDocumentDTO">Document object to be modified</param>        
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut] //Update
        [Route()]
        public IHttpActionResult Put([FromBody] DocumentDTO oDocumentDTO)
        {
            try
            {
                if (oDocumentDTO == null) BadRequest("No DTO passed");
                Document oDocument = new Document().Load(oDocumentDTO.DocumentID);
                if (oDocument == null) return NotFound();
                oDocument = Mapper.Map<DocumentDTO, Document>(oDocumentDTO); //Mapper code

                if (!oDocument.IsValid) return BadRequest(oDocument.Errors.ToModelState());
                oDocument.Update();
                oDocumentDTO = Mapper.Map<Document, DocumentDTO>(oDocument);

                List<DocumentDTO> oDocumentDTOList = new List<DocumentDTO>();
                oDocumentDTOList.Add(oDocumentDTO);
                SetCalculatedProperties(oDocumentDTOList);

                return Ok(oDocumentDTOList[0]);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // DELETE api/Document/guid
        /// <summary>
        /// Deletes Document Record.
        /// </summary>
        /// <remarks>
        /// Deletes Document by the specified unique identifier.
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
                Document oDocument = new Document().Load(guid);
                if (oDocument != null) oDocument.Delete();
                DocumentDTO oDocumentDTO = Mapper.Map<Document, DocumentDTO>(oDocument);

                return Ok(oDocumentDTO);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Document/where
        /// <summary>
        /// Get Documents
        /// </summary>
        /// <remarks>Get list of Documents matching criteria</remarks>
        /// <param name="where">Where clause to get list of Document</param>
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
                List<Document> oDocumentList = new Document().LoadList(where: where).ToList();
                List<DocumentDTO> oDocumentDTOList = Mapper.Map<List<Document>, List<DocumentDTO>>(oDocumentList);

                return Ok(new { Items = oDocumentDTOList, Count = oDocumentDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Document/LoadList
        /// <summary>
        /// Get Documents
        /// </summary>
        /// <remarks>Get list of Documents by Document criteria object. Ref Model/DTO structure.</remarks>
        /// <param name="oWhere">Document DTO object with properties to included in where criteria</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("LoadList")]
        public IHttpActionResult LoadList([FromBody] DocumentDTO oWhere)
        {
            try
            {
                var queryString = HttpContext.Current.Request.QueryString;
                int skip = Convert.ToInt32(queryString["$skip"]);
                int take = Convert.ToInt32(queryString["$top"]);
                Document oDocumentCriteria = Mapper.Map<DocumentDTO, Document>(oWhere);
                List<Document> oDocumentList = new Document().LoadList(oDocumentCriteria).ToList();
                List<DocumentDTO> oDocumentDTOList = Mapper.Map<List<Document>, List<DocumentDTO>>(oDocumentList);

                SetCalculatedProperties(oDocumentDTOList);

                return Ok(new { Items = oDocumentDTOList, Count = oDocumentDTOList.Count });
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Document/Sum/column/where
        [HttpPost]
        [Route("Sum/{column}/{where}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum(string column, string where)
        {
            try
            {
                var result = new Document().Sum(column: column, where: where);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Document/Sum
        [HttpPost]
        [Route("Sum")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Sum([FromBody] DocumentDTO oWhere)
        {
            try
            {
                Document oDocumentCriteria = Mapper.Map<DocumentDTO, Document>(oWhere);
                var result = new Document().Sum(oWhere.ScalarColumn, oDocumentCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Document/Min
        [HttpPost]
        [Route("Min")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Min([FromBody] DocumentDTO oWhere)
        {
            try
            {
                Document oDocumentCriteria = Mapper.Map<DocumentDTO, Document>(oWhere);
                var result = new Document().Min(oWhere.ScalarColumn, oDocumentCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Document/Max
        [HttpPost]
        [Route("Max")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Max([FromBody] DocumentDTO oWhere)
        {
            try
            {
                Document oDocumentCriteria = Mapper.Map<DocumentDTO, Document>(oWhere);
                var result = new Document().Max(oWhere.ScalarColumn, oDocumentCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/Document/Count
        [HttpPost]
        [Route("Count")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Count([FromBody] DocumentDTO oWhere)
        {
            try
            {
                Document oDocumentCriteria = Mapper.Map<DocumentDTO, Document>(oWhere);
                var result = new Document().Count(oDocumentCriteria);
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

        // POST api/Document/Insert
        /// <summary>
        /// Add Document
        /// </summary>
        /// <remarks>Add Document using JSON object in the format of Document Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody] JObject jDocumentDTO)
        {
            DocumentDTO oDocumentDTO = ((JObject)jDocumentDTO.ToObject<JsonObject>().value).ToObject<DocumentDTO>();
            return this.Post(oDocumentDTO);
        }

        // POST api/Document/Update
        /// <summary>
        /// Uodate Document
        /// </summary>
        /// <remarks>Uodate Document using JSON object in the format of Document Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] JObject jDocumentDTO)
        {
            DocumentDTO oDocumentDTO = ((JObject)jDocumentDTO.ToObject<JsonObject>().value).ToObject<DocumentDTO>();
            return this.Put(oDocumentDTO);
        }

        // POST api/Document/Remove
        /// <summary>
        /// Delete Document
        /// </summary>
        /// <remarks>Delete Document using JSON object in the format of Document Model/DTO</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Remove")]
        public IHttpActionResult Remove([FromBody] JObject jDocumentDTO)
        {
            string id = jDocumentDTO.ToObject<JsonObject>().key.ToString();
            return this.Delete(id);
        }

        #endregion

        #region "Additional Functions"

        public void SetCalculatedProperties(List<DocumentDTO> oDocumentDTOList)
        {
            List<NotificationTo> oNotificationToList = new NotificationTo().LoadList().ToList();
            List<User> oUserList = new User().LoadList().ToList();

            foreach (DocumentDTO oDocumentDTO in oDocumentDTOList)
            {
                List<NotificationTo> oNotificationTo = oNotificationToList.FindAll(u => u.DocumentID == oDocumentDTO.DocumentID);
                string oNotificationToEmails = string.Join("', '", oNotificationTo.Select(o => o.Email));
                List<User> oUsers = oUserList.FindAll(u => u.EMailAddress.Contains(oNotificationToEmails));
                string oNotificationToUserIDs = string.Join(", ", oUsers.Select(o => o.UserID));
                oDocumentDTO.NotificationToUserIDs = oNotificationToUserIDs;
            }
        }

        #endregion
    }
}
