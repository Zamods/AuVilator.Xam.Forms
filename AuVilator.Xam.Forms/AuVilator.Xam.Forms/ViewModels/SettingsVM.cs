using AuVilator.Xam.Forms.Models;
using AuVilator.Xam.Forms.Page;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace AuVilator.Xam.Forms.ViewModels
{
    public class SettingsVM : BaseVM
    {
        private SettingsM _settingsM;
        public Languages CurrentLanguage { get => _settingsM.currentLanguage; set => SetPropertyAndRaise(ref _settingsM.currentLanguage, value, "CurrentLanguage"); }
        public IList<string> LanguageOptions
        {
            get => GetLanguageNames();
        }

        public SettingsVM()
        {
            _settingsM = new SettingsM();
            ChangeLanguage = new RelayCommand<int>(l => SetLanguage(l));

            ExitSettings = new RelayCommand(() =>
            {
                App.Current.MainPage = new SilencerPage();
            });
        }

        private IList<string> GetLanguageNames()
        {
            return new List<string>()
            {
                Enum.GetName(typeof(Languages), Languages.English),
                "русский"
            };
        }

        public Task SetLanguage(int selectedIndex)
        {
            CultureInfo cultureToAdopt;
            Languages selectedLanguage = (Languages)Enum.ToObject(typeof(Languages), selectedIndex);
            switch (selectedLanguage)
            {
                case Languages.English:
                    cultureToAdopt = CultureInfo.CreateSpecificCulture("en");
                    SetUICulture(cultureToAdopt);
                    break;

                case Languages.Russian:
                    cultureToAdopt = CultureInfo.CreateSpecificCulture("ru-RU");
                    SetUICulture(cultureToAdopt);
                    break;
            }
            App.Current.MainPage = new SettingsPage();
            return Task.CompletedTask;
        }

        private Task SetUICulture(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Preferences.Set("lang", cultureInfo.TwoLetterISOLanguageName);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Exposed command to initiate [Task] SetLanguage() from view
        /// </summary>
        public ICommand ChangeLanguage { get; private set; }

        /// <summary>
        /// Exposed command to navigate back to main page
        /// </summary>
        public ICommand ExitSettings { get; private set; }
    }
}