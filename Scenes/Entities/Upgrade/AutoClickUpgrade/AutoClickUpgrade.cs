using Godot;
using System;
using System.Linq;

public partial class AutoClickUpgrade : UpgradeEntity
{
	private Sprite2D _cursor;
	private Texture2D _defaultCursorTexture2D;
	private Texture2D _mouseOverCursorTexture2D;

	private Timer _autoClickTimer;

	public enum CursorMode
	{
		DEFAULT,
		MOUSE_OVER
	}

	public override void _Ready()
	{
		SetCursorNode();
		LoadTexture2DCursors();
		SetupAutoClickTimer();
	}

	private void SetupAutoClickTimer()
	{
		float clickInterval = p_upgradeData.Stage switch
		{
			1 => 0.80f,
			2 => 0.70f,
			3 => 0.60f,
			4 => 0.50f,
			5 => 0.25f,
			_ => 0.80f
		};

		_autoClickTimer = new Timer
		{
			WaitTime = clickInterval,
			Autostart = true,
			OneShot = false
		};
		_autoClickTimer.Timeout += ApplyEffect;
		AddChild(_autoClickTimer);
	}

	private void SetCursorNode() => _cursor = GetNode<Sprite2D>("Cursor");

	private void LoadTexture2DCursors()
	{
		_defaultCursorTexture2D = GD.Load<Texture2D>("res://Assets/Cursor2.png");
		_mouseOverCursorTexture2D = GD.Load<Texture2D>("res://Assets/Cursor3.png");
		_cursor.Texture = _defaultCursorTexture2D;
	}

	public void SetCursorMode(CursorMode cursorMode)
	{
		_cursor.Texture = cursorMode switch
		{
			CursorMode.DEFAULT => _defaultCursorTexture2D,
			CursorMode.MOUSE_OVER => _mouseOverCursorTexture2D,
			_ => _defaultCursorTexture2D
		};
	}

	public override void ApplyEffect()
	{
		var squares = p_level.GetTree().GetNodesInGroup("Squares")
			.OfType<SquareEntity>()
			.Where(square => square.IsPositive && square.IsActive)
			.ToList();

		if (squares.Count > 0)
		{
			Vector2 startPosition = _cursor.GlobalPosition;
			Vector2 endPosition = squares.Last().GlobalPosition;
			Tween tween = GetTree().CreateTween();

			tween.TweenProperty(_cursor, "global_position", endPosition, 0.10f);
			tween.TweenCallback(Callable.From(() => SetCursorMode(CursorMode.MOUSE_OVER)));
			tween.TweenCallback(Callable.From(() => squares.Last().Click()));
			tween.TweenCallback(Callable.From(() => SetCursorMode(CursorMode.DEFAULT)));
		}
	}
}
