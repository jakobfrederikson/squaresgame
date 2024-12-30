using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Export] private Label _timeLabel;
	[Export] private Label _squaresClickedLabel;
	[Export] private Label _accuracyLabel;
	[Export] private Label _scoreLabel;

	private int _score;
	private int _totalClicks;
	private int _goodSquaresClicked;

	public override void _Ready()
	{
		_timeLabel.Text = "00:00";
		_squaresClickedLabel.Text = "00 / 00";
		_accuracyLabel.Text = "0.00%";
		_scoreLabel.Text = "0";

		_score = 0;
		_totalClicks = 0;
		_goodSquaresClicked = 0;
	}

	internal void UpdateTime(int minute, int second) =>
		_timeLabel.Text = string.Format("{0:D2}:{1:D2}", minute, second);

	internal void UpdateScore(int score)
	{
		_score = score;
		_scoreLabel.Text = Convert.ToString(_score);
		_goodSquaresClicked += 1;
		_totalClicks += 1;
		UpdatePoints();
		UpdateAccuracy();
	}

	private void UpdatePoints() =>
		_squaresClickedLabel.Text = $"{_goodSquaresClicked} / {_totalClicks}";

	internal void UpdateAccuracy()
	{
		if (_totalClicks > 0)
		{
			double accuracy = (double)_goodSquaresClicked / _totalClicks * 100;
			_accuracyLabel.Text = $"{accuracy:0.00}%";
		}
		else
		{
			_accuracyLabel.Text = "0.00%";
		}
	}

	internal void MissClicked()
	{
		_totalClicks += 1;
		UpdateAccuracy();
		UpdatePoints();
	}
}
