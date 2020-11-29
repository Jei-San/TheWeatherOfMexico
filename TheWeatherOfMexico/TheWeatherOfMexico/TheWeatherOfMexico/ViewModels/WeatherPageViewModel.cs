using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TheWeatherOfMexico.Models;
using Xamarin.Forms;

namespace TheWeatherOfMexico.ViewModels
{
    public class WeatherPageViewModel : INotifyPropertyChanged
    {
        private WeatherData data;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public WeatherData Data
        {
            get => data; 
            set
            {
                data = value;
                OnPropertyChanged();
            }
        }
        public ICommand SearchCommand { get; set; }


        public WeatherPageViewModel()
        {
            SearchCommand = new Command(async (searchTerm) =>
            {
                var entrada = searchTerm as string;
                var datos = entrada.Split(',');
                var lat = datos[0];
                var lon = datos[1];
                await GetData($"https://api.weatherbit.io/v2.0/current?lat={lat}&lon={lon}&key=7e27270258bb47e795e76510cdfd2483&lang=es");
            });
        }

        private async Task GetData(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<WeatherData>(jsonResult);
            Data = result;
        }

    }
}
