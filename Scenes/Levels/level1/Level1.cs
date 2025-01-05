using System.Collections.Generic;
using Godot;

public partial class Level1 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 1");

		SquareTimer.WaitTime = 1;

		SetSquaresForLevel();
	}

	protected override void SetSquaresForLevel()
	{
		ConfigureSquares(
			new SquareConfiguration(SquareEntityType.SQUARE, 0.5f),
			new SquareConfiguration(SquareEntityType.ICE_SQUARE, 0.5f)
		);
	}

}