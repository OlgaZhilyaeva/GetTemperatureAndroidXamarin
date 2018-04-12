using System;
using System.Net;
using System.Net.Http;
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
    class GeneratorActivity : Activity
    {
        public TextView UserName { get; set; }
        public TextView TextChoice { get; set; }
        public TextView TextResult { get; set; }
        public Button GetDataBtn { get; set; }
        public Button GenerateBtn { get; set; }
        public SeekBar SeekBar { get; set; }
        public ToggleButton ToggleButton { get; set; }
        private bool _state = false;
        private int _z = 0;

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

            int value = 0;
            SeekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    value = e.Progress;
                    TextChoice.Text = string.Format("The value is {0} ms", value);
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
                    Random rNum = new Random();
                    while (_state)
                    {
                        _z++;
                        double res = rNum.Next(360, 400) / 10.0;
                        DateTime time = DateTime.UtcNow;
                        TextResult.Post(() => { TextResult.Text = TextResult.Text.Insert(0, $"Value: {res:F}, Time: {time} \n"); });

                        string json = GetJson();
                        var result = PostRequest("https://hlp-hospital-api.azurewebsites.net/api/temperatures",json);
                        

                        Thread.Sleep(500);
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
            Temperature t = new Temperature()
            {
                Value = 36,
                DateTime = DateTime.UtcNow
            };

            return JsonConvert.SerializeObject(t);
        }

        async Task PostRequest(string URL, string json)
        {
            var myHttpClient = new HttpClient();
            var response = await myHttpClient.PostAsync(URL, new StringContent(json));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Toast.MakeText(this, "Your data doesn`t reach the Azure DB", ToastLength.Long).Show();
            }
        }

    }
}