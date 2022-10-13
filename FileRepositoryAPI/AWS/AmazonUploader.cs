using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon;
using Amazon.S3.Model;
using System.IO;
using System.Configuration;

namespace C2OPS.WebAPI
{
    public class AmazonUploader
    {
        public bool sendMyFileToS3(System.IO.Stream localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3)
        {
            try
            {
                IAmazonS3 awsClient = GetAWSClient();
                TransferUtility utility = new TransferUtility(awsClient);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

                if (subDirectoryInBucket == "" || subDirectoryInBucket == null)
                {
                    request.BucketName = bucketName; //no subdirectory just bucket name  
                }
                else
                {   // subdirectory and bucket name  
                    request.BucketName = bucketName + @"/" + subDirectoryInBucket;
                }
                request.Key = fileNameInS3; //file name up in S3  
                request.InputStream = localFilePath;
                utility.Upload(request); //commensing the transfer  
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Check the provided AWS Credentials." + amazonS3Exception.ErrorCode);
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }

            return true; //indicate that the file was sent  
        }

        public string getSignedUrl(string fileNameInS3)
        {
            IAmazonS3 awsClient = GetAWSClient();
            Amazon.S3.Model.GetPreSignedUrlRequest req = new Amazon.S3.Model.GetPreSignedUrlRequest();
            req.Key = fileNameInS3;
            string url = awsClient.GetPreSignedURL(req);

            return url; //indicate that the file was sent  
        }

        public byte[] getObjectStreamBytes(string fileNameInS3)
        {
            MemoryStream memoryStream = null;

            try
            {
                string sAWSBucketName = "c2ops-bucket";
                string sAWSSubDirectoryInBucket = "facility-images";
                if (ConfigurationManager.AppSettings["AWSBucketName"] != null) { sAWSBucketName = ConfigurationManager.AppSettings["AWSBucketName"].ToString(); }
                if (ConfigurationManager.AppSettings["AWSSubDirectoryInBucket"] != null) { sAWSSubDirectoryInBucket = ConfigurationManager.AppSettings["AWSSubDirectoryInBucket"].ToString(); }

                IAmazonS3 awsClient = GetAWSClient();               
                GetObjectRequest request = new GetObjectRequest();
                request.BucketName = sAWSBucketName + "/" + sAWSSubDirectoryInBucket;
                request.Key = fileNameInS3;
                using (GetObjectResponse response = awsClient.GetObject(request))
                {
                    using (Stream responseStream = response.ResponseStream)
                    {
                        memoryStream = new MemoryStream();
                        responseStream.CopyTo(memoryStream);
                    }
                }
            }
            catch // (AmazonS3Exception amazonS3Exception)
            {
               
                //if (amazonS3Exception.ErrorCode != null &&
                //    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                //    ||
                //    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                //{
                //    throw new Exception("Check the provided AWS Credentials." + amazonS3Exception.ErrorCode);
                //}
                //else
                //{
                //    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                //}
            }
            if (memoryStream == null) return getNoLogoStreamBytes();
            return memoryStream.ToArray();
        }

        public byte[] getNoLogoStreamBytes()
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                string sAWSBucketName = "c2ops-bucket";
                string sAWSSubDirectoryInBucket = "facility-images";
                if (ConfigurationManager.AppSettings["AWSBucketName"] != null) { sAWSBucketName = ConfigurationManager.AppSettings["AWSBucketName"].ToString(); }
                if (ConfigurationManager.AppSettings["AWSSubDirectoryInBucket"] != null) { sAWSSubDirectoryInBucket = ConfigurationManager.AppSettings["AWSSubDirectoryInBucket"].ToString(); }

                IAmazonS3 awsClient = GetAWSClient();
                GetObjectRequest request = new GetObjectRequest();
                request.BucketName = sAWSBucketName + "/" + sAWSSubDirectoryInBucket;

                request.Key = "nologo.jpg";
                GetObjectResponse response = awsClient.GetObject(request);

                using (Stream responseStream = response.ResponseStream)
                {
                    responseStream.CopyTo(memoryStream);
                }
            }
            catch // (AmazonS3Exception amazonS3Exception)
            {
            }
            if (memoryStream == null) return null;
            return memoryStream.ToArray();
        }

        private IAmazonS3 GetAWSClient()
        {
            string sAccessKey = "AKIA4ZVU664KDGB5DXIS";
            string sSecretKey = "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";
            if (ConfigurationManager.AppSettings["AWSAccessKey"] != null) { sAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"].ToString(); }
            if (ConfigurationManager.AppSettings["AWSSecretKey"] != null) { sSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"].ToString(); }
            IAmazonS3 client = new AmazonS3Client(sAccessKey, sSecretKey, RegionEndpoint.USWest1);
            return client;
        }
    }
}
