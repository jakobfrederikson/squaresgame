using Godot;

public partial class Level1 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 1");

		SquareTimer.WaitTime = 1;

		Initialize();
	}

	public override void _Process(double delta)
	{
		CheckGameEnd();
	}

	public void Initialize()
	{

	}

	private void CheckGameEnd()
	{
		if (_squaresClicked == 10)
			GetTree().ChangeSceneToPacked(PackedSceneLoader.Get(ScenePath.LEVEL_OVER_SCENE));
	}
}