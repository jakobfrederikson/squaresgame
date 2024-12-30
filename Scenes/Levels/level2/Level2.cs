using Godot;
using System;

public partial class Level2 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 2");

		SquareTimer.WaitTime = 0.5;

		int totalSpawns = (int)(GetLevelDuration() / SquareTimer.WaitTime);
		_spawner.PrecalculateSpawns(totalSpawns, 0.5f, 0.25f, 0.25f);
	}
}
