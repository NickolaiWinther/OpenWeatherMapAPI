using OpenWeatherMapAPI.Entities;
using OpenWeatherMapAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenWeatherMapAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchButton.IsEnabled = false;
            Task<WeatherData> task;

            try
            {
                task = WeatherDataService.GetWeatherData(InputTextBox.Text);
                task.Wait();
            }
            catch (AggregateException ex) when (ex.InnerException is TimeoutException)
            {
                MessageBox.Show("Timeout. Svar ikke opnået på 5 sekunder");
                return;
            }
            catch (AggregateException ex) when(ex.InnerException is HttpListenerException)
            {
                MessageBox.Show($"Kunne ikke finde byen {InputTextBox.Text}");
                return;
            }
            finally
            {
                SearchButton.IsEnabled = true;
            }

            WeatherData data = task.Result;
            TemperatureTextBlock.Text = $"{data.Temperature} °C";
            WeatherImage.Source = new BitmapImage(new Uri(data.Icon));
        }
    }
}
