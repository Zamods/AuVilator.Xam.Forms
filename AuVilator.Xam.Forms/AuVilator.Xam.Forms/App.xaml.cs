using AuVilator.Xam.Forms.Page;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AuVilator.Xam.Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            string culture = Preferences.Get("lang", "en");
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
            MainPage = new SilencerPage();
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