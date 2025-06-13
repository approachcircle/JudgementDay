using Godot;

namespace JudgementDay;

public partial class MainMenu : Control
{
	public static PackedScene MainScene { get; set; } = null!;
	public override void _Ready()
	{
		// we don't want to lazy-load the main scene as it feels janky for the game to freeze when pressing play
		GetNode<Button>("Play").Pressed += () =>
		{
			GetTree().ChangeSceneToPacked(MainScene);
		};
		GetNode<Button>("Tutorial").Pressed += () =>
		{
			LessonState.CurrentLesson = 1;
			GetTree().ChangeSceneToFile("res://scenes/lessons/Lesson1.tscn");
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
