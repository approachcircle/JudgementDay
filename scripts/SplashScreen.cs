using System;
using System.Linq;
using System.Threading;
using Godot;

namespace JudgementDay;

// massively overengineered code incoming
// accounts for slow hard drives/network access
public partial class SplashScreen : Control
{
	// if the animation has finished
	private bool _ready;
	// if the thread has finished its work and the game is "loaded"
	private bool _canTransfer;
	private GodotThread _initialisationThread;
	private CanvasGroup _loadingGroup;
	private AnimatedSprite2D _loadingSprite;
	private AnimatedSprite2D _splashSprite;
	private PackedScene _nextScene;
	
	public override void _Ready()
	{
		if (DecisionManager.DecisionWeights.Count != DecisionManager.DecisionRarities.Length)
		{
			GD.PushError("decision weights and rarities are not the same length!");
			System.Environment.Exit(1);
		}
		_loadingGroup = GetNode<CanvasGroup>("Loading");
		_loadingSprite = _loadingGroup.GetNode<AnimatedSprite2D>("Loading");
		_splashSprite = GetNode<AnimatedSprite2D>("Splash");
		_initialisationThread = new GodotThread();
		_initialisationThread.Start(Callable.From(InitialiseGame));
		_splashSprite.AnimationFinished += () =>
		{
			_ready = true;
		};
		_splashSprite.Play("splash");
	}

	public override void _Process(double delta)
	{
		// if the player is impatient, allow transfer as soon as the thread has finished its work
		if (Input.IsActionJustPressed("ui_accept"))
		{
			_ready = true;
		}
		_canTransfer = !_initialisationThread.IsAlive();
		// the thread is still doing work; not safe to transfer yet, so we'll show the player a spinner
		if (_ready && !_canTransfer)
		{
			if (!_loadingSprite.IsPlaying())
			{
				_loadingGroup.Visible = true;
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
		if (_initialisationThread.IsAlive())
		{
			GD.PushWarning("thread is still doing work, so we're about to block the main thread...");
		}
		_initialisationThread.WaitToFinish();
		// this will block EVERYTHING until the thread has finished its work,
		// GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
		GetTree().ChangeSceneToPacked(_nextScene);
	}

	private void InitialiseGame()
	{
		CheckDecisionDifference();
        LoadCharacterTextures();
        LoadNextScene();
        LoadMainGame();
        LessonState.SetLessonCount();
	}

	private static void LoadCharacterTextures()
	{
		// make these not pre-determined, add custom hats, shirts, etc. to generate characters OTF
		for (int i = 1; i < Global.CharacterCount + 1; i++)
		{
			Global.CharacterTextures[i - 1] = GD.Load<Texture2D>($"res://assets/chars/character{i}.png");
		}
	}

	private void LoadNextScene()
	{
		_nextScene = GD.Load<PackedScene>("res://scenes/MainMenu.tscn");
	}

	private void LoadMainGame()
	{
		MainMenu.MainScene = GD.Load<PackedScene>("res://scenes/MainScene.tscn");
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
				GD.PushWarning($"decisions are in favour of bad! -{sum}");
				break;
		}
	}
}
