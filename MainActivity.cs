using System;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using Newtonsoft.Json;
using Square.Picasso;

namespace MostrarTempo
{
    [Activity(Label = "Mostrar Tempo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        TextView txtCity, txtLastUpdate, txtDescription, txtHumidaty, txtTime, txtCelsius;

        private ImageView imgView;
        private LocationManager locationManager;
        private string provider;
        private static double lat;
        private OpenWeatherMap openWeatherMap = new OpenWeatherMap();

        static double lng;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
            locationManager = (LocationManager)GetSystemService(Context.LocationService);
            provider = locationManager.GetBestProvider(new Criteria(), false);

            Location location = locationManager.GetLastKnownLocation(provider);
            if (location == null)
                System.Diagnostics.Debug.WriteLine("No location");

    
        }
        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(provider, 400, 1, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }



        public void OnLocationChanged(Location location)
        {
            lat = Math.Round(location.Latitude, 4);
            lng = Math.Round(location.Longitude, 4);

            new GetWeather(this, openWeatherMap).Execute(Common.APIRequest(lat.ToString(), lng.ToString()));
        }

        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {

        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

        }

        private class GetWeather : AsyncTask<string, Java.Lang.Void, string>
        {
            private ProgressDialog pd = new ProgressDialog(Application.Context);
            private MainActivity activity;
            private OpenWeatherMap openWeatherMap;


            public GetWeather(MainActivity activity, OpenWeatherMap openWeatherMap)
            {
                this.activity = activity;
                this.openWeatherMap = openWeatherMap;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                pd.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
                pd.SetTitle("Please wait...");
                pd.Show();
            }
            protected override string RunInBackground(params string[] @params)
            {
                string stream = null;
                string urlString = @params[0];

                Helper http = new Helper();

                //urlString =  Common.Common.APIRequest(lat.ToString(), lng.ToString()
                stream = http.GetHTTPData(urlString);
                return stream;

            }

            protected override void OnPostExecute(string result)
            {
                base.OnPostExecute(result);
                if (result.Contains("Error: Not found city"))
                {
                    pd.Dismiss();
                    return;
                }
                openWeatherMap = JsonConvert.DeserializeObject<OpenWeatherMap>(result);
                pd.Dismiss();


                if (openWeatherMap != null)
                {
                    
                }

                //Control
                activity.txtCity = activity.FindViewById<TextView>(Resource.Id.txtCity);
                activity.txtLastUpdate = activity.FindViewById<TextView>(Resource.Id.txtLastUpdate);
                activity.txtDescription = activity.FindViewById<TextView>(Resource.Id.txtDescription);
                activity.txtHumidaty = activity.FindViewById<TextView>(Resource.Id.txtHumidity);
                activity.txtTime = activity.FindViewById<TextView>(Resource.Id.txtTime);
                activity.txtCelsius = activity.FindViewById<TextView>(Resource.Id.txtCelsius);

                activity.imgView = activity.FindViewById<ImageView>(Resource.Id.imageView);


                //Add data
                activity.txtCity.Text = $"{openWeatherMap.name},{openWeatherMap.sys.country}";
                activity.txtLastUpdate.Text = $"Last Update: {DateTime.Now.ToString("dd MMMM yyyy HH:mm")}";
                activity.txtDescription.Text = $"{openWeatherMap.weathers[0].description}";
                activity.txtHumidaty.Text = $"Humidity: {openWeatherMap.main.humidity} %";
                activity.txtTime.Text = $"{Common.UnixTimeStampToDateTime(openWeatherMap.sys.sunrise).ToString("HH:mm")}/{Common.UnixTimeStampToDateTime(openWeatherMap.sys.sunrise).ToString("HH:mm")}";

                activity.txtCelsius.Text = $"{openWeatherMap.main.temp} °C";

               // openWeatherMap.coord.


                if (!String.IsNullOrEmpty(openWeatherMap.weathers[0].icon))
                {
                    Picasso.With(activity.ApplicationContext)
                        .Load(Common.GetImage(openWeatherMap.weathers[0].icon))
                        .Into(activity.imgView);
                }
            }
        }
    }
}

