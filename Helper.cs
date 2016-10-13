using System;
using System.Text;
using Java.IO;
using Java.Net;

namespace MostrarTempo
{
    public class Helper
    {
        static string stream = null;

        public Helper() { }

        public string GetHTTPData(string urlString)
        {
            try
            {
                URL url = new URL(urlString);
                using (var urlConnection = (HttpURLConnection)url.OpenConnection())
                {
                    BufferedReader r = new BufferedReader(new InputStreamReader(urlConnection.InputStream));
                    StringBuilder sb = new StringBuilder();
                    String line;
                    while ((line = r.ReadLine()) != null)
                        sb.Append(line);
                    stream = sb.ToString();
                    urlConnection.Disconnect();

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
            return stream;
        }
    }
}