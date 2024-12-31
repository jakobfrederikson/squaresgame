using Godot;
using System;

public enum LevelOverType
{
	TimeUp,
	Won,
	NoMoreSquares
}

public partial class LevelOverScene : Control
{
	[Export] private Button _mainMenuButton;

	public override void _Ready()
	{
		_mainMenuButton.Pressed += BackToMainMenu;
	}

	private void BackToMainMenu() =>
		GetTree().ChangeSceneToPacked(PackedSceneLoader.Get(ScenePath.LEVEL_SELECT));

	public void SetEndReason(LevelOverType levelOverType)
	{
		GD.Print("Level is over because " + levelOverType);
	}
}
