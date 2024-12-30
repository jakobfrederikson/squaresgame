using Godot;
using System;

public partial class Level3 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 3");

		SquareTimer.WaitTime = 0.25;

		int totalSpawns = (int)(GetLevelDuration() / SquareTimer.WaitTime);
		_spawner.PrecalculateSpawns(totalSpawns, 0.3f, 0.2f, 0.5f);
	}
}
