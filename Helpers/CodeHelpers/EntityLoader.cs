using System;
using System.Collections.Generic;
using Godot;

public enum EntityType
{
    SQUARE,
    BAD_BLOCK,
    PRIZE_BOX
}

/// <summary>
/// Loads a PackedScene of the Entity you're looking for.
/// </summary>
public partial class EntityLoader
{
    private static readonly Dictionary<EntityType, string> EntityPathMaps = new()
    {
        { EntityType.SQUARE, "res://Scenes/Entities/Square/Square.tscn" },
        { EntityType.BAD_BLOCK, "res://Scenes/Entities/BadBlock/BadBlock.tscn" },
        { EntityType.PRIZE_BOX, "res://Scenes/Entities/PrizeBox/PrizeBox.tscn" }
    };

    public static PackedScene Get(EntityType entityPath)
    {
        if (EntityPathMaps.TryGetValue(entityPath, out var path))
        {
            var entity = (PackedScene)GD.Load(path);
            return entity;
        }
        throw new ArgumentException($"Scene path not defined for {entityPath}");
    }
}