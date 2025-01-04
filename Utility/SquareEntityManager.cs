using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public enum SquareEntityType
{
    SQUARE,
    BAD_BLOCK,
    PRIZE_BOX,
    ICE_SQUARE,
}

public partial class SquareEntityManager : Node
{
    private readonly Dictionary<SquareEntityType, string> _scenePaths = new()
    {
         { SquareEntityType.SQUARE, "res://Scenes/Entities/Square/Square.tscn"},
         { SquareEntityType.BAD_BLOCK, "res://Scenes/Entities/BadBlock/BadBlock.tscn"},
         { SquareEntityType.PRIZE_BOX, "res://Scenes/Entities/PrizeBox/PrizeBox.tscn"},
         { SquareEntityType.ICE_SQUARE, "res://Scenes/Entities/IceSquare/IceSquare.tscn"}
    };

    private readonly Dictionary<SquareEntityType, PackedScene> _squareEntityScenes;
    private List<SquareEntitySpawnInfo> _spawnInfos;

    public SquareEntityManager()
    {
        _squareEntityScenes = new();
        _spawnInfos = new();
    }

    /// <summary>
    /// Used in a class that derives from Level.
    /// Loads the scenes for the squares specified in a level, then passes the scenes
    /// to the SquareEntitySpawner to create the spawnQueue for the level.
    /// </summary>
    /// <param name="squaresToLoad">The list of squares, and optionally their percentages
    /// to show how many of each square to load.</param>
    public void LoadSquares(params SquareConfiguration[] configurations)
    {
        float totalSpecifiedPercentage = configurations
        .Where(config => config.Percentage.HasValue)
        .Sum(config => config.Percentage.Value);

        if (totalSpecifiedPercentage > 1.0f)
            throw new ArgumentException("The sum of all provided probabilities must not exceed 1.0.");

        float remainingPercentage = 1.0f - totalSpecifiedPercentage;
        int unspecifiedCount = configurations.Count(config => !config.Percentage.HasValue);

        foreach (var config in configurations)
        {
            float resolvedPercentage = config.Percentage ?? (remainingPercentage / unspecifiedCount);
            if (_scenePaths.TryGetValue(config.Type, out var scenePath))
            {
                PackedScene scene = (PackedScene)GD.Load(scenePath);
                if (scene != null)
                {
                    _squareEntityScenes[config.Type] = scene;
                    _spawnInfos.Add(new SquareEntitySpawnInfo
                    {
                        Type = config.Type,
                        Scene = scene,
                        SpawnPercentage = resolvedPercentage
                    });
                }
                else
                {
                    GD.PrintErr($"Failed to load scene for {config.Type}.");
                }
            }
            else
            {
                GD.PrintErr($"Scene path not found for {config.Type}.");
            }
        }
    }

    public List<SquareEntitySpawnInfo> GetSpawnInfo()
    {
        return _spawnInfos;
    }
}