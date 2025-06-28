namespace MauiQuestOrientation
{

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnStartGameClicked(object sender, EventArgs e)
        {
            await GameManager.Instance.StartNewGameAsync();
            await Shell.Current.GoToAsync(nameof(GamePage));
        }

        private async void OnContinueClicked(object sender, EventArgs e)
        {
            bool loaded = await GameManager.Instance.ContinueGameAsync();
            if (loaded)
            {
                await Shell.Current.GoToAsync(nameof(GamePage));
            }
            else
            {
                await DisplayAlert("Ошибка", "Нет сохранённой игры.", "OK");
            }
        }


        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}