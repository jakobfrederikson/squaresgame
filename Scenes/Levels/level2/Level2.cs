using System.Collections.Generic;
using Godot;

public partial class Level2 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 2");

		SquareTimer.WaitTime = 0.5;
	}

	protected override void SetSquaresForLevel()
	{
		ConfigureSquares(
			new SquareConfiguration(SquareEntityType.SQUARE, 0.5f),
			new SquareConfiguration(SquareEntityType.BAD_BLOCK, 0.25f),
			new SquareConfiguration(SquareEntityType.PRIZE_BOX)
		);
	}
}
