using Godot;
using System;

public partial class LevelCard : Control
{
	[Export] private TextureRect levelIconTextureRect;
	[Export] private Label levelTitleLabel;
	[Export] private Label levelDescriptionLabel;
	[Export] private Label levelDurationLabel;
	[Export] private Label levelPointsLabel;
	[Export] private Button buyLevelButton;
	[Export] private Button playLevelButton;

	private string scenePath;
	private PlayerData _playerData;
	private LevelData _levelData;
	private string ResourcePath;

	public void Initialize(LevelData levelData, PlayerData playerData)
	{
		GD.Print("Level Card Initialise for " + levelData.Name);
		GD.Print("LevelData null: " + levelData.PathToScene);
		_levelData = levelData;
		levelIconTextureRect.Texture =
			GD.Load<Texture2D>(levelData.TexturePath);
		_playerData = playerData;
		if (_playerData != null) GD.Print("_playerData successfully loaded.");
		levelTitleLabel.Text = levelData.Name;
		levelDescriptionLabel.Text = levelData.Description;
		levelDurationLabel.Text = "Time: " + Convert.ToString(levelData.RoundDuration);
		levelPointsLabel.Text = Convert.ToString(levelData.ScoreToWin);

		if (levelData.Bought == true)
		{
			EnablePlayButton();
		}
		else
		{
			EnableBuyButton();
		}

		scenePath = levelData.PathToScene;
	}

	private void OnPlayButtonPressed()
	{
		GD.Print($"Changing scene to file {scenePath}.");

		if (_levelData == null)
		{
			GD.PrintErr("LevelData is not initialized!");
			return;
		}

		GetTree().ChangeSceneToFile(scenePath);
	}

	private void OnBuyLevelButtonPressed()
	{
		GD.Print($"Buying level {levelTitleLabel.Text}");
		EnablePlayButton();

		_levelData.Bought = true;
		SaveData.SaveLevelData(_levelData);

		_playerData.RemovePoints(_levelData.Cost);
		SaveData.SavePlayerData(_playerData);
	}

	// Disable the Buy button and enable the Play button
	private void EnablePlayButton()
	{
		// Enable Play button
		playLevelButton.Disabled = false;
		playLevelButton.Pressed += OnPlayButtonPressed;
		playLevelButton.Visible = true;

		// Disable Buy button
		buyLevelButton.Disabled = true;
		buyLevelButton.Visible = false;
	}

	// Disable the Play button and enable the Buy button
	private void EnableBuyButton()
	{
		// Disable play button
		playLevelButton.Disabled = true;
		playLevelButton.Visible = false;

		// Enable Buy Button
		if (_playerData.Points >= _levelData.Cost)
		{
			buyLevelButton.Disabled = false;
			buyLevelButton.Pressed += OnBuyLevelButtonPressed;
		}
		else
			buyLevelButton.Disabled = true;

		buyLevelButton.Text = "Buy Level (" + Convert.ToString(_levelData.Cost) + ")";
		buyLevelButton.Visible = true;
	}
}
