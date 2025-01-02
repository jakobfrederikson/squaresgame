using System.Collections.Generic;
using Godot;

public partial class Level1 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 1");

		SquareTimer.WaitTime = 1;

		int totalSpawns = (int)(GetLevelDuration() / SquareTimer.WaitTime);
		var entityInfos = new List<SquareEntityInfo>
		{
			new SquareEntityInfo { Type = EntityType.SQUARE, SpawnProbability = 0.50f, Scene = SquareScene },
			new SquareEntityInfo { Type = EntityType.ICE_SQUARE, SpawnProbability = 0.50f, Scene = IceSquareScene }
		};
		_spawner.PrecalculateSpawns(totalSpawns, entityInfos);
	}
}