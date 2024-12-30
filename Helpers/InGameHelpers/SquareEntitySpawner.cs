using System;
using System.Collections.Generic;
using Godot;

public partial class SquareEntitySpawner : Node
{
    private PackedScene _squareScene;
    private PackedScene _badBlockScene;
    private PackedScene _prizeBoxScene;
    private MissedClickHandler _missedClickHandler;

    private List<PackedScene> _spawnQueue;

    private Rect2 _viewportRect;
    private Level _level;

    private float _squareProbability;
    private float _badBlockProbability;
    private float _prizeBoxProbability;

    public bool IsQueueEmpty => _spawnQueue.Count == 0 ? true : false;

    public SquareEntitySpawner(PackedScene squareScene,
        PackedScene badBlockScene, PackedScene prizeBoxScene,
        MissedClickHandler missedClickHandler, Rect2 viewportRect,
        Level level)
    {
        _squareScene = squareScene;
        _badBlockScene = badBlockScene;
        _prizeBoxScene = prizeBoxScene;
        _missedClickHandler = missedClickHandler;
        _viewportRect = viewportRect;
        _level = level;
    }

    public void PrecalculateSpawns(int totalSpawns, float squareProbability, float badBlockProbability, float prizeBoxProbability)
    {
        int squareCount = (int)(totalSpawns * squareProbability);
        int badBlockCount = (int)(totalSpawns * badBlockProbability);
        int prizeBoxCount = (int)(totalSpawns * prizeBoxProbability);

        _spawnQueue = new List<PackedScene>();

        for (int i = 0; i < squareCount; i++) _spawnQueue.Add(_squareScene);
        for (int i = 0; i < badBlockCount; i++) _spawnQueue.Add(_badBlockScene);
        for (int i = 0; i < prizeBoxCount; i++) _spawnQueue.Add(_prizeBoxScene);

        ShuffleList(_spawnQueue);
    }

    private void ShuffleList<T>(List<T> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void SpawnSquareEntity()
    {
        if (_spawnQueue.Count == 0) return; // No more squares to spawn

        PackedScene sqEntityToSpawn = _spawnQueue[0];
        _spawnQueue.RemoveAt(0);

        SquareEntity entity = sqEntityToSpawn.Instantiate<SquareEntity>();
        _level.AddChild(entity);
        _missedClickHandler.RegisterSquareEntity(entity);
        entity.Clicked += _level.OnSquareClicked;

        var rng = new RandomNumberGenerator();
        float x = rng.RandfRange(_viewportRect.Position.X + entity.Size().X / 2, _viewportRect.Position.X + _viewportRect.Size.X - entity.Size().X / 2);
        float y = rng.RandfRange(_viewportRect.Position.Y + entity.Size().Y / 2, _viewportRect.Position.Y + _viewportRect.Size.Y - entity.Size().Y / 2);
        entity.Position = new Vector2(x, y);
    }
}