using System;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using Newtonsoft.Json;
using Square.Picasso;
using System.Threading.Tasks;
using Android.Util;
using Java.Security;

namespace MostrarTempo
{
    [Activity(Label = "Mostrar Tempo", MainLauncher = true, Icon = "@drawable/Icon")]
    public class MainActivity : Activity, ILocationListener
    {
        TextView txtCity, txtLastUpdate, txtDescription, txtHumidaty, txtTime, txtCelsius, txtlatitude,txtlongitude, lblatualiza;

        double temperatura;

        System.Globalization.CultureInfo cult = new System.Globalization.CultureInfo("en-US");

        ImageView imgView;
        LocationManager locationManager;
        string provider;
        string latt;
        string longg;
        static double lat, lng;
        OpenWeatherMap openWeatherMap = new OpenWeatherMap();
        private ProgressDialog pd1 = new ProgressDialog(Application.Context);

        public string tag { get; private set; }

       
       
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
             SetContentView (Resource.Layout.Main);

         
            Criteria locationCriteria = new Criteria();
            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Low;

            locationManager = (LocationManager)GetSystemService(Context.LocationService);
            provider = locationManager.GetBestProvider(locationCriteria, false);

           
            
            Location location = locationManager.GetLastKnownLocation(provider);
            //if (location == null)
            //{
            //    System.Diagnostics.Debug.WriteLine("Sem Sinal GPS");

            //    pd1.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
            //    pd1.SetTitle("Sem Sinal GPS...");
            //    pd1.Show();


            //}
            //else
            //{


            //    lat = location.Latitude;
            //    lng = location.Longitude;

            //    latt  = (Convert.ToString(location.Latitude).Replace(",", "."));
            //    longg = (Convert.ToString(location.Longitude).Replace(",", "."));



            //    new GetWeather(this, openWeatherMap).Execute(Common.APIRequest(lat.ToString(), lng.ToString()));

            new GetWeather(this, openWeatherMap).Execute(Common.APIRequest(latt, longg));
            //}



        }
       

        protected override void OnResume()
        {
            base.OnResume();
           
            if (locationManager.IsProviderEnabled(provider))
            {
                locationManager.RequestLocationUpdates(provider, 400, 1f, this);
            }
        }
        

        protected override void OnPause()
        {
            base.OnPause();
           
            locationManager.RemoveUpdates(this);
        }



        public void OnLocationChanged(Location location)
        {

            Criteria locationCriteria = new Criteria();
            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Low;

            locationManager = (LocationManager)GetSystemService(Context.LocationService);
            provider = locationManager.GetBestProvider(locationCriteria, false);



            Location locations = locationManager.GetLastKnownLocation(provider);
            if (locations == null)
            {
                System.Diagnostics.Debug.WriteLine("Sem Sinal GPS");

                pd1.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
                pd1.SetTitle("Sem Sinal GPS...");
                pd1.Show();


            }
            else
            {


                lat = locations.Latitude;
                lng = locations.Longitude;

                latt = (Convert.ToString(locations.Latitude).Replace(",", "."));
                longg = (Convert.ToString(locations.Longitude).Replace(",", "."));



               //  new GetWeather(this, openWeatherMap).Execute(Common.APIRequest(lat.ToString(), lng.ToString()));

                new GetWeather(this, openWeatherMap).Execute(Common.APIRequest(latt, longg));
            }

            //lat = location.Latitude;
            //lng = location.Longitude;

            //latt = (Convert.ToString(location.Latitude).Replace(",", "."));
            //longg = (Convert.ToString(location.Longitude).Replace(",", "."));


            //new GetWeather(this, openWeatherMap).Execute(Common.APIRequest(lat.ToString(), lng.ToString()));


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
            OpenWeatherMap openWeatherMap;


            public GetWeather(MainActivity activity, OpenWeatherMap openWeatherMap)
            {
                this.activity = activity;
                this.openWeatherMap = openWeatherMap;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                pd.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
                pd.SetTitle("Espere um instante...");
                pd.Show();
            }
            protected override string RunInBackground(params string[] @params)
            {
                string stream = null;
                string urlString = @params[0];

                Helper http = new Helper();

                stream = http.GetHTTPData(urlString);
                //Common.APIRequest(lat.ToString(), lng.ToString())
                // dynamic results = http.getDataFromService(urlString).ConfigureAwait(false);                          


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

                    //Control
                    activity.txtCity = activity.FindViewById<TextView>(Resource.Id.txtCity);
                    activity.lblatualiza = activity.FindViewById<TextView>(Resource.Id.lblatualiza);
                    activity.txtLastUpdate = activity.FindViewById<TextView>(Resource.Id.txtLastUpdate);
                    activity.txtDescription = activity.FindViewById<TextView>(Resource.Id.txtDescription);
                    activity.txtHumidaty = activity.FindViewById<TextView>(Resource.Id.txtHumidity);
                    activity.txtTime = activity.FindViewById<TextView>(Resource.Id.txtTime);
                    activity.txtCelsius = activity.FindViewById<TextView>(Resource.Id.txtCelsius);

                    activity.txtlatitude = activity.FindViewById<TextView>(Resource.Id.latitude);
                    activity.txtlongitude = activity.FindViewById<TextView>(Resource.Id.longitude);



                    activity.imgView = activity.FindViewById<ImageView>(Resource.Id.imageView);


                    //Add data

                   
                    activity.txtCity.Text = $"{openWeatherMap.name},{openWeatherMap.sys.country}";
                    activity.lblatualiza.Text = $"Última Atualização";
                    activity.txtLastUpdate.Text = $"{DateTime.Now.ToString("dd MMMM yyyy HH:mm")}";
                    activity.txtDescription.Text = $"{openWeatherMap.weather[0].description}";
                    activity.txtHumidaty.Text = $"Humidade: {openWeatherMap.main.humidity} %";
                    activity.txtTime.Text =
                        $"{Common.UnixTimeStampToDateTime(openWeatherMap.sys.sunrise).ToString("HH:mm")}/{Common.UnixTimeStampToDateTime(openWeatherMap.sys.sunset).ToString("HH:mm")}";


                    activity.txtCelsius.Text = $"{(openWeatherMap.main.temp) - 273.15} °C";

                    activity.txtlatitude.Text = $"Latitude : {(lat.ToString().Replace(",", "."))}";

                    activity.txtlongitude.Text = $"Logitude : {(lng.ToString().Replace(",", "."))}";


                    if (!String.IsNullOrEmpty(openWeatherMap.weather[0].icon))
                    {
                        Picasso.With(activity.ApplicationContext)
                            .Load(Common.GetImage(openWeatherMap.weather[0].icon))
                            .Into(activity.imgView);
                    }
                }
            }
        }
    }
}

