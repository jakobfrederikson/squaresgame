using Godot;
using System;

public partial class AutoClickUpgrade : UpgradeEntity
{
	private Sprite2D _cursor;
	private Texture2D _defaultCursorTexture2D;
	private Texture2D _mouseOverCursorTexture2D;

	public enum CursorMode
	{
		DEFAULT,
		MOUSE_OVER
	}

	public override void _Ready()
	{
		SetCursorNode();
		LoadTexture2DCursors();
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

	public override void ApplyEffect(Level level)
	{
		base.ApplyEffect(level);
	}
}
