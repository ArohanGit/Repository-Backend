using FileRepository.BusinessObjects;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FileRepositoryAPI.WebAPI
{
    [RoutePrefix("api/ApproveReject")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApproveRejectController : ApiController
    {
        // GET api/ApproveRepository
        [HttpGet]
        [Route("ApproveRepository/{approveDet}")]
        public IHttpActionResult ApproveRepository(string approveDet)
        {
            try
            {
                string[] sApproveDet = approveDet.Split('|');
                if (sApproveDet.Length <= 0) return Ok("");
                int? nRepositoryID = Convert.ToInt32(sApproveDet[0]);
                string sWebUserID = Convert.ToString(sApproveDet[1]);

                Repository oRepository = new Repository().Load(nRepositoryID);
                if (oRepository.ApprovalLevel == 0 || oRepository.IsApproved == "Y") { return ResponseMessage(this.ReturnResponseMessage("Already rejected.! Can not change status to approve.")); }
                //{ return Ok(new { response = "Already rejected.! Can not change status to approve." }); }

                // Action Already Taken
                NotificationTo oNotificationTo = new NotificationTo().Load(where: "RepositoryID=" + oRepository.RepositoryID + " And WebUserID='" + sWebUserID + "'");
                if (oRepository.ApprovalLevel != oNotificationTo.ApproverLevel) { return ResponseMessage(this.ReturnResponseMessage("Action already taken. Can not change status." )); }

                // If Is Owner & Documents not Uploaded then return.
                int? nCnt = new Files().Count(where: "RepositoryID=" + oRepository.RepositoryID);
                if (oRepository.ApprovalLevel == oNotificationTo.ApproverLevel && oNotificationTo.AllowUpload == "Y" && nCnt <= 0) { return ResponseMessage(this.ReturnResponseMessage("Can not change status please attach files and try again.")); }

                new Repository().ApproveRepository(oRepository, sWebUserID);
               
                return ResponseMessage(this.ReturnResponseMessage("Repository approved successfully."));
                               
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        // GET api/RejectRepository
        [HttpGet]
        [Route("RejectRepository/{approveDet}")]
        public IHttpActionResult RejectRepository(string approveDet)
        {
            try
            {
                string[] sApproveDet = approveDet.Split('|');
                if (sApproveDet.Length <= 0) return Ok("");
                int? nRepositoryID = Convert.ToInt32(sApproveDet[0]);
                string sWebUserID = Convert.ToString(sApproveDet[1]);
                int? nPrevLvl = 0;

                User oUser = new User().Load(where: "WebUserID='" + sWebUserID + "'");
                Repository oRepository = new Repository().Load(nRepositoryID);
                if (oRepository.IsApproved == "Y") { return ResponseMessage(this.ReturnResponseMessage("Already approved. Can not change status to reject.")); }
                nPrevLvl = oRepository.ApprovalLevel;

                // Action Already Taken
                NotificationTo oNotificationTo = new NotificationTo().Load(where: "RepositoryID=" + oRepository.RepositoryID + " And WebUserID='" + sWebUserID + "'");
                if (oRepository.ApprovalLevel != oNotificationTo.ApproverLevel) { return ResponseMessage(this.ReturnResponseMessage("Action already taken. Can not change status.")); }

                oRepository.ApprovalLevel = 0;
                oRepository.RejectRemark = "Rejected by Approver " + oUser.Name;
                oRepository.Save();

                // Send Notification
                oNotificationTo = new NotificationTo().Load(where: "RepositoryID=" + oRepository.RepositoryID + " And ApproverLevel=" + nPrevLvl);
                if (oNotificationTo == null) { return Ok("Repository rejected successfully."); }
                Repository.SendRejectNotification(oRepository, oNotificationTo.WebUserID);

                return ResponseMessage(this.ReturnResponseMessage("Repository rejected successfully."));
            }
            catch (Exception ex)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(msg);
            }
        }

        private HttpResponseMessage ReturnResponseMessage(string formattedText)
        {
            formattedText = "<b><h2>" + formattedText + " Please close this window." + "</h2></b>";
            string mediaType = "text/html";

            // Create an HttpResponseMessage with the formatted text and media type
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(formattedText, System.Text.Encoding.UTF8, mediaType);
            return response;
            // Return the IHttpActionResult instance with the HttpResponseMessage
        }
    }
}