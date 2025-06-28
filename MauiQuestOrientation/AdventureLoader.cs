using System.Text.Json;

public static class AdventureLoader
{
    public static async Task<Adventure?> LoadAsync(string fileName = "adventure.json")
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var adv = JsonSerializer.Deserialize<Adventure>(json, options);
            System.Diagnostics.Debug.WriteLine($"adv is null: {adv == null}");
            System.Diagnostics.Debug.WriteLine($"adv.Languages: {adv?.Languages?.Length}");
            System.Diagnostics.Debug.WriteLine($"adv.ControlPoints: {adv?.ControlPoints?.Count}");
            System.Diagnostics.Debug.WriteLine($"adv.Cutscenes: {adv?.Cutscenes?.Count}");
            System.Diagnostics.Debug.WriteLine($"adv.Items: {adv?.Items?.Count}");
            return adv;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка загрузки adventure.json: {ex}");
            return null;
        }
    }


}
