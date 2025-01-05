using Godot;

public partial class UpgradeEntity : Node2D
{
    public virtual void ApplyEffect(Level level) { }

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}