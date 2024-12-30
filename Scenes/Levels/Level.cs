using System.Collections.Generic;
using System.Numerics;
using Godot;

/// <summary>
/// Base class for Square Game levels.
/// 
/// Every level has a:
/// SquareTimer, RoundTimer, LevelData, PlayerData and hud.
/// </summary>
public partial class Level : Node2D
{
    [Export] protected Timer SquareTimer;
    [Export] protected Timer RoundTimer;
    protected int prt_baseTime; // protected baseTime
    [Export] protected LevelData LevelData;
    protected PackedScene SquareScene;
    protected int squareSpawnCount;
    protected PackedScene BadBlockScene;
    protected PackedScene PrizeBoxScene;
    protected PlayerData PlayerData;
    protected Hud hud;
    protected MissedClickHandler _missedClickHandler;
    protected ClickDataRecorder _clickDataRecorder;
    protected SquareEntitySpawner _spawner;
    protected int _score;

    public override void _Ready()
    {
        GD.Print("Hello from base level class.");
        GD.Print(LevelData);

        SquareTimer.Timeout += SpawnSquareEntity;
        SquareTimer.Autostart = true;

        RoundTimer.Timeout += UpdateHUDTime;
        RoundTimer.Autostart = true;

        prt_baseTime = 0;

        // Load SquareEntity PackedScenes
        SquareScene = EntityLoader.Get(EntityType.SQUARE);
        BadBlockScene = EntityLoader.Get(EntityType.BAD_BLOCK);
        PrizeBoxScene = EntityLoader.Get(EntityType.PRIZE_BOX);

        // Get PlayerData
        PlayerData = (PlayerData)ResourceDataLoader.Get(ResourcePath.PLAYER_DATA);

        // Initialize the HUD
        var hudScene = (PackedScene)GD.Load("res://Scenes/HUD/HUD.tscn");
        hud = hudScene.Instantiate<Hud>();
        AddChild(hud);

        _missedClickHandler = new MissedClickHandler();
        _missedClickHandler.SquareEntityMissClicked += OnSquareMissClicked;
        AddChild(_missedClickHandler);

        CreateBackButton();

        _score = 0;

        _spawner = new SquareEntitySpawner(SquareScene, BadBlockScene, PrizeBoxScene,
        _missedClickHandler, GetViewport().GetVisibleRect(), this, seed: 1);

        _clickDataRecorder = new ClickDataRecorder();
    }

    protected virtual float GetLevelDuration() => LevelData.RoundDuration;
    protected virtual void SpawnSquareEntity() => _spawner.SpawnSquareEntity();

    internal virtual void OnSquareClicked(SquareEntity square)
    {
        _clickDataRecorder.RecordSquareEntityClick(square.Type);
        _score += square.ScoreValue;

        hud.UpdateScore(_score);

        if (_spawner.IsQueueEmpty || _score >= LevelData.ScoreToWin)
        {
            EndLevel();
            GetTree().ChangeSceneToPacked(PackedSceneLoader.Get(ScenePath.LEVEL_OVER_SCENE));
        }
    }

    protected virtual void OnSquareMissClicked()
    {
        _clickDataRecorder.RecordMissedClick();
        hud.MissClicked();
    }

    protected virtual void UpdateHUDTime()
    {
        prt_baseTime += 1;

        var timeDict = Time.GetTimeDictFromUnixTime(prt_baseTime);

        int minute = (int)timeDict["minute"];
        int second = (int)timeDict["second"];

        hud.UpdateTime(minute, second);
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

    protected void EndLevel()
    {
        LevelData.TotalSquaresClicked = _clickDataRecorder.SquareEntityClickCount(EntityType.SQUARE);
        LevelData.BadBlockClicks = _clickDataRecorder.SquareEntityClickCount(EntityType.BAD_BLOCK);
        LevelData.PrizeBoxClicks = _clickDataRecorder.SquareEntityClickCount(EntityType.PRIZE_BOX);
        LevelData.MissedClicks = _clickDataRecorder.MissedClickCount;

        SaveData.SaveLevelData(LevelData);
        PlayerData.UpdateLevelStats(LevelData);
    }
}