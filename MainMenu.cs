using System;
using Godot;

namespace JudgementDay;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		GetNode<Button>("Button").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scenes/MainScene.tscn");
		};
		int sum = 0;
		foreach (int weight in DecisionManager.DecisionWeights.Values)
		{
			sum += weight;
		}
		GD.Print($"good/bad difference: {sum}");
	}
}
