using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMapAPI.Entities
{
    public class WeatherData
    {
        public WeatherData()
        {

        }

        public string Temperature { get; set; }
        public string City { get; set; }
        public string Icon { get; set; }
    }
}
