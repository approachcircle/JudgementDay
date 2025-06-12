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
		AudioStreamPlayer player = GetNode<AudioStreamPlayer>("Player");
		player.Stream = GD.Load<AudioStreamWav>("res://assets/menu_loop.wav");
		player.Finished += () => { player.Play(); };
		player.Play();
	}
}
