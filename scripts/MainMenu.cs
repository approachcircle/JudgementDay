using Godot;

namespace JudgementDay;

public partial class MainMenu : Control
{
	private AudioStreamPlayer _gavelSound;
	private AnimatedSprite2D _logoAnimation;
	private bool _gavelplayed = false;
	
	public override void _Ready()
	{
		GetNode<Button>("Button").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scenes/MainScene.tscn");
		};
		_gavelSound = GetNode<AudioStreamPlayer>("Gavel");
		AudioStreamPlayer player = GetNode<AudioStreamPlayer>("Player");
		player.Stream = GD.Load<AudioStreamWav>("res://assets/menu_loop.wav");
		player.Finished += () => { player.Play(); };
		player.Play();
		_logoAnimation = GetNode<AnimatedSprite2D>("GavelAnimation");
		_logoAnimation.Play();
	}

	public override void _Process(double delta)
	{
		if (_logoAnimation.Frame > 15 && !_gavelplayed)
		{
			_gavelSound.Play();
			_gavelplayed = true;
		}
	}
}
