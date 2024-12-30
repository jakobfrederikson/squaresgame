using Godot;
using System;

public partial class UpgradesMenu : Control
{
	[Export] private GridContainer gridContainer;
	[Export] private PackedScene upgradeCardScene;

	public override void _Ready()
	{
		LoadUpgrades();
	}

	private void LoadUpgrades()
	{
		string upgradesPath = "res://Resources/UpgradeData/";
		var dir = DirAccess.Open(upgradesPath);

		if (dir == null)
			GD.PrintErr($"Failed to open upgrades directory: {dir}");

		dir.ListDirBegin();
		while (true)
		{
			string folderName = dir.GetNext();
			if (folderName == "") break;

			string upgradePath = upgradesPath + $"{folderName}";
			GD.Print(upgradePath);
			var upgradeData = ResourceLoader.Load<UpgradeData>(upgradePath);

			UpgradeCard upgradeCard = (UpgradeCard)upgradeCardScene.Instantiate();
			upgradeCard.Initialize(upgradeData);

			gridContainer.AddChild(upgradeCard);
		}
		dir.ListDirEnd();
	}

	public void OnStartMenuButtonPressed()
	{
		PackedScene scene = PackedSceneLoader.Get(ScenePath.START_MENU);
		GetTree().ChangeSceneToPacked(scene);
	}
}
