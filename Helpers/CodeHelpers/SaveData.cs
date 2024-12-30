using Godot;

public class SaveData
{
    public static void SavePlayerData(PlayerData playerData)
    {
        var result = ResourceSaver.Save(playerData);
        if (result != Error.Ok)
        {
            GD.PrintErr($"Failed to save PlayerData to {playerData.ResourcePath}: {result}");
        }
        else
        {
            GD.Print($"PlayerData saved successfully to {playerData.ResourcePath}");
        }
    }

    public static void SaveLevelData(LevelData levelData)
    {
        if (levelData.PathToResource == null || levelData.PathToResource == "")
        {
            GD.PrintErr("LevelData resource path is not set. Cannot save.");
        }
        var result = ResourceSaver.Save(levelData, levelData.PathToResource);
        if (result != Error.Ok)
        {
            GD.PrintErr($"Failed to save LevelData to {levelData.PathToResource}: {result}");
        }
        else
        {
            GD.Print($"LevelData saved successfully to {levelData.PathToResource}");
        }
    }

    public static void SaveUpgradeData(UpgradeData upgradeData)
    {
        if (upgradeData.PathToResource == null || upgradeData.PathToResource == "")
        {
            GD.PrintErr("UpgradeData resource path is not set. Cannot save.");
        }
        var result = ResourceSaver.Save(upgradeData);
        if (result != Error.Ok)
        {
            GD.PrintErr($"Failed to save UpgradeData to {upgradeData.PathToResource}: {result}");
        }
        else
        {
            GD.Print($"LevelData saved successfully to {upgradeData.PathToResource}");
        }
    }
}