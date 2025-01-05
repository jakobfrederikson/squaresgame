using System.Collections.Generic;
using System.Numerics;
using Godot;

/// <summary>
/// Base class for Square Game levels.
/// 
/// Every level has a:
/// SquareTimer, RoundTimer, LevelData, PlayerData and hud.
/// </summary>
public abstract partial class Level : Node2D
{
    [Export] protected Timer SquareTimer;
    [Export] protected Timer RoundTimer;
    [Export] protected LevelData LevelData;
    protected PlayerData PlayerData;
    protected Hud hud;
    protected MissedClickHandler _missedClickHandler;
    protected ClickDataRecorder _clickDataRecorder;
    protected SquareEntityManager SquareEntityManager;
    protected SquareEntitySpawner SquareEntitySpawner;
    protected UpgradeEntityManager UpgradeEntityManager;
    protected UpgradeEntitySpawner UpgradeEntitySpawner;
    protected int _score;
    protected int _currentRoundTime;

    protected int TimeTakenToFinishLevel => LevelData.RoundDuration - _currentRoundTime;

    public override void _Ready()
    {
        GD.Print("Hello from base level class.");

        InitialiseTimers();
        InitialisePlayerData();
        InitialiseHUD();
        InitialiseHandlers();

        _score = 0;
        _currentRoundTime = LevelData.RoundDuration;

        SquareEntityManager = new SquareEntityManager();
        SquareEntitySpawner = new SquareEntitySpawner(_missedClickHandler, this, seed: 1);

        UpgradeEntityManager = new UpgradeEntityManager();

        UpgradeEntitySpawner = new UpgradeEntitySpawner(this);
        UpgradeEntitySpawner.SpawnUpgrades(UpgradeEntityManager.GetEnabledUpgrades());

        _clickDataRecorder = new ClickDataRecorder();
    }

    /// <summary>
    /// Initializes the timers and their respective signals.
    /// </summary>
    private void InitialiseTimers()
    {
        SquareTimer.Timeout += SpawnSquareEntity;
        SquareTimer.Autostart = true;

        RoundTimer.Timeout += UpdateHUDTime;
        RoundTimer.Timeout += CheckGameEnd;
        RoundTimer.Autostart = true;
    }

    private void InitialisePlayerData()
    {
        PlayerData = (PlayerData)ResourceDataLoader.Get(ResourcePath.PLAYER_DATA);
    }

    private void InitialiseHUD()
    {
        var hudScene = (PackedScene)GD.Load("res://Scenes/HUD/HUD.tscn");
        hud = hudScene.Instantiate<Hud>();
        AddChild(hud);
    }

    /// <summary>
    /// Sets up handlers for missed clicks and back button creation.
    /// </summary>
    private void InitialiseHandlers()
    {
        _missedClickHandler = new MissedClickHandler();
        _missedClickHandler.SquareEntityMissClicked += OnSquareMissClicked;
        AddChild(_missedClickHandler);

        CreateBackButton();
    }

    protected virtual float GetLevelDuration() => LevelData.RoundDuration;
    protected virtual void SpawnSquareEntity() => SquareEntitySpawner.SpawnSquareEntity();
    protected int GetTotalSpawns() => (int)(LevelData.RoundDuration / SquareTimer.WaitTime);

    /// <summary>
    /// Configures the squares to be loaded and their spawn probabilities for the level.
    /// This method must be implemented by derived classes to define which squares
    /// should appear in the level and in what proportions.
    /// </summary>
    protected abstract void SetSquaresForLevel();

    /// <summary>
    /// Configures the squares for the level by specifying their types and optional probabilities.
    /// </summary>
    /// <param name="configurations">Array of square configurations, each specifying a type and optional probability.</param>
    protected void ConfigureSquares(params SquareConfiguration[] configurations)
    {
        SquareEntityManager.LoadSquares(configurations);
        SquareEntitySpawner.InitialiseSpawnQueue(
            totalSpawns: GetTotalSpawns(),
            spawnInfos: SquareEntityManager.GetSpawnInfo()
        );
    }

    internal virtual void OnSquareClicked(SquareEntity square)
    {
        _clickDataRecorder.RecordSquareEntityClick(square.Type);
        _score += square.ScoreValue;
        hud.UpdateScore(_score);
        if (_score >= LevelData.ScoreToWin) CheckGameEnd();
    }

    protected virtual void OnSquareMissClicked()
    {
        _clickDataRecorder.RecordMissedClick();
        hud.MissClicked();
    }

    protected virtual void UpdateHUDTime()
    {
        _currentRoundTime -= 1;
        var timeDict = Time.GetTimeDictFromUnixTime(_currentRoundTime);
        int minute = (int)timeDict["minute"];
        int second = (int)timeDict["second"];
        hud.UpdateTime(minute, second);
    }

    protected void CheckGameEnd()
    {
        if (_currentRoundTime >= LevelData.RoundDuration) EndLevel(LevelOverType.TimeUp);
        else if (_score >= LevelData.ScoreToWin) EndLevel(LevelOverType.Won);
        else if (SquareEntitySpawner.IsQueueEmpty) EndLevel(LevelOverType.NoMoreSquares);
    }

    protected void EndLevel(LevelOverType levelOverType)
    {
        SaveLevelAndPlayerData();
        var levelOverScene = PackedSceneLoader.Get(ScenePath.LEVEL_OVER_SCENE);
        SetRoundEndData(levelOverType);
        GetTree().ChangeSceneToPacked(levelOverScene);
    }

    protected void SaveLevelAndPlayerData()
    {
        LevelData.NormalSquareClicks = _clickDataRecorder.GetSquareEntityClickCount(SquareEntityType.SQUARE);
        LevelData.BadBlockClicks = _clickDataRecorder.GetSquareEntityClickCount(SquareEntityType.BAD_BLOCK);
        LevelData.PrizeBoxClicks = _clickDataRecorder.GetSquareEntityClickCount(SquareEntityType.PRIZE_BOX);
        LevelData.MissedClicks = _clickDataRecorder.MissedClickCount;
        LevelData.TotalClicks = _clickDataRecorder.TotalClicks;
        LevelData.Score = _score;
        LevelData.TimeTakenToFinishLevel = TimeTakenToFinishLevel;

        PlayerData.UpdateLevelStats(LevelData);
        SaveData.SavePlayerAndLevelData(PlayerData, LevelData);
    }

    protected void SetRoundEndData(LevelOverType levelOverType)
    {
        RoundEndData.Instance.LevelOverType = levelOverType;
        RoundEndData.Instance.Score = _score;
        RoundEndData.Instance.TotalPoints = PlayerData.TotalPoints;
        RoundEndData.Instance.TotalClicks = LevelData.TotalClicks;
        RoundEndData.Instance.TotalSquaresClicked = LevelData.TotalSquaresClicked;
        RoundEndData.Instance.NormalSquareClicks = LevelData.NormalSquareClicks;
        RoundEndData.Instance.PrizeBoxClicks = LevelData.PrizeBoxClicks;
        RoundEndData.Instance.BadBlockClicks = LevelData.BadBlockClicks;
        RoundEndData.Instance.MissedClicks = LevelData.MissedClicks;
        RoundEndData.Instance.TimeTakenToFinishLevel = LevelData.TimeTakenToFinishLevel;
    }

    private void CreateBackButton()
    {
        // Create a Control node as a parent for the button
        var control = new Control
        {
            AnchorLeft = 0,
            AnchorTop = 0,
            AnchorRight = 1,
            AnchorBottom = 1,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ExpandFill
        };

        AddChild(control);

        // Create a Button instance
        var backButton = new Button
        {
            Text = "Back to Menu",
            SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter,
            SizeFlagsVertical = Control.SizeFlags.ShrinkCenter
        };

        // Anchor the button to the bottom center of the screen
        backButton.AnchorLeft = 0.5f;
        backButton.AnchorTop = 1.0f;
        backButton.AnchorRight = 0.5f;
        backButton.AnchorBottom = 1.0f;

        // Connect the button's "pressed" signal
        backButton.Pressed += OnBackButtonPressed;

        // Add the button to the Control node
        control.AddChild(backButton);
    }

    private void OnBackButtonPressed()
    {
        PackedScene scene = PackedSceneLoader.Get(ScenePath.START_MENU);
        GetTree().ChangeSceneToPacked(scene);
    }
}