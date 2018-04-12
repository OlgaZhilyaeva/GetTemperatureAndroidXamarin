using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using YourHealth.Models;

namespace YourHealth.Activities
{
    [Activity(Label = "Get My Data")]
    class GetDataActivity:Activity
    {
        public Button GetDataBtn { get; set; }
        public Button GenerateBtn { get; set; }
        public Button GetMyDataBtn { get; set; }
        public Spinner Spinner { get; set; }
        public DateTime Time { get; set; }
        public DateTime TimeLowBorder { get; set; }
        public ListView DataListView { get; set; }
        List<Temperature> _temperatures = new List<Temperature>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.GetData);

            GetDataBtn = FindViewById<Button>(Resource.Id.ButGetData);
            GetDataBtn.Click += (object sender, EventArgs e) =>
            {
                Intent nextActivity = new Intent(this, typeof(GetDataActivity));
                StartActivity(nextActivity);
            };
            GenerateBtn = FindViewById<Button>(Resource.Id.ButGenerator);
            GenerateBtn.Click += (object sender, EventArgs e) =>
            {
                Intent nextActivity = new Intent(this, typeof(GeneratorActivity));
                StartActivity(nextActivity);
            };

            //Spinner start
            Spinner = FindViewById<Spinner>(Resource.Id.Spinner);
            Spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                this, Resource.Array.dropDownArray, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            Spinner.Adapter = adapter;
            //Spiner finish

            DataListView = FindViewById<ListView>(Resource.Id.MyDataListView);
            GetMyDataBtn = FindViewById<Button>(Resource.Id.GetMyDataBtn);
            GetMyDataBtn.Click += ButtonOnClick;
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            var result = PostRequest("https://hlp-hospital-api.azurewebsites.net/api/temperatures").Result;
            _temperatures.Clear();
            _temperatures.AddRange(GetAllTemperatures(result));
            DataListView.Adapter = new ArrayAdapter<Temperature>(this, Android.Resource.Layout.SimpleListItem1, _temperatures);

        }

        private IEnumerable<Temperature> GetAllTemperatures(string result)
        {
            List<Temperature> temp = JsonConvert.DeserializeObject<List<Temperature>>(result);
            List<Temperature> temperatures = temp.Where(x => x.DateTime >= TimeLowBorder && x.DateTime <= Time).ToList();
            return temperatures;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //Spinner spinner = (Spinner)sender;
            //string toast = string.Format("Your choice is {0}", spinner.GetItemAtPosition(e.Position));

            Time = DateTime.UtcNow;
            TimeLowBorder = GetDateLimits(e.Position,Time);
            Toast.MakeText(this, TimeLowBorder.ToString(), ToastLength.Long).Show();
        }

        private DateTime GetDateLimits(int ePosition,DateTime time)
        {
            DateTime variableTime = new DateTime();
            switch (ePosition)
            {
                case 0:
                    variableTime = time.AddHours(-1);
                    break;
                case 1:
                    variableTime = time.AddHours(-3);
                    break;
                case 2:
                    variableTime = time.AddHours(-6);
                    break;
                case 3:
                    variableTime = time.AddHours(-9);
                    break;
                case 4:
                    variableTime = time.AddHours(-24);
                    break;
                case 5:
                    variableTime = time.AddHours(-72);
                    break;
                case 6:
                    variableTime = time.AddHours(-168);
                    break;
                case 7:
                    variableTime = time.AddHours(-336);
                    break;
            }
            return variableTime;
        }

        async Task<string> PostRequest(string URL)
        {
            var myHttpClient = new HttpClient();
            var response = await myHttpClient.GetAsync(URL);
            //var response1 = await myHttpClient.PostAsync(URL, new StringContent("JSON IS HERE"));

            var json = await response.Content.ReadAsStringAsync();
            return json;
        }

    }
}