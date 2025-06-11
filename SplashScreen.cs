using Godot;

namespace JudgementDay;

public partial class SplashScreen : Control
{
	public override void _Ready()
	{
		GetNode<VideoStreamPlayer>("VideoStreamPlayer").Finished += Transfer;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_accept")) Transfer();
	}

	private void Transfer()
	{
		GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
	}   
}
