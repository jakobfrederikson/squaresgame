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
    [Export] public int Cost;
    [Export] public bool Bought = false;
    [Export] public int RoundDuration;
    [Export] public int ScoreToWin;

    public int TotalSquaresClicked;
    public int ScucessfulClicks;
    public int BadBlockClicks;
    public int PrizeBoxClicks;
    public int NormalSquareClicks;
    public int MissedClicks;

    public LevelData() : this("", "", "", "", "", 0, 0, false) { }

    public LevelData(string texturePath, string pathToScene, string pathToResource,
                     string name, string description, int scoreToWin,
                     int cost, bool bought)
    {
        TexturePath = texturePath;
        PathToScene = pathToScene;
        PathToResource = pathToResource;
        Name = name;
        Description = description;
        ScoreToWin = scoreToWin;
        Cost = cost;
        Bought = bought;

        TotalSquaresClicked = 0;
        ScucessfulClicks = 0;
        BadBlockClicks = 0;
        PrizeBoxClicks = 0;
        NormalSquareClicks = 0;
        MissedClicks = 0;
        RoundDuration = 0;
    }
}
