
using Godot;

public partial class MissedClickHandler : Node2D
{
    [Signal]
    public delegate void SquareEntityMissClickedEventHandler();

    private bool _mouseOverSquareEntity = false;

    public void RegisterSquareEntity(SquareEntity squareEntity)
    {
        squareEntity.MouseEntered += OnSquareMouseEntered;
        squareEntity.MouseExited += OnSquareMouseExited;
    }

    private void OnSquareMouseEntered() => _mouseOverSquareEntity = true;
    private void OnSquareMouseExited() => _mouseOverSquareEntity = false;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse_click") && !_mouseOverSquareEntity)
            EmitSignal(SignalName.SquareEntityMissClicked);
    }
}