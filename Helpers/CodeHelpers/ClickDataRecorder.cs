using System;
using Godot;

public class ClickDataRecorder
{
    private Godot.Collections.Dictionary<EntityType, int> _squareEntityClickCounts;
    private int _missedClickCount;
    private int _totalClicks;

    public int MissedClickCount => _missedClickCount;
    public int TotalClicks => _totalClicks;

    public ClickDataRecorder()
    {
        _squareEntityClickCounts = new Godot.Collections.Dictionary<EntityType, int>();

        foreach (EntityType type in Enum.GetValues(typeof(EntityType)))
        {
            _squareEntityClickCounts[type] = 0;
        };
        _missedClickCount = 0;
        _totalClicks = 0;
    }

    public void RecordSquareEntityClick(EntityType type)
    {
        _squareEntityClickCounts[type]++;
        RecordClick();
    }

    public void RecordMissedClick()
    {
        _missedClickCount++;
        RecordClick();
    }

    public void RecordClick() => _totalClicks++;

    public int GetSquareEntityClickCount(EntityType type)
    {
        if (_squareEntityClickCounts.TryGetValue(type, out int count))
        {
            return count;
        }

        return 0;
    }
}