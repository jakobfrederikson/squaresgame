using System.Collections.Generic;
using Godot;

public class UpgradeEntityManager
{
    private readonly List<UpgradeData> _allUpgrades;

    public UpgradeEntityManager()
    {
        _allUpgrades = new();
        _allUpgrades = LoadAllUpgrades();
    }

    private List<UpgradeData> LoadAllUpgrades()
    {
        List<UpgradeData> upgrades = new();

        using var dir = DirAccess.Open("res://Resources/UpgradeData/");
        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (!string.IsNullOrEmpty(fileName))
            {
                if (!dir.CurrentIsDir())
                {
                    GD.Print($"Found file: {fileName}");
                    var upgrade = ResourceLoader.Load<UpgradeData>($"res://Resources/UpgradeData/{fileName}");
                    upgrades.Add(upgrade);
                }
                fileName = dir.GetNext();
            }
        }
        else
        {
            GD.Print("An error occurred when trying to access the directory path for Upgrades.");
        }

        return upgrades;
    }

    public List<UpgradeData> GetAllUpgrades() => _allUpgrades;

    public List<UpgradeData> GetBoughtUpgrades() =>
        _allUpgrades.FindAll(upgrade => upgrade.IsBought);

    public List<UpgradeData> GetEnabledUpgrades() =>
        _allUpgrades.FindAll(upgrade => upgrade.IsBought && upgrade.IsEnabled);
}