using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApiX
{
    public partial class App : Application
    {
        public static HttpClient RestClient { get; private set; }
        public static string BaseUrl { get; private set; }

        public App()
        {
            InitializeComponent();
            App.RestClient = new HttpClient(new Dictionary<string, string>
            {
                { "LLAVE", "123apirest"}
            });


            App.BaseUrl = "http://192.168.1.42/ApiX";
             MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
           var listusers = await new UserRequest(App.RestClient).All();
            MainPage = new NavigationPage(new UserPage(listusers)); 
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
