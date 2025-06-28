using System.Globalization;

namespace MauiQuestOrientation
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var lang = Preferences.Get("Language", "ru");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);

            MainPage = new AppShell();
        }
    }
}
