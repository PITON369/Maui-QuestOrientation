namespace MauiQuestOrientation
{
    public partial class SettingsPage : ContentPage
    {
        private readonly string[] _languages = new[] { "Русский", "English" };
        private readonly string[] _languageCodes = new[] { "ru", "en" };

        public SettingsPage()
        {
            InitializeComponent();
            LanguagePicker.ItemsSource = _languages;
            var savedLang = Preferences.Get("Language", "ru");
            LanguagePicker.SelectedIndex = Array.IndexOf(_languageCodes, savedLang);
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            var selectedIndex = LanguagePicker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                Preferences.Set("Language", _languageCodes[selectedIndex]);
                DisplayAlert("Info", "Язык сохранён / Language saved", "OK");
            }
        }
    }
}