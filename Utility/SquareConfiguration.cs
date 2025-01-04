public class SquareConfiguration
{
    public SquareEntityType Type { get; }
    public float? Percentage { get; }

    public SquareConfiguration(SquareEntityType type, float? percentage = null)
    {
        Type = type;
        Percentage = percentage;
    }
}