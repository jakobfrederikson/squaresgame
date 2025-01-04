using System;
using System.Collections.Generic;
using Godot;

/// <summary>
/// Loads a PackedScene of the Entity you're looking for.
/// </summary>
public partial class EntityLoader
{
    private static readonly Dictionary<SquareEntityType, string> EntityPathMaps = new()
    {
        { SquareEntityType.SQUARE, "res://Scenes/Entities/Square/Square.tscn" },
        { SquareEntityType.BAD_BLOCK, "res://Scenes/Entities/BadBlock/BadBlock.tscn" },
        { SquareEntityType.PRIZE_BOX, "res://Scenes/Entities/PrizeBox/PrizeBox.tscn" },
        { SquareEntityType.ICE_SQUARE, "res://Scenes/Entities/IceSquare/IceSquare.tscn"},
    };

    public static PackedScene Get(SquareEntityType entityPath)
    {
        if (EntityPathMaps.TryGetValue(entityPath, out var path))
        {
            var entity = (PackedScene)GD.Load(path);
            GD.Print($"Loaded entity <{path}>");
            return entity;
        }
        throw new ArgumentException($"Scene path not defined for {entityPath}");
    }
}