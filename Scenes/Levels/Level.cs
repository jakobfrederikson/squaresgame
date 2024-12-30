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

    protected int _squaresClicked;
    protected MissedClickHandler _missedClickHandler;

    public override void _Ready()
    {
        GD.Print("Hello from base level class.");

        SquareTimer.Timeout += SpawnSquareEntity;
        SquareTimer.Autostart = true;

        RoundTimer.Timeout += UpdateHUDTime;
        RoundTimer.Autostart = true;

        prt_baseTime = 0;
        _squaresClicked = 0;

        // Load SquareEntity PackedScenes
        SquareScene = EntityLoader.Get(Entities.SQUARE);
        BadBlockScene = EntityLoader.Get(Entities.BAD_BLOCK);
        PrizeBoxScene = EntityLoader.Get(Entities.PRIZE_BOX);

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
    }

    /// <summary>
    /// Base SpawnSquareEntity method that instantiates 
    /// a square and sets it position to be randomly around the viewport.
    /// </summary>
    protected virtual void SpawnSquareEntity()
    {
        var square = SquareScene.Instantiate<Square>();
        AddChild(square);

        _missedClickHandler.RegisterSquareEntity(square);
        square.Clicked += OnSquareClicked;

        squareSpawnCount++;

        var viewportRect = GetViewport().GetVisibleRect();

        var rng = new RandomNumberGenerator();
        float x = rng.RandfRange(viewportRect.Position.X + square.Size().X / 2, viewportRect.Position.X + viewportRect.Size.X - square.Size().X / 2);
        float y = rng.RandfRange(viewportRect.Position.Y + square.Size().Y / 2, viewportRect.Position.Y + viewportRect.Size.Y - square.Size().Y / 2);

        square.Position = new Godot.Vector2(x, y);
    }

    protected virtual void OnSquareClicked()
    {
        _squaresClicked += 1;
        hud.SquareClicked();
    }

    protected virtual void OnSquareMissClicked()
    {
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
}