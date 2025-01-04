using System;
using System.Collections.Generic;
using Godot;

public enum ScenePath
{
    START_MENU,
    LEVEL_SELECT,
    UPGRADES,
    STATS,
    LEVEL_OVER_SCENE
}

// Used when you need a packed scene to change your current scene to.
public partial class PackedSceneLoader
{
    private static readonly Dictionary<ScenePath, string> ScenePathMaps = new()
    {
        { ScenePath.START_MENU, "res://Scenes/Menus/StartMenu/StartMenu.tscn" },
        { ScenePath.LEVEL_SELECT, "res://Scenes/Menus/LevelSelectMenu/LevelSelectMenu.tscn" },
        { ScenePath.UPGRADES, "res://Scenes/Menus/UpgradesMenu/UpgradesMenu.tscn" },
        { ScenePath.STATS, "res://Scenes/Menus/StatsMenu/StatsMenu.tscn" },
        { ScenePath.LEVEL_OVER_SCENE, "res://Scenes/LevelHelpers/LevelOver/LevelOverScene.tscn"}
    };

    public static PackedScene Get(ScenePath scenePath)
    {
        if (ScenePathMaps.TryGetValue(scenePath, out var path))
        {
            var entity = (PackedScene)GD.Load(path);
            return entity;
        }
        throw new ArgumentException($"Scene path not defined for {scenePath}");
    }
}