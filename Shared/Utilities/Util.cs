using System.Net;
using System.Text;

namespace ADIRA.Shared.Utilities
{
    public static class Util
    {
        private static readonly Random _rand = new();

        public static int GetRandomNumber(int min, int max)
        {
            return _rand.Next(min, max);
        }

        public static string EncodeRAW(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }

        public static HttpWebRequest CreateHttpWebRequest(string url, Enums.HttpMethod httpMethod, string accessCode, string contentType, string accept, string formValue)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = httpMethod.ToString();
            httpRequest.Headers.Add(string.Format("Authorization: Bearer {0}", accessCode));
            httpRequest.ContentType = contentType;
            httpRequest.Accept = accept;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] byte1 = encoding.GetBytes(formValue);
            httpRequest.ContentLength = byte1.Length;
            Stream newStream = httpRequest.GetRequestStream();
            newStream.Write(byte1, 0, byte1.Length);
            newStream.Dispose();
            newStream.Close();

            return httpRequest;
        }

        public static async Task<string> GetResponseBody(HttpWebRequest request)
        {
            WebResponse webResponse = null;
            string responseText;

            try
            {
                webResponse = await request.GetResponseAsync();
                using (StreamReader ResponseReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    responseText = await ResponseReader.ReadToEndAsync();
                }
            }
            catch (WebException e)
            {
                responseText = ((int)((HttpWebResponse)e.Response).StatusCode).ToString();
                throw;
            }
            finally
            {
                webResponse?.Close();
            }

            return responseText;
        }
    }
}
