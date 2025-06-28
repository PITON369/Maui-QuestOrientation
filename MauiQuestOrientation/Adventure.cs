using System.Collections.Generic;

public class Adventure
{
    public string[] Languages { get; set; }
    public List<ControlPoint> ControlPoints { get; set; }
    public List<Cutscene> Cutscenes { get; set; }
    public List<Item> Items { get; set; }
}

public class ControlPoint
{
    public string Code { get; set; }
    public string CutsceneId { get; set; }
}

public class Cutscene
{
    public string Id { get; set; }
    public Dictionary<string, string> Text { get; set; }
    public string Image { get; set; }
    public List<Choice> Choices { get; set; }
}

public class Choice
{
    public Dictionary<string, string> Text { get; set; }
    public string NextSceneId { get; set; }
    public string RequiredItemId { get; set; }
}

public class Item
{
    public string Id { get; set; }
    public Dictionary<string, string> Name { get; set; }
    public Dictionary<string, string> Description { get; set; }
    public bool CanUseOnOthers { get; set; }
    public string Image { get; set; }
}
