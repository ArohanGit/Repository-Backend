using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Web.Http.Description;
using FileRepository.BusinessObjects;

namespace FileRepositoryAPI
{
    [RoutePrefix("api/Test")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestController : ApiController
    {
        // GET api/User
        [HttpGet]
        [Route()]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok("Success...!!!");
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/CheckConnection
        [HttpGet]
        [Route("CheckConnection")]
        public IHttpActionResult CheckConnection()
        {
            try
            {
                string sConStr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                System.Data.SqlClient.SqlConnection scon = new System.Data.SqlClient.SqlConnection(sConStr);
                scon.Open();
                return Ok("Success...!!! \n Connection String :" + sConStr + " Connection State :" + scon.State);
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/CheckConnection
        [HttpGet]
        [Route("TestMail")]
        public IHttpActionResult TestMail()
        {
            try
            {
                // FileRepository.BusinessObjects.Common.SendMail("prakash@arohansystems.com", "Test Body");
                return Ok("Mail Sent");
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        [HttpGet]
        [Route("SendNotification")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult SendNotification()
        {
            try
            {
                //FileRepositoryNote.SendNotification(134);
                return Ok("Notification Sent Successfully.");
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        static HttpClient client = new HttpClient();
        public object obj { get; set; }
    }

    public class mailDTO
    {
        public string _from { get; set; }
        public string _to { get; set; }
        public string _subject { get; set; }
        public string _body { get; set; }
        public bool _IsBodyHtml { get; set; }
        public bool _EnableSSL { get; set; }
        public string _hostname { get; set; }
        public int _port { get; set; }
        public string _UserName { get; set; }
        public string _Password { get; set; }
    }
}
