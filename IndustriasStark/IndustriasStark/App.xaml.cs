using IndustriasStark.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IndustriasStark
{
    public partial class App : Application
    {
        public static String DbName;
        public static String DbPath;
        public App()
        {
            InitializeComponent();

            MainPage = new PagePrincipal();
        }

        public App(string dbPath, string dbName)
        {
            InitializeComponent();
            Device.SetFlags(new[] {"Brush_Experimental"});
            App.DbName = dbName;
            App.DbPath = dbPath;
            MainPage = new PagePrincipal();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
