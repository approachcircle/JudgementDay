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
	}
}
