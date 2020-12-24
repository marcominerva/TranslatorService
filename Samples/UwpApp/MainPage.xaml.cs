using System;
using TranslatorService;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UwpApp
{
    public sealed partial class MainPage : Page
    {
        private TranslatorClient translatorClient;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            translatorClient = new TranslatorClient(ServiceKeys.TranslatorSubscriptionKey, ServiceKeys.TranslatorRegion);

            try
            {
                var languages = await translatorClient.GetLanguagesAsync();
                TargetLanguage.ItemsSource = languages;
                TargetLanguage.SelectedIndex = 0;
            }
            catch (TranslatorServiceException ex)
            {
                var messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
            }

            base.OnNavigatedTo(e);
        }

        private async void TranslateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Sentence.Text) || TargetLanguage.SelectedValue == null)
            {
                return;
            }

            DetectedLanguage.Text = string.Empty;
            TranslationResult.Text = string.Empty;

            try
            {
                var translationResult = await translatorClient.TranslateAsync(Sentence.Text, to: TargetLanguage.SelectedValue.ToString());
                DetectedLanguage.Text = $"Detected source language: {translationResult.DetectedLanguage.Language} ({translationResult.DetectedLanguage.Score:P2})";
                TranslationResult.Text = translationResult.Translation.Text;
            }
            catch (TranslatorServiceException ex)
            {
                var messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            translatorClient.Dispose();
            base.OnNavigatingFrom(e);
        }
    }
}
