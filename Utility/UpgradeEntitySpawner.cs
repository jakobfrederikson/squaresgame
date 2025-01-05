using System.Collections.Generic;
using Godot;

public class UpgradeEntitySpawner
{
    private readonly Level _level;

    public UpgradeEntitySpawner(Level level)
    {
        _level = level;
    }

    public void SpawnUpgrades(List<UpgradeData> upgrades)
    {
        foreach (var upgradeData in upgrades)
        {
            if (ResourceLoader.Exists(upgradeData.PathToResource))
            {
                var packedScene = ResourceLoader.Load<PackedScene>(upgradeData.PathToResource);
                if (packedScene != null)
                {
                    var upgrade = packedScene.Instantiate<UpgradeEntity>();
                    _level.AddChild(upgrade);
                }
            }
        }
    }
}