using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AzureSentimentAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            //accessKey retrieved from cognetive service created in azure.
            string accessKey = "c177184498ae4e24bf1e4d5c54cfc5e5";
            string uri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
            string json = "{\r\n        \"documents\": [\r\n            {\r\n                \"language\": \"en\",\r\n                \"id\": \"1\",\r\n                \"text\": \"We love this trail and make the trip every year. The views are breathtaking and well worth the hike!\"\r\n            },\r\n            {\r\n                \"language\": \"en\",\r\n                \"id\": \"2\",\r\n                \"text\": \"Poorly marked trails! I thought we were goners. Worst hike ever.\"\r\n            },\r\n            {\r\n                \"language\": \"en\",\r\n                \"id\": \"3\",\r\n                \"text\": \"Everyone in my family liked the trail but thought it was too challenging for the less athletic among us. Not necessarily recommended for small children.\"\r\n            },\r\n            {\r\n                \"language\": \"en\",\r\n                \"id\": \"4\",\r\n                \"text\": \"It was foggy so we missed the spectacular views, but the trail was ok. Worth checking out if you are in the area.\"\r\n            },                \r\n            {\r\n                \"language\": \"en\",\r\n                \"id\": \"5\",\r\n                \"text\": \"This is my favorite trail. It has beautiful views and many places to stop and rest\"\r\n            }\r\n        ]\r\n    }";
            Program program = new Program();
            string result = program.EvaluateResult(uri, json, accessKey);
            var jsonForm = JsonConvert.DeserializeObject(result);
            Console.WriteLine(jsonForm);
            Console.ReadKey();
        }

        private string EvaluateResult(string URI, string body, string key)
        {
            Uri uri = new Uri(string.Format(URI));

            // Create the request
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", key);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(body);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error setting up stream writer: " + ex.Message);
            }

            // Get the response
            HttpWebResponse httpResponse = null;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error from : " + uri + ": " + ex.Message);
                return null;
            }

            string result = null;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }
    }
}
