using Godot;
using System;

[Tool]
public sealed partial class PlayerData : Resource
{
	// General Player Data 
	[Export] public int TotalPoints;
	[Export] public int CurrentPoints { get; private set; }
	[Export(PropertyHint.File, "*.tres")] public Godot.Collections.Array<UpgradeData> Upgrades;

	// Clicking stats
	[Export] public int LifeTimeClicks;
	[Export] public int LifeTimeSquaresClicked;
	[Export] public int LifeTimeSquaresManuallyClicked;
	[Export] public int LifeTimeSquaresClickedByUpgrade;
	[Export] public int LifeTimeGoodSquaresClicked;
	[Export] public int LifeTimeBadSquaresClicked;
	[Export] public int LifeTimeMissedClicks;
	[Export] public double LifeTimeOverallAccuracy { get; private set; }
	//[Export] public Godot.Collections.Dictionary LifeTimeStatsPerLevel;

	public PlayerData()
	{
		Upgrades = new Godot.Collections.Array<UpgradeData>();
		//LifeTimeStatsPerLevel = new Godot.Collections.Dictionary();
	}

	public void GivePoints(int points)
	{
		CurrentPoints += points;
		TotalPoints += points;
	}

	public void RemovePoints(int points)
	{
		CurrentPoints -= points;
	}

	public void UpdateLevelStats(LevelData levelData)
	{
		LifeTimeSquaresClicked += levelData.TotalSquaresClicked;
		LifeTimeGoodSquaresClicked += levelData.GoodSquareClicks;
		LifeTimeBadSquaresClicked += levelData.BadBlockClicks;
		LifeTimeMissedClicks += levelData.MissedClicks;
		LifeTimeClicks += levelData.TotalClicks;
		LifeTimeOverallAccuracy = (double)LifeTimeGoodSquaresClicked / LifeTimeClicks * 100;

		SaveData.SavePlayerAndLevelData(this, levelData);
	}

	public void ResetStats()
	{
		TotalPoints = 0;
		CurrentPoints = 0;
		LifeTimeClicks = 0;
		LifeTimeSquaresClicked = 0;
		LifeTimeSquaresManuallyClicked = 0;
		LifeTimeSquaresClickedByUpgrade = 0;
		LifeTimeGoodSquaresClicked = 0;
		LifeTimeBadSquaresClicked = 0;
		LifeTimeMissedClicks = 0;
		LifeTimeOverallAccuracy = 0;

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
