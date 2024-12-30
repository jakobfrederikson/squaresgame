using Godot;

public partial class Level1 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 1");

		SquareTimer.WaitTime = 1;

		int totalSpawns = (int)(GetLevelDuration() / SquareTimer.WaitTime);
		_spawner.PrecalculateSpawns(totalSpawns, 1.0f, 0.0f, 0.0f);
	}
}