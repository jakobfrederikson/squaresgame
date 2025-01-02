using Godot;
using System;

public partial class IceSquare : SquareEntity
{
    public override int ScoreValue => 0;

    private Timer _freezeTimer;
    private Vector2 _frozenMousePosition;
    private bool _frozen = false;

    public override void _Ready()
    {
        base._Ready();

        _freezeTimer = new Timer();
        _freezeTimer.WaitTime = 2f;
        _freezeTimer.OneShot = true;
        _freezeTimer.Timeout += UnfreezeMouse;
        AddChild(_freezeTimer);
    }

    public override void _Process(double delta)
    {
        if (_frozen)
        {
            Vector2 currentMousePos = GetViewport().GetMousePosition();
            Vector2 mouseDelta = (currentMousePos - _frozenMousePosition) * 5f * (float)delta;
            _frozenMousePosition += mouseDelta;
            Input.WarpMouse(_frozenMousePosition);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse_click") && _mouseInSquare)
        {
            EmitSignal(SignalName.Clicked, this);
            FreezeMouse();
            Hide();
        }
    }

    private void FreezeMouse()
    {
        _frozenMousePosition = GetViewport().GetMousePosition();
        _freezeTimer.Start();
        _frozen = true;
        GD.Print("Ice Square - Froze mouse commence.");
    }

    private void UnfreezeMouse()
    {
        _frozen = false;
        GD.Print("Ice Square - Unfreeze mouse.");
        Despawn();
    }

    private void HideSquare()
    {
        this.Hide();
    }
}
