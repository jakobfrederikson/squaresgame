using Godot;

public partial class CustomCusor : Node
{
    public override void _Ready()
    {
        Input.SetCustomMouseCursor(
            ResourceLoader.Load<Texture2D>("res://Assets/Cursor2.png")
        );
        Input.SetCustomMouseCursor(
            ResourceLoader.Load<Texture2D>("res://Assets/Cursor3.png"),
            Input.CursorShape.PointingHand
        );
    }
}