using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Export] private Label timeLabel;
	[Export] private Label squaresClickedLabel;
	[Export] private Label accuracyLabel;

	private int _squaresClicked;
	private int _totalClicks;

	public override void _Ready()
	{
		timeLabel.Text = "00:00";
		squaresClickedLabel.Text = "0";
		accuracyLabel.Text = "0.00%";

		_squaresClicked = 0;
		_totalClicks = 0;
	}

	internal void UpdateTime(int minute, int second) =>
		timeLabel.Text = string.Format("{0:D2}:{1:D2}", minute, second);

	internal void SquareClicked()
	{
		_squaresClicked += 1;
		_totalClicks += 1;
		UpdatePoints();
		UpdateAccuracy();
	}

	private void UpdatePoints() =>
		squaresClickedLabel.Text = Convert.ToString(_totalClicks);

	internal void UpdateAccuracy()
	{
		if (_totalClicks > 0)
		{
			double accuracy = (double)_squaresClicked / _totalClicks * 100;
			accuracyLabel.Text = $"{accuracy:0.00}%";
		}
		else
			accuracyLabel.Text = "0.00%";
	}

	internal void MissClicked()
	{
		_totalClicks += 1;
		UpdateAccuracy();
		UpdatePoints();
	}
}
