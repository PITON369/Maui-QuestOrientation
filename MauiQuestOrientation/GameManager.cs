using System.Text.Json;

namespace MauiQuestOrientation
{
    public class GameManager
    {
        // Синглтон Instance
        private static readonly Lazy<GameManager> _instance = new(() => new GameManager());
        public static GameManager Instance => _instance.Value;

        // Приватный конструктор, чтобы нельзя было создать экземпляр извне
        private GameManager() { }

        private const string SaveFileName = "gamestate.json";
        public GameState State { get; private set; } = new();

        public async Task StartNewGameAsync()
        {
            var adventure = await AdventureLoader.LoadAsync("adventure.json");
            State = new GameState
            {
                Adventure = adventure,
                CurrentSceneId = "start",
                Inventory = new List<string>(),
                EnteredCodes = new List<string>(),
                CompletedScenes = new List<string>()
            };
            await SaveAsync();
        }

        public async Task<bool> ContinueGameAsync()
        {
            var file = Path.Combine(FileSystem.AppDataDirectory, SaveFileName);
            if (!File.Exists(file))
                return false;

            var json = await File.ReadAllTextAsync(file);
            State = JsonSerializer.Deserialize<GameState>(json) ?? new GameState();
            return true;
        }

        public async Task SaveAsync()
        {
            var file = Path.Combine(FileSystem.AppDataDirectory, SaveFileName);
            var json = JsonSerializer.Serialize(State);
            await File.WriteAllTextAsync(file, json);
        }
    }

}
