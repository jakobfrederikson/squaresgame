using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

public record SquareEntitySpawnInfo
{
    public SquareEntityType Type { get; set; }
    public float SpawnPercentage { get; set; }
    public PackedScene Scene { get; set; }
}

public partial class SquareEntitySpawner : Node
{
    private readonly List<SquareEntitySpawnInfo> _spawnQueue;
    private readonly MissedClickHandler _missedClickHandler;
    private readonly Level _level;
    private readonly Random _rng;

    public bool IsQueueEmpty => _spawnQueue.Count == 0 ? true : false;

    public int Seed { get; private set; }

    public SquareEntitySpawner(
        MissedClickHandler missedClickHandler,
        Level level,
         int seed)
    {
        _missedClickHandler = missedClickHandler;
        _level = level;
        Seed = seed;
        _rng = new Random(seed);
        _spawnQueue = new();
    }

    public void InitialiseSpawnQueue(int totalSpawns, List<SquareEntitySpawnInfo> spawnInfos)
    {
        float totalPercentage = spawnInfos.Sum(info => info.SpawnPercentage);
        if (Math.Abs(totalPercentage - 1.0f) > 0.0001f)
            throw new ArgumentException("The sum of all percentages must equal to 1.0f.");

        foreach (var info in spawnInfos)
        {
            int count = (int)(totalSpawns * info.SpawnPercentage);
            for (int i = 0; i < count; i++)
            {
                _spawnQueue.Add(info);
            }
        }
        ShuffleList(_spawnQueue);
    }

    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void SpawnSquareEntity()
    {
        if (_spawnQueue.Count == 0) return; // No more squares to spawn

        SquareEntitySpawnInfo info = _spawnQueue[0];
        SquareEntity squareEntity = info.Scene.Instantiate<SquareEntity>();
        squareEntity.Type = info.Type;

        _spawnQueue.RemoveAt(0);

        _level.AddChild(squareEntity);
        _missedClickHandler.RegisterSquareEntity(squareEntity);
        squareEntity.Clicked += _level.OnSquareClicked;

        var rng = new RandomNumberGenerator();
        var viewportRect = _level.GetViewport().GetVisibleRect();
        float x = rng.RandfRange(viewportRect.Position.X + squareEntity.Size().X / 2, viewportRect.Position.X + viewportRect.Size.X - squareEntity.Size().X / 2);
        float y = rng.RandfRange(viewportRect.Position.Y + squareEntity.Size().Y / 2, viewportRect.Position.Y + viewportRect.Size.Y - squareEntity.Size().Y / 2);
        squareEntity.Position = new Vector2(x, y);
    }
}