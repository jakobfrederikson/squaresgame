using Godot;

public class ClickDataRecorder
{
    private Godot.Collections.Dictionary<EntityType, int> _squareEntityClickCounts;
    private int _missedClickCount;

    public int MissedClickCount => _missedClickCount;
    public int SquareEntityClickCount(EntityType type)
    {
        return type switch
        {
            EntityType.SQUARE => _squareEntityClickCounts[EntityType.SQUARE],
            EntityType.BAD_BLOCK => _squareEntityClickCounts[EntityType.BAD_BLOCK],
            EntityType.PRIZE_BOX => _squareEntityClickCounts[EntityType.PRIZE_BOX],
            _ => 0
        };
    }

    public ClickDataRecorder()
    {
        _squareEntityClickCounts = new Godot.Collections.Dictionary<EntityType, int>
        {
            {EntityType.SQUARE, 0},
            {EntityType.BAD_BLOCK, 0},
            {EntityType.PRIZE_BOX, 0},
        };
        _missedClickCount = 0;
    }

    public void RecordSquareEntityClick(EntityType type) =>
        _squareEntityClickCounts[type]++;
    public void RecordMissedClick() => _missedClickCount++;
}