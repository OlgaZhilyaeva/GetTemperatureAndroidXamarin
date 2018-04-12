using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;


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

            var button = FindViewById<Button>(Resource.Id.login);
            var name = FindViewById<EditText>(Resource.Id.name);

            Toast.MakeText(this,"hello",ToastLength.Long).Show();

            button.Click += (object sender, EventArgs e) =>
            {
                //Buttonclick(sender, e);
                Intent nextActivity = new Intent(this, typeof(GeneratorActivity));
                nextActivity.PutExtra("name", name.Text);
                StartActivity(nextActivity);
            };
        }

        private void Buttonclick(object sender, EventArgs e)
        {
            var name = FindViewById<EditText>(Resource.Id.name).Text;
            var password = FindViewById<EditText>(Resource.Id.password).Text;
            Toast.MakeText(this, password + " + " + name, ToastLength.Short).Show();
        }
    }
}

