using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization.DateTimeFormatting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UwpApp
{
    public sealed partial class MainPage : Page
    {
        private MicrosoftTranslation.TranslatorService translatorService;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            translatorService = new MicrosoftTranslation.TranslatorService(ServiceKeys.TranslatorSubscriptionKey);

            var languages = await translatorService.GetLanguagesAsync();

            var dictionary = new Dictionary<string, string>();
            languages.ToList().ForEach(l =>
            {
                try
                {
                    var culture = new CultureInfo(l);
                    if (!culture.EnglishName.StartsWith("Unknown"))
                        dictionary.Add(l, culture.EnglishName);
                }
                catch { }
            });

            targetLanguage.ItemsSource = dictionary.OrderBy(d => d.Key);

            base.OnNavigatedTo(e);
        }

        private async void translateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(sentence.Text) || targetLanguage.SelectedValue == null)
                return;

            translation.Text = string.Empty;

            var translatedText = await translatorService.TranslateAsync(sentence.Text, targetLanguage.SelectedValue.ToString());
            translation.Text = translatedText;
        }
    }
}
