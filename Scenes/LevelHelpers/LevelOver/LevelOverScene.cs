using Godot;
using System;

public partial class LevelOverScene : Control
{

	[Export] private Button _mainMenuButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainMenuButton.Pressed += BackToMainMenu;
	}

	private void BackToMainMenu() =>
		GetTree().ChangeSceneToPacked(PackedSceneLoader.Get(ScenePath.LEVEL_SELECT));
}
