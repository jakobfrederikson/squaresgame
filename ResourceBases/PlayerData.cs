using Godot;
using System;

[Tool]
public sealed partial class PlayerData : Resource
{
	// General Player Data 
	[Export] public int TotalPoints;
	[Export] public int Points { get; private set; }
	[Export(PropertyHint.File, "*.tres")] public Godot.Collections.Array<UpgradeData> Upgrades;

	// Clicking stats
	[Export] public int TotalSquaresManuallyClicked;
	[Export] public int TotalSquaresClickedByUpgrade;
	[Export] public int TotalClickOnSquareAccuracy;
	[Export] public Godot.Collections.Dictionary ClickOnSquareAccuracyByLevel = new Godot.Collections.Dictionary();

	public PlayerData() : this(0, null, 0, 0, 0, null) { }

	public PlayerData(int points, Godot.Collections.Array<UpgradeData> upgrades,
		int mSquaresClicked, int upgSquaresClicked, int totalClickAccuracy,
		Godot.Collections.Dictionary accByLevel)
	{
		Points = points;
		Upgrades = upgrades;
		TotalSquaresManuallyClicked = mSquaresClicked;
		TotalSquaresClickedByUpgrade = upgSquaresClicked;
		TotalClickOnSquareAccuracy = totalClickAccuracy;
		ClickOnSquareAccuracyByLevel = accByLevel;
	}

	public void GivePoints(int points)
	{
		this.Points += points;
		TotalPoints += points;
	}

	public void RemovePoints(int points)
	{
		this.Points -= points;
	}

	public void ResetStats()
	{
		TotalPoints = 0;
		Points = 0;
		TotalSquaresManuallyClicked = 0;
		TotalSquaresClickedByUpgrade = 0;
		TotalClickOnSquareAccuracy = 0;
		ClickOnSquareAccuracyByLevel.Clear();
		if (Upgrades != null)
		{
			foreach (var upgrade in Upgrades)
			{
				upgrade.IsEnabled = false; // Disable all upgrades
				upgrade.UpgradeStage = 0; // Reset upgrade stages
			}
		}
		SaveData.SavePlayerData(this);
	}
}
