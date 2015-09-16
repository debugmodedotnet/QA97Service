using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using QA97Service.Models;
using QA97Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QA97Service.Controllers
{
    public class ImageUploadController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        public ImgReturn Post()
        {

            HttpStatusCode result = new HttpStatusCode();

            var httpRequest = HttpContext.Current.Request;
            var url = ""; var msg = "";
            try
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    //var filePath = HttpContext.Current.Server.MapPath("~/img/" + postedFile.FileName);

                    //postedFile.SaveAs(filePath);
                    //url = "/img/" + postedFile.FileName.ToString();
                    //docfiles.Add(filePath);

                    //Azure Upload
                    var contentType = postedFile.ContentType;
                    var streamContents = postedFile.InputStream;
                    var blobName = postedFile.FileName;
                    string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", "debugmodelab", "t2yJAWoR8tcL0fcz6TbP/5m3fSmgWS0qIAY2aj8G9k4vbhdzlKsKpmrHJ3AgWP5GsyTkPM8g9lGSyPG2MhNVzQ==");
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(UserConnectionString);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer blobContainer = blobClient.GetContainerReference("masterjee");
                    blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });


                    //Random Name
                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    var stringChars = new char[8];
                    var random = new Random();

                    for (int i = 0; i < stringChars.Length; i++)
                        stringChars[i] = chars[random.Next(chars.Length)];

                    var finalString = new String(stringChars);
                    //Random Name

                    var blob1 = blobContainer.GetBlockBlobReference(finalString);
                    blob1.Properties.ContentType = contentType;
                    blob1.UploadFromStream(streamContents);

                    url = "https://debugmodelab.blob.core.windows.net/" + blobContainer.Name + "/" + finalString;
                    //Azure Upload

                }
                result = HttpStatusCode.Created;
                if (url != "")
                    msg = "Uploaded Successfully";


            }
            catch (Exception errMsg)
            {
                result = HttpStatusCode.BadRequest;
                msg = "Error Uploading file" + errMsg;

            }

            var userName = httpRequest.Form["username"];
            if (userName != null)
            {
                UserImagesController ctrl = new UserImagesController();
                Entities.UserImage Imgobj = new Entities.UserImage();
                string idOfUserAnswering = (from r in db.Users where r.UserName == userName select r.Id).FirstOrDefault();
                Imgobj.ImageUrl = url;
                Imgobj.CreatedDate = DateTime.Now;
                Imgobj.ModifiedBy = idOfUserAnswering;
                Imgobj.ModifiedDate = DateTime.Now;
                Imgobj.CreatedBy = idOfUserAnswering;
                Imgobj.UserId = idOfUserAnswering;
                ctrl.PostUserImage(Imgobj);
            }
            ImgReturn a = new ImgReturn();
            a.imgUrl = url;
            a.response = result;
            a.msg = msg;
            return a;
        }



    }
}
