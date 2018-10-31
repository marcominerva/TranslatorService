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
        private TranslatorService.TranslatorClient translatorClient;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            translatorClient = new TranslatorService.TranslatorClient(ServiceKeys.TranslatorSubscriptionKey);

            var languages = await translatorClient.GetLanguagesAsync();
            TargetLanguage.ItemsSource = languages;
            TargetLanguage.SelectedIndex = 0;

            base.OnNavigatedTo(e);
        }

        private async void TranslateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Sentence.Text) || TargetLanguage.SelectedValue == null)
            {
                return;
            }

            DetectedLanguage.Text = string.Empty;
            Translation.Text = string.Empty;

            var translationResult = await translatorClient.TranslateAsync(Sentence.Text, to: TargetLanguage.SelectedValue.ToString());
            DetectedLanguage.Text = $"Detected source language: {translationResult.DetectedLanguage.Language} ({translationResult.DetectedLanguage.Score:P2})";
            Translation.Text = translationResult.Translation.Text;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            translatorClient.Dispose();
            base.OnNavigatingFrom(e);
        }
    }
}
