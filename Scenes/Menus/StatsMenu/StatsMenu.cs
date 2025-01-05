using Godot;
using System;
using System.Linq;

public partial class StatsMenu : Control
{
	[Export] private Button mainMenuButton;
	[Export] private Button resetStatsButton;
	[Export] private Button givePointsButton;
	[Export] private PlayerData playerData;

	[Export(PropertyHint.NodeType)] Label PointsLabel;
	[Export(PropertyHint.NodeType)] Label TotalPointsLabel;
	[Export(PropertyHint.NodeType)] GridContainer UpgradesGridContainer;
	[Export(PropertyHint.NodeType)] Label SquaresClickedLabel;
	[Export(PropertyHint.NodeType)] Label UprgSquaresClickedLabel;
	[Export(PropertyHint.NodeType)] Label AccuracyLabel;
	[Export] VBoxContainer AccuracyByLevelContainer;

	public override void _Ready()
	{
		givePointsButton.Pressed += GivePointsButtonPressed;
		resetStatsButton.Pressed += ResetStatsButtonPressed;
		mainMenuButton.Pressed += MainMenuButtonPressed;

		InitialiseStats();
	}

	private void InitialiseStats()
	{
		PointsLabel.Text = Convert.ToString(playerData.CurrentPoints);
		TotalPointsLabel.Text = Convert.ToString(playerData.TotalPoints);
		InitialiseUpgrades();
		SquaresClickedLabel.Text = Convert.ToString(playerData.LifeTimeSquaresClicked);
		UprgSquaresClickedLabel.Text = Convert.ToString(playerData.LifeTimeSquaresClickedByUpgrade);
		AccuracyLabel.Text = Convert.ToString(playerData.LifeTimeOverallAccuracy);
		InitialiseAccuracyByLevel();
	}

	private void InitialiseUpgrades()
	{
		UpgradeEntityManager upgradeDataManager = new();
		var upgrades = upgradeDataManager.GetAllUpgrades();
		System.Console.WriteLine("WRITING ALL UPGRADES NAMES	");
		foreach (var u in upgrades) Console.WriteLine(u.Name);
		if (upgrades.Count() == 0)
		{
			var noUpgradesLabel = new Label();
			noUpgradesLabel.Text = "No Upgrades yet.";
			UpgradesGridContainer.AddChild(noUpgradesLabel);
			return;
		}

		foreach (var upgrade in upgrades)
		{
			if (!upgrade.IsBought) return;

			var UpgradeVBox = new VBoxContainer();
			var UpgradeTitleLabel = new Label();
			var UpgradeDescriptionLabel = new Label();
			var UpgradeStage = new Label();

			UpgradeTitleLabel.Text = upgrade.Name;
			UpgradeDescriptionLabel.Text = upgrade.Description;
			UpgradeStage.Text = "Stage: " + Convert.ToString(upgrade.Stage);

			UpgradesGridContainer.AddChild(UpgradeVBox);
			UpgradeVBox.AddChild(UpgradeTitleLabel);
			UpgradeVBox.AddChild(UpgradeDescriptionLabel);
			UpgradeVBox.AddChild(UpgradeStage);
		}
	}

	private void InitialiseAccuracyByLevel() => throw new ArgumentException("Not yet implemented.");

	public void ResetStatsButtonPressed()
	{
		var dialog = new ConfirmationDialog();
		dialog.DialogText = "Are you sure you want to reset all stats?";
		dialog.GetOkButton().Pressed += PerformReset;
		AddChild(dialog);
		dialog.PopupCentered();
	}

	private void PerformReset()
	{
		playerData.ResetStats();
		InitialiseStats();
		GD.Print("Reset player data complete.");
	}

	private void GivePointsButtonPressed()
	{
		playerData.GivePoints(500);
		SaveData.SavePlayerData(playerData);
		PointsLabel.Text = Convert.ToString(playerData.CurrentPoints);
		TotalPointsLabel.Text = Convert.ToString(playerData.TotalPoints);
	}

	private void MainMenuButtonPressed()
		=> GetTree().ChangeSceneToPacked(PackedSceneLoader.Get(ScenePath.START_MENU));
}
