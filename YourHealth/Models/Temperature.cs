using System;

namespace YourHealth.Models
{
    public class Temperature
    {
        public int TemperatureId { get; set; }
        public decimal Value { get; set; }
        public DateTime DateTime { get; set; }

        public override string ToString()
        {
            var z = Value + DateTime.ToString();
            return z;
        }
    }
}