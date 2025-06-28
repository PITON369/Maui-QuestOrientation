namespace MauiQuestOrientation;

public partial class GamePage : ContentPage
{
    private Adventure _adventure => GameManager.Instance.State.Adventure;
    private string _language => Preferences.Get("Language", "ru");

    public GamePage()
    {
        InitializeComponent();

        // ��������, ��� Adventure � Cutscenes ���������
        if (_adventure == null || _adventure.Cutscenes == null)
        {
            DisplayAlert("������", "������ ����������� �� ���������.", "OK");
            UpdateUI();
            return;
        }

        if (!string.IsNullOrEmpty(GameManager.Instance.State.CurrentSceneId))
        {
            // ����� � �������� ��������� ���-�����
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
        // ������ ���-����� �� ���������
        CutsceneFrame.IsVisible = false;
        CutsceneImage.IsVisible = false;
        CutsceneText.Text = string.Empty;
        ChoicesLayout.Children.Clear();

        // �������� ���������
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

        // ��� �������: ������ ��� ���� � �������� ���
        var allCodes = string.Join(", ", _adventure.ControlPoints.Select(c => $"'{c.Code}'"));
        System.Diagnostics.Debug.WriteLine($"��� ���� ��: {allCodes}");
        System.Diagnostics.Debug.WriteLine($"�������� ���: '{code}'");

        // ����� �� �� ����
        var cp = _adventure.ControlPoints.FirstOrDefault(c => c.Code.Trim() == code.Trim());
        if (cp == null)
        {
            DisplayAlert("������", "��� �� ������.", "OK");
            return;
        }

        // ����� ���-�����
        var scene = _adventure.Cutscenes.FirstOrDefault(s => s.Id == cp.CutsceneId);
        if (scene == null)
        {
            DisplayAlert("������", "���-����� �� �������.", "OK");
            return;
        }

        // ��������� ������� ����� � ���������
        GameManager.Instance.State.CurrentSceneId = scene.Id;
        if (!GameManager.Instance.State.CompletedScenes.Contains(scene.Id))
            GameManager.Instance.State.CompletedScenes.Add(scene.Id);

        // �������� ���-�����
        ShowCutscene(scene);

        // ��������� ��������
        GameManager.Instance.SaveAsync();
    }

    private void ShowCutscene(Cutscene scene)
    {
        CutsceneFrame.IsVisible = true;
        CutsceneText.Text = scene.Text.TryGetValue(_language, out var text) ? text : scene.Id;

        // �������� ��������, ���� ����
        if (!string.IsNullOrEmpty(scene.Image))
        {
            CutsceneImage.Source = ImageSource.FromFile(scene.Image);
            CutsceneImage.IsVisible = true;
        }
        else
        {
            CutsceneImage.IsVisible = false;
        }

        // �������� �������� ������
        ChoicesLayout.Children.Clear();
        if (scene.Choices != null)
        {
            foreach (var choice in scene.Choices)
            {
                var btn = new Button
                {
                    Text = choice.Text.TryGetValue(_language, out var chText) ? chText : "�����",
                    Command = new Command(() => OnChoiceSelected(choice))
                };
                ChoicesLayout.Children.Add(btn);
            }
        }
    }

    private void OnChoiceSelected(Choice choice)
    {
        // ����� ��������� ���-�����
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
            DisplayAlert("�����", "���������� ���� ���.", "OK");
            CutsceneFrame.IsVisible = false;
        }
    }
}
