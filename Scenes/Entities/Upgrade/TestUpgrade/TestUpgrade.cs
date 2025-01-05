using Godot;
using System;

public partial class TestUpgrade : UpgradeEntity
{
	public override void _Ready()
	{
		GD.Print("Hello from Test Upgrade _Ready().");
	}

	public override void ApplyEffect()
	{
		GD.Print("Hello from ApplyEffect in TestUpgrade.");
	}
}
