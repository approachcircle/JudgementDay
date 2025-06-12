using System.Linq;
using System.Threading;
using Godot;

namespace JudgementDay;

// massively overengineered code incoming
public partial class SplashScreen : Control
{
	// if the animation has finished
	private bool _ready;
	// if the thread has finished its work and the game is "loaded"
	private bool _canTransfer;
	private Thread _initialisationThread;
	private AnimatedSprite2D _loadingSprite;
	private AnimatedSprite2D _splashSprite;
	
	public override void _Ready()
	{
		if (DecisionManager.DecisionWeights.Count != DecisionManager.DecisionRarities.Length)
		{
			GD.PushError("decision weights and rarities are not the same length!");
			System.Environment.Exit(1);
		}
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

	private void InitialiseGame()
	{
		CheckDecisionDifference();
        LoadCharacterTextures();
	}

	private static void LoadCharacterTextures()
	{
		for (int i = 1; i < State.CharacterCount + 1; i++)
		{
			State.CharacterTextures[i - 1] = GD.Load<Texture2D>($"res://assets/chars/character{i}.png");
		}
	}

	private static void CheckDecisionDifference()
	{
		int sum = DecisionManager.DecisionWeights.Values.Sum();

		switch (sum)
		{
			case > 0:
				GD.PushWarning($"decisions are in favour of good! +{sum}");
				break;
			case < 0:
				GD.PushWarning($"decisions are in favour of bad! +{sum}");
				break;
		}
	}
}
