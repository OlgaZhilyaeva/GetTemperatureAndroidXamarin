using Android.Content;
using Android.Widget;

namespace YourHealth.Models
{
    public class Logger
    {
        public static void LogError(Context context, string message, bool isShort = true)
        {
            Toast.MakeText(context, $"Error: {message}", isShort ? ToastLength.Short : ToastLength.Long).Show();
        }
    }
}