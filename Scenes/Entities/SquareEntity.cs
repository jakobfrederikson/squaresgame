using Godot;

public partial class SquareEntity : Node2D
{
    [Signal]
    public delegate void ClickedEventHandler(SquareEntity square);

    [Signal]
    public delegate void MouseEnteredEventHandler();
    [Signal]
    public delegate void MouseExitedEventHandler();

    [Export] private Control control;
    private Texture2D _defaultCursor;
    private Texture2D _pointingHandCursor;

    private RectangleShape2D _rectangleShape2D;
    protected bool _mouseInSquare;
    protected Timer _despawnTimer;
    protected Tween _tween;

    public virtual int ScoreValue => 1;
    public EntityType Type { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _mouseInSquare = false;

        control.MouseEntered += OnControlMouseEntered;
        control.MouseExited += OnControlMouseExited;
        _defaultCursor = ResourceLoader.Load<Texture2D>("res://Assets/Cursor2.png");
        _pointingHandCursor = ResourceLoader.Load<Texture2D>("res://Assets/Cursor3.png");

        // Initialise the despawn timer
        _despawnTimer = new Timer();
        _despawnTimer.WaitTime = 1.0f;
        _despawnTimer.OneShot = true;
        _despawnTimer.Timeout += StartDespawnSequence;
        AddChild(_despawnTimer);

        _despawnTimer.Start();
    }

    private void OnControlMouseEntered()
    {
        _mouseInSquare = true;
        EmitSignal(SignalName.MouseEntered);
        Input.SetCustomMouseCursor(_pointingHandCursor);
    }

    private void OnControlMouseExited()
    {
        _mouseInSquare = false;
        EmitSignal(SignalName.MouseExited);
        Input.SetCustomMouseCursor(_defaultCursor);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse_click") && _mouseInSquare)
        {
            EmitSignal(SignalName.Clicked, this);
            Despawn();
        }
    }

    protected virtual void StartDespawnSequence()
    {
        _tween = CreateTween();
        _tween.TweenProperty(this, "scale", Godot.Vector2.Zero, 0.75f)
            .SetEase(Tween.EaseType.OutIn);
        _tween.TweenCallback(Callable.From(Despawn));
    }

    protected virtual void Despawn()
    {
        QueueFree();
        GD.Print($"{this} - square has despawned.");
    }

    public virtual Vector2 Size() => control.Size;
}