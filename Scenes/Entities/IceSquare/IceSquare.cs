using Godot;
using System;

public partial class IceSquare : SquareEntity
{
    public override int ScoreValue => 0;

    private Timer _freezeTimer;
    private float _originalMouseSensitivity;

    public override void _Ready()
    {
        base._Ready();

        _freezeTimer = new Timer();
        _freezeTimer.WaitTime = 0.5f;
        _freezeTimer.OneShot = true;
        _freezeTimer.Timeout += UnfreezeMouse;
        AddChild(_freezeTimer);

        // _originalMouseSensitivity = Input.GetMouseSensitivity();
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
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
        _freezeTimer.Start();
        GD.Print("Ice Square - Froze mouse commence.");
    }

    private void UnfreezeMouse()
    {
        Input.SetMouseMode(Input.MouseModeEnum.Visible);
        GD.Print("Ice Square - Unfreeze mouse.");
        Despawn();
    }

    private void HideSquare()
    {
        this.Hide();
    }
}
