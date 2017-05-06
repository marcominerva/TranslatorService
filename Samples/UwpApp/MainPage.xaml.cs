using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        private TranslatorService.TranslatorServiceClient translatorService;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            translatorService = new TranslatorService.TranslatorServiceClient(ServiceKeys.TranslatorSubscriptionKey);

            var languages = await translatorService.GetLanguageNamesAsync();
            targetLanguage.ItemsSource = languages.OrderBy(lang => lang.Name).ToList();
            targetLanguage.SelectedIndex = 0;

            base.OnNavigatedTo(e);
        }

        private async void translateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(sentence.Text) || targetLanguage.SelectedValue == null)
            { 
                return;
            }

            translation.Text = string.Empty;

            var translatedText = await translatorService.TranslateAsync(sentence.Text, targetLanguage.SelectedValue.ToString());
            translation.Text = translatedText;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            translatorService.Dispose();
            base.OnNavigatingFrom(e);
        }
    }
}
