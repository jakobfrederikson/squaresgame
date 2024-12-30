using System.Collections.Generic;
using Godot;

public partial class Level2 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 2");

		SquareTimer.WaitTime = 0.5;

		int totalSpawns = (int)(GetLevelDuration() / SquareTimer.WaitTime);
		var entityInfos = new List<SquareEntityInfo>
		{
			new SquareEntityInfo { Type = EntityType.SQUARE, SpawnProbability = 0.5f, Scene = SquareScene},
			new SquareEntityInfo { Type = EntityType.BAD_BLOCK, SpawnProbability = 0.25f, Scene = BadBlockScene},
			new SquareEntityInfo { Type = EntityType.PRIZE_BOX, SpawnProbability = 0.25f, Scene = PrizeBoxScene}
		};
		_spawner.PrecalculateSpawns(totalSpawns, entityInfos);
	}
}
