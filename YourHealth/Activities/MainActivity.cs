using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using YourHealth.Models;

namespace YourHealth.Activities
{
    [Activity(Label = "Authentification", MainLauncher = true)]
    public class MainActivity : Activity
    {
        GeneratorActivity generator = new GeneratorActivity();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Toast.MakeText(this, "Hello", ToastLength.Long).Show();

            var button = FindViewById<Button>(Resource.Id.login);
            var name = FindViewById<EditText>(Resource.Id.name).Text;
            var password = FindViewById<EditText>(Resource.Id.password).Text;

            button.Click += (object sender, EventArgs e) =>
            {
                HospitalUser User = new HospitalUser()
                {
                    Login = name,
                    Password = password
                };

                var userJson = JsonConvert.SerializeObject(User);

                var messageError = "Incorrect password or login";
                var userResult = generator.PostRequest("http://hlp-hospital-api.azurewebsites.net/api/users", userJson, messageError);

                if (userResult)
                {
                    Intent nextActivity = new Intent(this, typeof(GeneratorActivity));
                    nextActivity.PutExtra("name", name);
                    StartActivity(nextActivity);
                }
            };
        }


    }
}

