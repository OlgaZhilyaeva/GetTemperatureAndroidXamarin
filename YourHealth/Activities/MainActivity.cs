using System;
using System.Net;
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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Toast.MakeText(this, "Hello", ToastLength.Long).Show();

            var btnLogin = FindViewById<Button>(Resource.Id.login);

            btnLogin.Click += (object sender, EventArgs e) =>
            {
                var name = FindViewById<EditText>(Resource.Id.name).Text;
                var password = FindViewById<EditText>(Resource.Id.password).Text;

                HospitalUser user = new HospitalUser()
                {
                    Login = name,
                    Password = password
                };

                var userJson = JsonConvert.SerializeObject(user);

                var messageError = "incorrect password or login";
                var response = RestHttpClient.I.PostRequestRaw("http://hlp-hospital-api.azurewebsites.net/api/users", userJson);

                if (response.HttpResponseMessage.IsSuccessStatusCode)
                {
                    Intent nextActivity = new Intent(this, typeof(GeneratorActivity));
                    nextActivity.PutExtra("name", name);
                    StartActivity(nextActivity);
                }
                else
                {
                    Logger.LogError(this,
                        response.HttpResponseMessage.StatusCode == HttpStatusCode.Forbidden
                            ? messageError
                            : "unexpected server error occured");
                }
            };
        }


    }
}

