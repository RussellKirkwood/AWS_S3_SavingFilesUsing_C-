public class AWSStorageHelper
    {
            private static readonly string _awsAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            private static readonly string _awsSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            
            private static readonly string _bucketName = "xx";

            public string determineAttachmentExtension(string ContentType)
            {
                string AttachmentExtension = ContentType.Substring(ContentType.IndexOf('/') + 1).ToLower();

                if (AttachmentExtension == "jpeg")
                {
                    AttachmentExtension = "jpg";
                }

                if (AttachmentExtension == "tiff")
                {
                    AttachmentExtension = "tif";
                }

                return AttachmentExtension;
            }

            public string determineAttachmentExtensionByURL(string URL)
            {
                string AttachmentExtension = URL.Substring(URL.IndexOf('.') + 1).ToLower();

                if (AttachmentExtension == "jpeg")
                {
                    AttachmentExtension = "jpg";
                }

                if (AttachmentExtension == "tiff")
                {
                    AttachmentExtension = "tif";
                }

                return AttachmentExtension;
            }

            public string determineAttachmentFileName(string Uri)
            {
                string AttachmentFileName = Uri.Substring(Uri.LastIndexOf('/') + 1).ToLower();

                return AttachmentFileName;
            }


            public string UploadtoS3(string fileurl, string filetype, string folder, string prefix, string filename)
            {
                string returnString = "";
                Guid newGuid = Guid.NewGuid();
                var newGuidstr = newGuid.ToString();

                string url = fileurl;
                byte[] imageData;
                using (WebClient client = new WebClient())
                {
                    imageData = client.DownloadData(url);
                }

                try
                {
                    IAmazonS3 client;
                    using (client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, Amazon.RegionEndpoint.USEast1))
                    {
                        var request = new PutObjectRequest()
                        {
                            BucketName = _bucketName,
                            CannedACL = S3CannedACL.PublicRead,                                                              
                            Key = string.Format(folder + "/{0}", prefix + "-" + newGuidstr.Substring(0, 4) + "-" + filename + "." + filetype)
                                     
                        };

                        using (var ms = new MemoryStream(imageData))
                        {
                            request.InputStream = ms;
                            client.PutObject(request);                            
                            returnString = "https://s3.amazonaws.com/xx/" + request.Key;
                        }
                    }
                }
                catch (Exception ex)
                {
                    returnString = ex.Message;
                }

                return returnString;

            }            

            public ResourceData UploadtoAWSByte(byte[] imageData, string filename )
            {
            var ResourceData = new ResourceData();

            string returnString = "";
                Guid newGuid = Guid.NewGuid();
                var newGuidstr = newGuid.ToString();

                try
                {
                    IAmazonS3 client;
                       using (client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, Amazon.RegionEndpoint.USEast1))
                    {
                        var request = new PutObjectRequest()
                        {
                            BucketName = _bucketName,
                            CannedACL = S3CannedACL.PublicRead,                                                               
                            Key = string.Format("xxxxx" + "/{0}", "xxx" + "-" + newGuidstr.Substring(0, 4) + "-" + filename + ".pdf")
                        };

                        using (var ms = new MemoryStream(imageData))
                        {
                            request.InputStream = ms;
                            client.PutObject(request);                            
                            returnString = "https://s3.amazonaws.com/xx/" + request.Key;
                        }
                    }
                }
                catch (Exception ex)
                {
                    returnString = ex.Message;
                }

                ResourceData.ResourceURL2Primary = returnString;

                return ResourceData;

            }
        }
