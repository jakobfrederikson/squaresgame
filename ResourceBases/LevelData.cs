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
    [Export] public bool Bought;
    [Export] public bool PointsPerWin;
    [Export] public int RoundDuration;
    [Export] public int ScoreToWin;


    // Set once a player plays a level
    [Export] public int TotalClicks;
    [Export] public int NormalSquareClicks;
    [Export] public int BadBlockClicks;
    [Export] public int PrizeBoxClicks;
    [Export] public int MissedClicks;
    [Export] public float PointsMultiplierPerWin;
    [Export] public int Score;
    [Export] public double TimeTakenToFinishLevel;

    public int TotalSquaresClicked => NormalSquareClicks + PrizeBoxClicks + BadBlockClicks;
    public int GoodSquareClicks => NormalSquareClicks + PrizeBoxClicks;

    public LevelData()
    {
        InitializeDefaultValues();
    }

    public LevelData(string texturePath, string pathToScene,
        string pathToResource, string name, string description,
        int cost, bool bought, int roundDuration, int scoreToWin,
        int totalClicks, int normalSquareClicks, int badBlockClicks,
        int prizeBoxClicks, int missedClicks)
    {
        TexturePath = texturePath;
        PathToScene = pathToScene;
        PathToResource = pathToResource;
        Name = name;
        Description = description;
        Cost = cost;
        Bought = bought;
        RoundDuration = roundDuration;
        ScoreToWin = scoreToWin;
        TotalClicks = totalClicks;
        NormalSquareClicks = normalSquareClicks;
        BadBlockClicks = badBlockClicks;
        PrizeBoxClicks = prizeBoxClicks;
        MissedClicks = missedClicks;
    }

    private void InitializeDefaultValues()
    {
        Bought = false;
        TotalClicks = 0;
        BadBlockClicks = 0;
        PrizeBoxClicks = 0;
        NormalSquareClicks = 0;
        MissedClicks = 0;
        RoundDuration = 0;
        PointsMultiplierPerWin = 1.0f;
    }
}
