namespace MauiQuestOrientation;

using MauiQuestOrientation.Resources.Strings;
using System.Globalization;
using System.Threading;

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
        var selectedLangCode = selectedIndex >= 0 ? _languageCodes[selectedIndex] : "en";
        Preferences.Set("Language", selectedLangCode);

        Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLangCode);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(selectedLangCode);

        // Принудительно обновить культуру ресурсов
        AppResources.Culture = new CultureInfo(selectedLangCode);

        // Для отладки
        System.Diagnostics.Debug.WriteLine($"AppResources.Game: {AppResources.Game}");
        System.Diagnostics.Debug.WriteLine($"AppResources.Culture: {AppResources.Culture}");
        System.Diagnostics.Debug.WriteLine($"CurrentUICulture: {Thread.CurrentThread.CurrentUICulture}");

        DisplayAlert(AppResources.Info, AppResources.LanguageSaved, AppResources.OK);
        Application.Current.MainPage = new AppShell();
    }

}