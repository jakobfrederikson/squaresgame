using Godot;
using System;

public partial class StartMenu : Control
{
	public override void _Ready()
	{
		base._Ready();
	}

	public void OnLevelSelectButtonPressed()
	{
		PackedScene scene = PackedSceneLoader.Get(ScenePath.LEVEL_SELECT);
		GetTree().ChangeSceneToPacked(scene);
	}

	public void OnUpgradesButtonPressed()
	{
		PackedScene scene = PackedSceneLoader.Get(ScenePath.UPGRADES);
		GetTree().ChangeSceneToPacked(scene);
	}

	public void OnStatsButtonPressed()
	{
		PackedScene scene = PackedSceneLoader.Get(ScenePath.STATS);
		GetTree().ChangeSceneToPacked(scene);
	}

	public void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
