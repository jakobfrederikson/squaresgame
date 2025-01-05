using Godot;
using System;
using System.Collections.Generic;

public partial class UpgradesMenu : Control
{
	[Export] private GridContainer gridContainer;
	[Export] private PackedScene upgradeCardScene;

	private UpgradeEntityManager _upgradeEntityManager;

	public override void _Ready()
	{
		_upgradeEntityManager = new UpgradeEntityManager();
		LoadUpgrades();
	}

	private void LoadUpgrades()
	{
		List<UpgradeData> allUpgrades = _upgradeEntityManager.GetAllUpgrades();

		if (allUpgrades == null || allUpgrades.Count == 0)
		{
			GD.PrintErr("No upgrades found to display in the menu.");
			return;
		}

		foreach (var upgradeData in allUpgrades)
		{
			if (upgradeData == null)
			{
				GD.PrintErr("Encountered a null UpgradeData entry.");
				continue;
			}

			UpgradeCard upgradeCard = (UpgradeCard)upgradeCardScene.Instantiate();
			upgradeCard.Initialize(upgradeData);
			gridContainer.AddChild(upgradeCard);
		}
	}

	public void OnStartMenuButtonPressed()
	{
		PackedScene scene = PackedSceneLoader.Get(ScenePath.START_MENU);
		GetTree().ChangeSceneToPacked(scene);
	}
}
