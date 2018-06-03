using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using YourHealth.Models;

namespace YourHealth.Activities
{
    [Activity(Label = "Generator")]
    public class GeneratorActivity : Activity
    {
        public TextView UserName { get; set; }
        public TextView TextChoice { get; set; }
        public TextView TextResult { get; set; }
        public Button GetDataBtn { get; set; }
        public Button GenerateBtn { get; set; }
        public SeekBar SeekBar { get; set; }
        public ToggleButton ToggleButton { get; set; }
        private bool _state = false;
        private int _value = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Generator);

            UserName = FindViewById<TextView>(Resource.Id.userName);
            string name = Intent.GetStringExtra("name");
            UserName.Text = "Hello " + name;

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

            SeekBar = FindViewById<SeekBar>(Resource.Id.seekBar);
            TextChoice = FindViewById<TextView>(Resource.Id.textChoice);

            SeekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    _value = e.Progress;
                    TextChoice.Text = string.Format("The value is {0} s", _value);
                }
            };
            //switchOn
            ToggleButton = FindViewById<ToggleButton>(Resource.Id.toogleBut);
            TextResult = FindViewById<TextView>(Resource.Id.textResult);

            ToggleButton.CheckedChange += ToggleButtonOnCheckedChange;
        }

        private void ToggleButtonOnCheckedChange(object o, CompoundButton.CheckedChangeEventArgs checkedChangeEventArgs)
        {
            if (checkedChangeEventArgs.IsChecked)
            {
                _state = true;
                Task.Run(() =>
                {
                    while (_state)
                    {
                        string json = GetJson();

                        var response = RestHttpClient.I.PostRequestRaw("https://hlp-hospital-api.azurewebsites.net/api/temperatures",
                            json);
                        if (!response.HttpResponseMessage.IsSuccessStatusCode)
                        {
                            Logger.LogError(this, "Your data doesn`t reach the Azure DB");
                        }

                        Thread.Sleep(_value * 1000);
                    }
                });
            }
            else
            {
                _state = false;
            }
        }

        private string GetJson()
        {
            Random rNum = new Random();
            decimal res = (rNum.Next(360, 400) / 10.0m);

            Temperature t = new Temperature()
            {
                Value = res,
                DateTime = DateTime.UtcNow
            };

            TextResult.Post(() =>
            {
                TextResult.Text = TextResult.Text.Insert(0, $"Value: {t.Value}, Time: {t.DateTime} \n");
            });

            return JsonConvert.SerializeObject(t);
        }

        public bool PostRequest(string url, string json, string messageError)
        {
            var myHttpClient = new HttpClient();
            var response = myHttpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Toast.MakeText(this, messageError, ToastLength.Long).Show();
                return false;
            }

            var content = response.Content.ReadAsStringAsync().Result;

            if (content != "true")
            {
                return false;
            }

            return true;
        }
    }
}