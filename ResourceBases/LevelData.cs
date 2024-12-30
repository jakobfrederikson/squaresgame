using Godot;
using System;

[Tool]
public partial class LevelData : Resource
{
    [Export(PropertyHint.File)] public string TexturePath;
    [Export(PropertyHint.File, "*.tscn")] public string PathToScene;
    [Export(PropertyHint.File, "*.tres")] public string PathToResource;
    [Export] public string Name;
    [Export(PropertyHint.MultilineText)] public string Description;
    [Export] public int Duration;
    [Export] public int PointsToWin;
    [Export] public int Cost;
    [Export] public bool Bought = false;

    public LevelData() : this("", "", "", "", "", 0, 0, 0, false) { }

    public LevelData(string texturePath, string pathToScene, string pathToResource,
                     string name, string description, int duration, int pointsToWin,
                     int cost, bool bought)
    {
        TexturePath = texturePath;
        PathToScene = pathToScene;
        PathToResource = pathToResource;
        Name = name;
        Description = description;
        Duration = duration;
        PointsToWin = pointsToWin;
        Cost = cost;
        Bought = bought;
    }
}
