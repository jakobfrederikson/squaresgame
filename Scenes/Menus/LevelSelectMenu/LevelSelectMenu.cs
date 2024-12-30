using Godot;
using System;

public partial class LevelSelectMenu : Control
{
	[Export] private NodePath levelCardContainerPath;
	[Export] private PackedScene levelCardScene;
	[Export(PropertyHint.Dir)] private string levelsDirPath;

	private GridContainer levelCardContainer;
	private PlayerData _playerData;

	public override void _Ready()
	{
		levelCardContainer = GetNode<GridContainer>(levelCardContainerPath);
		_playerData = (PlayerData)ResourceDataLoader.Get(ResourcePath.PLAYER_DATA);
		LoadLevels();
	}

	public void OnStartMenuButtonPressed()
	{
		PackedScene scenePath = PackedSceneLoader.Get(ScenePath.START_MENU);
		GetTree().ChangeSceneToPacked(scenePath);
	}

	/// <summary>
	/// Get a list of levels in the res://Levels/ directory and intialize a level card scene
	/// per level loaded in the level card scroll container. 
	/// </summary>
	private void LoadLevels()
	{
		// Dir is added via export variable, but append a '/' to make sure it's res://Levels/,
		// not res://Levels
		levelsDirPath = levelsDirPath + "/";

		// Open file stream
		var dir = DirAccess.Open(levelsDirPath);

		if (dir == null)
		{
			GD.PrintErr($"Failed to open levels directory: {levelsDirPath}");
		}

		dir.ListDirBegin();
		while (true)
		{
			string folderName = dir.GetNext();
			if (folderName == "") break;
			if (!dir.CurrentIsDir()) continue;

			// Get the path of the level scene.
			// res://Levels/'level1' etc.../
			string pathToLevelDir = $"{levelsDirPath}{folderName}/";
			string resourcePath = $"{pathToLevelDir}{folderName}_data.tres";


			// Load the LevelData resource file for the particular level
			// res://Levels/'level1' etc.../level1_data.tres
			var levelData = ResourceLoader.Load<LevelData>(resourcePath);

			GD.Print($"{levelData.Name} ResourcePath: {levelData.ResourcePath}");

			if (levelData == null)
			{
				GD.PrintErr($"LevelData node not found in scene: {pathToLevelDir}");
				continue;
			}

			// Instantiate a level card instance for the current level
			// Use LevelCard 'Initialize' method to initialize all labels/scene path.
			LevelCard levelCard = (LevelCard)levelCardScene.Instantiate();
			levelCard.Initialize(levelData, _playerData);

			// Add the level card to the scroll container
			levelCardContainer.AddChild(levelCard);
		}

		// Close file stream
		dir.ListDirEnd();
	}
}
