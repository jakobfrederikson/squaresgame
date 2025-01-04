using System;
using Godot;

public class ClickDataRecorder
{
    private Godot.Collections.Dictionary<SquareEntityType, int> _squareEntityClickCounts;
    private int _missedClickCount;
    private int _totalClicks;

    public int MissedClickCount => _missedClickCount;
    public int TotalClicks => _totalClicks;

    public ClickDataRecorder()
    {
        _squareEntityClickCounts = new Godot.Collections.Dictionary<SquareEntityType, int>();

        foreach (SquareEntityType type in Enum.GetValues(typeof(SquareEntityType)))
        {
            _squareEntityClickCounts[type] = 0;
        };
        _missedClickCount = 0;
        _totalClicks = 0;
    }

    public void RecordSquareEntityClick(SquareEntityType type)
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

    public int GetSquareEntityClickCount(SquareEntityType type)
    {
        if (_squareEntityClickCounts.TryGetValue(type, out int count))
        {
            return count;
        }

        return 0;
    }
}