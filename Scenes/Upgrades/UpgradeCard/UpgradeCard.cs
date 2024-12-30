using Godot;
using System;

public partial class UpgradeCard : PanelContainer
{
	[Export] private Button button;
	[Export] private PlayerData playerData;

	private UpgradeData upgradeData;

	public void Initialize(UpgradeData data)
	{
		upgradeData = data;

		button.Text = upgradeData.IsBought
		? $"{upgradeData.Name}\n{(upgradeData.IsEnabled ? "Enabled" : "Disabled")}"
		: $"{upgradeData.Name} ({upgradeData.Cost})";
		button.Icon = GD.Load<Texture2D>(upgradeData.TexturePath);

		if (upgradeData.IsBought)
		{
			button.Disabled = false;
			button.Pressed += ToggleUpgradeState;
		}
		else
		{
			button.Disabled = upgradeData.Cost > playerData.Points;
			if (!button.Disabled)
			{
				button.Pressed += BuyUpgrade;
			}
		}
	}

	private void ToggleUpgradeState()
	{
		upgradeData.IsEnabled = !upgradeData.IsEnabled;
		button.Text = $"{upgradeData.Name}\n{(upgradeData.IsEnabled ? "Enabled" : "Disabled")}";
		GD.Print($"{upgradeData.Name} is now {(upgradeData.IsEnabled ? "enabled" : "disabled")}.");
		SaveData.SaveUpgradeData(upgradeData);
	}

	private void BuyUpgrade()
	{
		if (playerData.Points >= upgradeData.Cost)
		{
			// Upgrade data stuff
			upgradeData.IsBought = true;
			button.Pressed -= BuyUpgrade;
			button.Pressed += ToggleUpgradeState;
			ToggleUpgradeState();

			// Player data stuff
			playerData.RemovePoints(upgradeData.Cost);
			playerData.Upgrades.Add(upgradeData);

			GD.Print($"Bought {upgradeData.Name}. Remaining points: {playerData.Points}");
			SaveData.SaveUpgradeData(upgradeData);
			SaveData.SavePlayerData(playerData);
		}
		else
		{
			GD.Print($"Not enough points to buy {upgradeData.Name}.");
		}
	}
}
