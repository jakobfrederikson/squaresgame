using System.Collections.Generic;
using Godot;

public partial class Level3 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 3");

		SquareTimer.WaitTime = 0.25;

		SetSquaresForLevel();
	}

	protected override void SetSquaresForLevel()
	{
		ConfigureSquares(
			new SquareConfiguration(SquareEntityType.SQUARE, 0.3f),
			new SquareConfiguration(SquareEntityType.BAD_BLOCK, 0.2f),
			new SquareConfiguration(SquareEntityType.PRIZE_BOX)
		);
	}
}
