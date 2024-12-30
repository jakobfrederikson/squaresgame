using System;
using System.Collections.Generic;
using Godot;

public enum ResourcePath
{
    LEVEL_ONE_DATA,
    LEVEL_TWO_DATA,
    LEVEL_THREE_DATA,
    PLAYER_DATA,
    UPGRADE_AUTO_CLICK_DATA,
    UPGRADE_TEST_DATA
}

public partial class ResourceDataLoader
{
    private static readonly Dictionary<ResourcePath, string> ResourcePathMaps = new()
    {
        { ResourcePath.PLAYER_DATA, "res://Resources/PlayerData/PlayerData.tres" },
        { ResourcePath.LEVEL_ONE_DATA, "res://Scenes/Levels/level1/level1_data.tres" },
        { ResourcePath.LEVEL_TWO_DATA, "res://Scenes/Levels/level2/level2_data.tres" },
        { ResourcePath.LEVEL_THREE_DATA, "res://Scenes/Levels/level3/level3_data.tres" },
        { ResourcePath.UPGRADE_AUTO_CLICK_DATA, "res://Resources/UpgradeData/AutoClickUpgrade.tres" },
        { ResourcePath.UPGRADE_TEST_DATA, "res://Resources/UpgradeData/UpgradeTest.tres" }
    };

    public static Resource Get(ResourcePath resourcePath)
    {
        var resource = GD.Load(ResourcePathMaps[resourcePath]);
        return resource;
    }
}