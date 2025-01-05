using Godot;

public partial class UpgradeEntity : Node2D
{
    protected UpgradeData p_upgradeData;
    protected Level p_level;

    public void Initialise(UpgradeData upgradeData, Level level)
    {
        p_upgradeData = upgradeData;
        p_level = level;
    }

    public virtual void ApplyEffect() { }
}