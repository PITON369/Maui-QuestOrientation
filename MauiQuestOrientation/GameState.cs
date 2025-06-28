using System;

public class GameState
{
    public Adventure Adventure { get; set; } = null!;
    public string? CurrentSceneId { get; set; }
    public List<string> Inventory { get; set; } = new();
    public List<string> EnteredCodes { get; set; } = new();
    public List<string> CompletedScenes { get; set; } = new();

}

