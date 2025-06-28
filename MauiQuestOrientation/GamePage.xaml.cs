namespace MauiQuestOrientation;

public partial class GamePage : ContentPage
{
    private Adventure _adventure => GameManager.Instance.State.Adventure;
    private string _language => Preferences.Get("Language", "ru");

    public GamePage()
    {
        InitializeComponent();

        // Проверка, что Adventure и Cutscenes загружены
        if (_adventure == null || _adventure.Cutscenes == null)
        {
            DisplayAlert("Ошибка", "Данные приключения не загружены.", "OK");
            UpdateUI();
            return;
        }

        if (!string.IsNullOrEmpty(GameManager.Instance.State.CurrentSceneId))
        {
            // Найти и показать стартовую кат-сцену
            var scene = _adventure.Cutscenes.FirstOrDefault(s => s.Id == GameManager.Instance.State.CurrentSceneId);
            if (scene != null)
                ShowCutscene(scene);
            else
                UpdateUI();
        }
        else
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        // Скрыть кат-сцену по умолчанию
        CutsceneFrame.IsVisible = false;
        CutsceneImage.IsVisible = false;
        CutsceneText.Text = string.Empty;
        ChoicesLayout.Children.Clear();

        // Обновить инвентарь
        InventoryLayout.Children.Clear();
        foreach (var itemId in GameManager.Instance.State.Inventory)
        {
            var item = _adventure.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                InventoryLayout.Children.Add(new Label { Text = item.Name.TryGetValue(_language, out var name) ? name : item.Id });
            }
        }
    }

    private void OnEnterCodeClicked(object sender, EventArgs e)
    {
        var code = CodeEntry.Text?.Trim();
        if (string.IsNullOrEmpty(code))
            return;

        // Для отладки: выведи все коды и введённый код
        var allCodes = string.Join(", ", _adventure.ControlPoints.Select(c => $"'{c.Code}'"));
        System.Diagnostics.Debug.WriteLine($"Все коды КП: {allCodes}");
        System.Diagnostics.Debug.WriteLine($"Введённый код: '{code}'");

        // Найти КП по коду
        var cp = _adventure.ControlPoints.FirstOrDefault(c => c.Code.Trim() == code.Trim());
        if (cp == null)
        {
            DisplayAlert("Ошибка", "Код не найден.", "OK");
            return;
        }

        // Найти кат-сцену
        var scene = _adventure.Cutscenes.FirstOrDefault(s => s.Id == cp.CutsceneId);
        if (scene == null)
        {
            DisplayAlert("Ошибка", "Кат-сцена не найдена.", "OK");
            return;
        }

        // Сохранить текущую сцену в состоянии
        GameManager.Instance.State.CurrentSceneId = scene.Id;
        if (!GameManager.Instance.State.CompletedScenes.Contains(scene.Id))
            GameManager.Instance.State.CompletedScenes.Add(scene.Id);

        // Показать кат-сцену
        ShowCutscene(scene);

        // Сохранить прогресс
        GameManager.Instance.SaveAsync();
    }

    private void ShowCutscene(Cutscene scene)
    {
        CutsceneFrame.IsVisible = true;
        CutsceneText.Text = scene.Text.TryGetValue(_language, out var text) ? text : scene.Id;

        // Показать картинку, если есть
        if (!string.IsNullOrEmpty(scene.Image))
        {
            CutsceneImage.Source = ImageSource.FromFile(scene.Image);
            CutsceneImage.IsVisible = true;
        }
        else
        {
            CutsceneImage.IsVisible = false;
        }

        // Показать варианты выбора
        ChoicesLayout.Children.Clear();
        if (scene.Choices != null)
        {
            foreach (var choice in scene.Choices)
            {
                var btn = new Button
                {
                    Text = choice.Text.TryGetValue(_language, out var chText) ? chText : "Выбор",
                    Command = new Command(() => OnChoiceSelected(choice))
                };
                ChoicesLayout.Children.Add(btn);
            }
        }
    }

    private void OnChoiceSelected(Choice choice)
    {
        // Найти следующую кат-сцену
        var nextScene = _adventure.Cutscenes.FirstOrDefault(s => s.Id == choice.NextSceneId);
        if (nextScene != null)
        {
            GameManager.Instance.State.CurrentSceneId = nextScene.Id;
            if (!GameManager.Instance.State.CompletedScenes.Contains(nextScene.Id))
                GameManager.Instance.State.CompletedScenes.Add(nextScene.Id);

            ShowCutscene(nextScene);
            GameManager.Instance.SaveAsync();
        }
        else
        {
            DisplayAlert("Конец", "Дальнейших сцен нет.", "OK");
            CutsceneFrame.IsVisible = false;
        }
    }
}
