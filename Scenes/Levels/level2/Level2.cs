using Godot;
using System;

public partial class Level2 : Level
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("Hello Level 2");

		SquareTimer.WaitTime = 0.5;
	}

	public override void _Process(double delta)
	{
		CheckGameEnd();
	}

	private void CheckGameEnd()
	{
		if (_squaresClicked == 20)
			GetTree().ChangeSceneToPacked(PackedSceneLoader.Get(ScenePath.LEVEL_OVER_SCENE));
	}

	protected override void SpawnSquareEntity()
	{
		base.SpawnSquareEntity();
		// If random is above 0.5, proceed
		// If random is above 0.75, proceed to spawn BadBlock
		// If random is lower than 0.75, proceed to spawn PrizeBox

		var rng = new RandomNumberGenerator();

		float randomValue = rng.Randf();

		if (randomValue > 0.50f)
		{
			if (randomValue < 0.75f)
			{
				SpawnBadBlock();
			}
			else if (randomValue > 0.75f)
			{
				SpawnPrizeBox();
			}
		}
	}

	private void SpawnBadBlock()
	{
		var badBlock = BadBlockScene.Instantiate<BadBlock>();
		AddChild(badBlock);

		_missedClickHandler.RegisterSquareEntity(badBlock);
		// badBlock.Clicked += OnBadBlockClicked -> Possibly -1 point.

		var viewportRect = GetViewport().GetVisibleRect();
		var rng = new RandomNumberGenerator();
		float x = rng.RandfRange(viewportRect.Position.X + 25, viewportRect.Position.X + viewportRect.Size.X - badBlock.Size().X - 25);
		float y = rng.RandfRange(viewportRect.Position.Y + 25, viewportRect.Position.Y + viewportRect.Size.Y - badBlock.Size().Y - 25);

		badBlock.Position = new Godot.Vector2(x, y);

		GD.Print("Spawned a Bad Block!");
	}

	private void SpawnPrizeBox()
	{
		var prizeBox = PrizeBoxScene.Instantiate<PrizeBox>();
		AddChild(prizeBox);

		_missedClickHandler.RegisterSquareEntity(prizeBox);
		// prizeBox.Clicked += OnPrizeBoxClicked -> Possibly +2 points.

		var viewportRect = GetViewport().GetVisibleRect();
		var rng = new RandomNumberGenerator();
		float x = rng.RandfRange(viewportRect.Position.X + 25, viewportRect.Position.X + viewportRect.Size.X - prizeBox.Size().X - 25);
		float y = rng.RandfRange(viewportRect.Position.Y + 25, viewportRect.Position.Y + viewportRect.Size.Y - prizeBox.Size().Y - 25);

		prizeBox.Position = new Godot.Vector2(x, y);

		GD.Print("Spawned a Prize Box!");
	}
}
