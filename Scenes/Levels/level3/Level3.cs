using Godot;
using System;

public partial class Level3 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 3");

		SquareTimer.WaitTime = 0.25;
	}

	public override void _Process(double delta)
	{
		CheckGameEnd();
	}

	private void CheckGameEnd()
	{
		if (_squaresClicked == 50)
			GetTree().ChangeSceneToPacked(PackedSceneLoader.Get(ScenePath.LEVEL_OVER_SCENE));
	}
}
