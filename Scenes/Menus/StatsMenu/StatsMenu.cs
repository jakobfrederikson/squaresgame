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
		PointsLabel.Text = Convert.ToString(playerData.Points);
		TotalPointsLabel.Text = Convert.ToString(playerData.TotalPoints);
		InitialiseUpgrades();
		SquaresClickedLabel.Text = Convert.ToString(playerData.TotalSquaresManuallyClicked);
		UprgSquaresClickedLabel.Text = Convert.ToString(playerData.TotalSquaresClickedByUpgrade);
		AccuracyLabel.Text = Convert.ToString(playerData.TotalClickOnSquareAccuracy);
		InitialiseAccuracyByLevel();
	}

	private void InitialiseUpgrades()
	{
		if (playerData.Upgrades.Count() == 0)
		{
			var noUpgradesLabel = new Label();
			noUpgradesLabel.Text = "No Upgrades yet.";
			UpgradesGridContainer.AddChild(noUpgradesLabel);
			return;
		}

		foreach (var upgrade in playerData.Upgrades)
		{
			var UpgradeVBox = new VBoxContainer();
			var UpgradeTitleLabel = new Label();
			var UpgradeDescriptionLabel = new Label();
			var UpgradeStage = new Label();

			UpgradeTitleLabel.Text = upgrade.Name;
			UpgradeDescriptionLabel.Text = upgrade.Description;
			UpgradeStage.Text = "Stage: " + Convert.ToString(upgrade.UpgradeStage);

			UpgradesGridContainer.AddChild(UpgradeVBox);
			UpgradeVBox.AddChild(UpgradeTitleLabel);
			UpgradeVBox.AddChild(UpgradeDescriptionLabel);
			UpgradeVBox.AddChild(UpgradeStage);
		}
	}

	private void InitialiseAccuracyByLevel()
	{
		if (playerData.ClickOnSquareAccuracyByLevel.Count == 0)
		{
			var noAccuracyLabel = new Label();
			noAccuracyLabel.Text = "You have not played any levels yet.";
			AccuracyByLevelContainer.AddChild(noAccuracyLabel);
			return;
		}

		foreach (var entry in playerData.ClickOnSquareAccuracyByLevel)
		{
			var LevelHBox = new HBoxContainer();
			var LevelNameLabel = new Label();
			var GapSpacer = new Control();
			GapSpacer.SizeFlagsHorizontal = SizeFlags.Expand;
			var LevelAccuracyLabel = new Label();

			// Populate labels with the level name and accuracy
			LevelNameLabel.Text = $"Level: {entry.Key}";
			LevelAccuracyLabel.Text = $"Accuracy: {entry.Value}%";

			// Add the VBoxContainer to the AccuracyGridContainer
			AccuracyByLevelContainer.AddChild(LevelHBox);
			LevelHBox.AddChild(LevelNameLabel);
			LevelHBox.AddChild(GapSpacer);
			LevelHBox.AddChild(LevelAccuracyLabel);
		}
	}

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
		PointsLabel.Text = Convert.ToString(playerData.Points);
		TotalPointsLabel.Text = Convert.ToString(playerData.TotalPoints);
	}

	private void MainMenuButtonPressed()
		=> GetTree().ChangeSceneToPacked(PackedSceneLoader.Get(ScenePath.START_MENU));
}
