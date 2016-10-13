using System;
using System.Text;

namespace MostrarTempo
{
    class Common
    {
        public static string API_KEY = "02020f73a50a52af3f212f7fc093c339";
        public static string API_LINK = "api.openweathermap.org/data/2.5/weather"; //?lat={lat}&lon={lon}

        public static string APIRequest(string lat, string lng)
        {
            StringBuilder sb = new StringBuilder(API_LINK);
            sb.AppendFormat("?lat={0}&lon={1}&APPID={2}", lat, lng, API_KEY);
            return sb.ToString();


        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(unixTimeStamp).ToLocalTime();

            return dt;
        }

        public static string GetImage(string icon)
        {
            return $"http://openweathermap.org/img/w/{icon}.png";
        }

    }
}