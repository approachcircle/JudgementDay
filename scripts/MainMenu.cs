using Godot;

namespace JudgementDay;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		// we don't want to lazy-load the main scene as it feels janky for the game to freeze when pressing play
		PackedScene mainScene = GD.Load<PackedScene>("res://scenes/MainScene.tscn");
		GetNode<Button>("Button").Pressed += () =>
		{
			GetTree().ChangeSceneToPacked(mainScene);
		};
		AudioStreamPlayer player = GetNode<AudioStreamPlayer>("Player");
		player.Stream = GD.Load<AudioStreamWav>("res://assets/menu_loop.wav");
		player.Finished += () => { player.Play(); };
		player.Play();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("exit")) GetTree().Quit();
	}
}
