using OpenWeatherMapAPI.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OpenWeatherMapAPI.Service
{
    public class WeatherDataService
    {

        private const string apiKey = "5862724786fd66d515a841189e06efa9";
        private const string url = "https://api.openweathermap.org/data/2.5/weather?";

        public static Task<WeatherData> GetWeatherData(string city)
        {
            return Task<WeatherData>.Factory.StartNew(() =>
            {


                string apiOutput = "";
                WebRequest request = WebRequest.Create($"{url}q={city}&appid={apiKey}&units=metric&mode=xml");
                request.Timeout = 5000;

                try
                {
                    WebResponse response = request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        apiOutput = reader.ReadToEnd();
                    }
                }
                catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
                {
                    throw new TimeoutException("Request took too long");
                }
                catch (WebException ex)
                {
                    throw new HttpListenerException(ex.HResult, "Request failet");
                }

                XmlDocument xml = new XmlDocument();
                try
                {
                    xml.LoadXml(apiOutput);
                }
                catch
                {
                    throw new FormatException("API returned data in invalid format");
                }

                XmlNode temperaturNode = xml.SelectSingleNode("current").SelectSingleNode("temperature");
                string imageUrl = "http://openweathermap.org/img/wn/";

                return new WeatherData()
                {
                    Temperature = xml.SelectSingleNode("current").SelectSingleNode("temperature").Attributes.GetNamedItem("value").Value,
                    City = city,
                    Icon = $"{imageUrl}{xml.SelectSingleNode("current").SelectSingleNode("weather").Attributes.GetNamedItem("icon").Value}@2x.png"
                };
            });
        }
    }
}
