using Godot;
using System;

public enum LevelOverType
{
	TimeUp,
	Won,
	NoMoreSquares
}

public partial class LevelOverScene : Control
{
	[Export] private Button _mainMenuButton;
	[Export] private Label _roundOverMessageLabel;
	[Export] private Label _scoreLabel;
	[Export] private Label _playerTotalScoreLabel;
	[Export] private Label _totalLevelClicksLabel;
	[Export] private Label _normalSquaresClickedLabel;
	[Export] private Label _prizeBoxSquaresClickedLabel;
	[Export] private Label _badBlockSquaresClickedLabel;
	[Export] private Label _missedClicksLabel;
	[Export] private Label _roundTimeTakenLabel;

	private LevelOverType _levelOverType;

	public override void _Ready()
	{
		_mainMenuButton.Pressed += BackToMainMenu;
		_levelOverType = RoundEndData.Instance.LevelOverType;

		Initialise();
	}

	private void Initialise()
	{
		SetRoundOverMessage();
		_scoreLabel.Text = $"{RoundEndData.Instance.Score}";
		_playerTotalScoreLabel.Text = $"{RoundEndData.Instance.TotalPoints}";
		_totalLevelClicksLabel.Text = $"{RoundEndData.Instance.TotalClicks}";
		_normalSquaresClickedLabel.Text = $"{RoundEndData.Instance.NormalSquareClicks}";
		_prizeBoxSquaresClickedLabel.Text = $"{RoundEndData.Instance.PrizeBoxClicks}";
		_badBlockSquaresClickedLabel.Text = $"{RoundEndData.Instance.BadBlockClicks}";
		_missedClicksLabel.Text = $"{RoundEndData.Instance.MissedClicks}";
		_roundTimeTakenLabel.Text = $"{RoundEndData.Instance.TimeTakenToFinishLevel}";
	}

	private void SetRoundOverMessage()
	{
		switch (_levelOverType)
		{
			case LevelOverType.Won:
				_roundOverMessageLabel.Text = "Level complete. You got the score needed to beat this level.";
				break;
			case LevelOverType.TimeUp:
				_roundOverMessageLabel.Text = "Time is up, you didn't complete the level in time!";
				break;
			case LevelOverType.NoMoreSquares:
				_roundOverMessageLabel.Text = "No more squares are available to spawn! You lose.";
				break;
			default:
				_roundOverMessageLabel.Text = "Unhandled winning reason... good job, I guess.";
				break;
		}
	}

	private void BackToMainMenu() =>
		GetTree().ChangeSceneToPacked(PackedSceneLoader.Get(ScenePath.LEVEL_SELECT));
}
