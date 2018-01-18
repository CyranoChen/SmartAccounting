using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Sap.SmartAccounting.Core
{
    public class RestClient
    {
        public virtual string ApiGet(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var client = new WebClient();

                using (var responseStream = client.OpenRead(url))
                {
                    if (responseStream != null)
                    {
                        var readStream = new StreamReader(responseStream, Encoding.UTF8);

                        return readStream.ReadToEnd();
                    }
                }
            }

            return null;
        }

        protected virtual string ApiPost(string url, string data)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var client = new WebClient();

                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Headers.Add("ContentLength", data.Length.ToString());

                var responseResult = client.UploadData(url, RequestMethod.Post.ToString().ToUpper()
                    , Encoding.UTF8.GetBytes(data));

                return Encoding.UTF8.GetString(responseResult);
            }

            return null;
        }

        #region Members and Properties

        protected string ServiceUrl { get; set; }

        protected string AppKey { get; set; }

        protected string CryptographicKey { get; set; }

        protected string Method { get; set; }

        protected ResponseType Format { get; set; }

        protected SortedDictionary<string, string> Parameters { get; set; }

        #endregion
    }

    public enum ResponseType
    {
        Xml,
        Json
    }

    public enum RequestMethod
    {
        Get,
        Post
    }
}