using System.Collections.Generic;
using Godot;

public partial class Level3 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 3");

		SquareTimer.WaitTime = 0.25;

		int totalSpawns = (int)(GetLevelDuration() / SquareTimer.WaitTime);
		var entityInfos = new List<SquareEntityInfo>
		{
			new SquareEntityInfo { Type = EntityType.SQUARE, SpawnProbability = 0.3f, Scene = SquareScene},
			new SquareEntityInfo { Type = EntityType.BAD_BLOCK, SpawnProbability = 0.2f, Scene = BadBlockScene},
			new SquareEntityInfo { Type = EntityType.PRIZE_BOX, SpawnProbability = 0.5f, Scene = PrizeBoxScene}
		};
		_spawner.PrecalculateSpawns(totalSpawns, entityInfos);
	}
}
