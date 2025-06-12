using System.Threading;
using Godot;

namespace JudgementDay;

// massively overengineered code incoming
public partial class SplashScreen : Control
{
	// if the animation has finished
	private bool _ready = false;
	// if the thread has finished its work and the game is "loaded"
	private bool _canTransfer = false;
	private Thread _initialisationThread;
	private AnimatedSprite2D _loadingSprite;
	private AnimatedSprite2D _splashSprite;
	
	public override void _Ready()
	{
		_loadingSprite = GetNode<AnimatedSprite2D>("Loading");
		_splashSprite = GetNode<AnimatedSprite2D>("Splash");
		_initialisationThread = new Thread(InitialiseGame);
		_initialisationThread.Start();
		_splashSprite.AnimationFinished += () =>
		{
			_ready = true;
		};
		_splashSprite.Play("splash");
	}

	public override void _Process(double delta)
	{
		// if the player is impatient, allow transfer as soon as the thread has finished its work
		if (Input.IsActionJustPressed("ui_accept") && _canTransfer) Transfer();
		_canTransfer = _initialisationThread.ThreadState is not ThreadState.WaitSleepJoin;
		// the thread is still doing work; not safe to transfer yet, so we'll show the player a spinner
		if (_ready && !_canTransfer)
		{
			if (!_loadingSprite.IsPlaying())
			{
				_loadingSprite.Visible = true;
				_loadingSprite.Play("spinner");
			}
		}
		// animation has finished, and the thread has finished its work; safe to transfer
		else if (_ready && _canTransfer)
		{
			Transfer();
		}
	}

	private void Transfer()
	{
		// this will block EVERYTHING until the thread has finished its work,
		_initialisationThread.Join();
		GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
	}

	public void InitialiseGame()
	{
		for (int i = 1; i < State.CharacterCount + 1; i++)
		{
			State.CharacterTextures[i - 1] = GD.Load<Texture2D>($"res://assets/chars/character{i}.png");
		}
	}
}
