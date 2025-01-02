using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

public class SquareEntityInfo
{
    public EntityType Type { get; set; }
    public float SpawnProbability { get; set; }
    public PackedScene Scene { get; set; }
}

public partial class SquareEntitySpawner : Node
{
    private PackedScene _squareScene;
    private PackedScene _badBlockScene;
    private PackedScene _prizeBoxScene;
    private PackedScene _iceSquareScene;
    private MissedClickHandler _missedClickHandler;

    private List<SquareEntityInfo> _spawnQueue;

    private Rect2 _viewportRect;
    private Level _level;
    private Random _rng;

    public bool IsQueueEmpty => _spawnQueue.Count == 0 ? true : false;

    public int Seed { get; private set; }

    public SquareEntitySpawner(PackedScene squareScene,
        PackedScene badBlockScene, PackedScene prizeBoxScene,
        PackedScene iceSquareScene, MissedClickHandler missedClickHandler,
         Rect2 viewportRect, Level level, int seed)
    {
        _squareScene = squareScene;
        _badBlockScene = badBlockScene;
        _prizeBoxScene = prizeBoxScene;
        _iceSquareScene = iceSquareScene;
        _missedClickHandler = missedClickHandler;
        _viewportRect = viewportRect;
        _level = level;
        Seed = seed;
        _rng = new Random(seed);
    }

    public void PrecalculateSpawns(int totalSpawns, List<SquareEntityInfo> entityInfos)
    {
        float totalProbability = entityInfos.Sum(info => info.SpawnProbability);
        if (Math.Abs(totalProbability - 1.0f) > 0.0001f)
            throw new ArgumentException("The sum of all probabilities must equal to 1.0f.");

        _spawnQueue = new List<SquareEntityInfo>();

        foreach (var info in entityInfos)
        {
            int count = (int)(totalSpawns * info.SpawnProbability);
            for (int i = 0; i < count; i++)
            {
                _spawnQueue.Add(info);
            }
        }

        ShuffleList(_spawnQueue);
        WriteToFile(_spawnQueue);
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

        PackedScene sqEntityToSpawn = _spawnQueue[0].Scene;
        SquareEntity entity = sqEntityToSpawn.Instantiate<SquareEntity>();
        entity.Type = _spawnQueue[0].Type;
        _spawnQueue.RemoveAt(0);

        _level.AddChild(entity);
        _missedClickHandler.RegisterSquareEntity(entity);
        entity.Clicked += _level.OnSquareClicked;

        var rng = new RandomNumberGenerator();
        float x = rng.RandfRange(_viewportRect.Position.X + entity.Size().X / 2, _viewportRect.Position.X + _viewportRect.Size.X - entity.Size().X / 2);
        float y = rng.RandfRange(_viewportRect.Position.Y + entity.Size().Y / 2, _viewportRect.Position.Y + _viewportRect.Size.Y - entity.Size().Y / 2);
        entity.Position = new Vector2(x, y);
    }

    private void WriteToFile(List<SquareEntityInfo> spawnQueue)
    {
        int fileIndex = 1;
        string fileName;

        do
        {
            fileName = $"shuffled_spawn_queue_{fileIndex}.txt";
            fileIndex++;
        } while (File.Exists(fileName));

        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine("Shuffled Spawn Queue:");
            foreach (var entity in spawnQueue)
            {
                writer.WriteLine(entity.Type);
            }
        }

        Console.WriteLine($"Spawn queue written to {fileName}");
    }
}