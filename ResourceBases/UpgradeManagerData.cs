using System.Collections.Generic;
using Godot;

[Tool]
public partial class UpgradeManagerData : Resource
{
    private readonly List<UpgradeData> _allUpgrades;

    public UpgradeManagerData()
    {
        _allUpgrades = new();
        _allUpgrades = InitialiseAllUpgrades();
    }

    private List<UpgradeData> InitialiseAllUpgrades()
    {
        List<UpgradeData> upgrades = new();

        using var dir = DirAccess.Open("res://Resources/UpgradeData/");
        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                if (dir.CurrentIsDir())
                {
                    GD.Print($"Found directory: {fileName}");
                }
                else
                {
                    GD.Print($"Found file: {fileName}");
                    var upgrade = ResourceLoader.Load<UpgradeData>("res://Resources/UpgradeData/" + fileName);
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

    public List<UpgradeData> GetAllUpgrades()
    {
        return _allUpgrades;
    }
}