using System;
using System.Net.Http;
using System.Text;
using Java.IO;
using System.Threading.Tasks;
using Java.Net;
using Newtonsoft.Json;
using System.IO;

namespace MostrarTempo
{
    public class Helper
    {
        static string stream = null;

        public Helper() { }

        //public string GetHTTPData(string urlString)
        //{
        //    try
        //    {
        //        URL url = new URL(urlString);
        //        using (var urlConnection = (HttpURLConnection)url.OpenConnection())
        //        {
        //            BufferedReader r = new BufferedReader(new InputStreamReader(urlConnection.InputStream));
        //            StringBuilder sb = new StringBuilder();
        //            String line;
        //            while ((line = r.ReadLine()) != null)
        //                sb.Append(line);
        //            stream = sb.ToString();
        //            urlConnection.Disconnect();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message);

        //    }
        //    return stream;
        //}

        public async Task<dynamic> getDataFromService(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }

            return data;
        }

        //public static async Task<dynamic> getDataFromService(string queryString)
        //{
        //    HttpClient client = new HttpClient();
        //    var response = await client.GetAsync(queryString);

        //    dynamic data = null;
        //    if (response != null)
        //    {
        //        string json = response.Content.ReadAsStringAsync().Result;
        //        data = JsonConvert.DeserializeObject(json);
        //    }

        //    return data;
        //}
    }
}