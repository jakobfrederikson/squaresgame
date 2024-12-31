using Godot;

/// <summary>
/// Singleton that holds the data of how the last level ended.
/// </summary>
public partial class RoundEndData : Node
{
    public static RoundEndData Instance { get; private set; }

    /// <summary>
    /// The latest way a round was ended.
    /// </summary>
    public LevelOverType LevelOverType { get; set; }
    public int Score { get; set; }
    public int TotalPoints { get; set; }
    public int TotalSquaresClicked { get; set; }
    public int NormalSquareClicks { get; set; }
    public int PrizeBoxClicks { get; set; }
    public int BadBlockClicks { get; set; }
    public int MissedClicks { get; set; }
    public double TimeTakenToFinishLevel { get; set; }

    public override void _Ready()
    {
        Instance ??= this;
    }
}