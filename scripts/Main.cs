using Godot;
using System;

namespace JudgementDay;

public partial class Main : Node2D
{
    // private readonly Vector2 _targetPlayerPosition = new Vector2(112, 968);
    private readonly Vector2 _targetCharacterPosition = new(1440, 616);
    private Character _characterNode;
    public static GameState GameState { get; private set; } = new();
    private CharacterOutcomeStage _stage;
    private float _gravity = .0f;
    private Color _characterColour = new();
    private AnimatedSprite2D _score;
    private Trajectory _lastTrajectory = Trajectory.Neutral;
    
    public override void _Ready()
    {
        GameState = new GameState();
        AddNewCharacter();
        SetupButtons();
        Global.UsedCharacters = [];
        _score = GetNode<AnimatedSprite2D>("Score");
        GetNode<Button>("EndGame").Pressed += () => { GameState.GameEnding = true; };
        _score.Play("neutral");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_stage is CharacterOutcomeStage.Dropping)
        {
            ApplyCharacterGravity(delta, true);
        }
        if (_stage is CharacterOutcomeStage.Burning)
        {
            BurnCharacter();
        }
        if (_stage is CharacterOutcomeStage.Flying)
        {
            ApplyCharacterGravity(delta, false);
        }
    }

    public override void _Process(double delta)
    {
        if (GameState.GameEnding)
        {
            PackedScene gameEnd = GD.Load<PackedScene>("res://scenes/GameEnd.tscn");
            GameEnd.GameState = GameState;
            GetTree().ChangeSceneToPacked(gameEnd);
            return;
        }
        if (Input.IsActionJustPressed("exit"))
        {
            GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
        }
        if (_stage is CharacterOutcomeStage.Dead)
        {
            _characterNode.QueueFree();
            AddNewCharacter();
        }
        
        if (GameState.Score > 0)
        {
            GameState.Trajectory = Trajectory.Winning;   
        } 
        else if (GameState.Score < 0)
        {
            GameState.Trajectory = Trajectory.Losing;
        }
        else
        {
            GameState.Trajectory = Trajectory.Neutral;
        }
        
        if (GameState.Trajectory != _lastTrajectory)
        {
            if (GameState.Score > 0)
                _score.PlayBackwards("winning");
            else if (GameState.Score < 0)
                _score.Play("losing");
            else
                _score.Play("neutral");
            
            _lastTrajectory = GameState.Trajectory;
        }
    }

    private void AddNewCharacter()
    {
        _stage = CharacterOutcomeStage.Alive;
        _gravity = .0f;
        _characterColour = new();
        GetNode<AnimatedSprite2D>("bg").Frame = 0;
        PackedScene character = GD.Load<PackedScene>("res://scenes/Character.tscn");
        _characterNode = character.Instantiate<Character>();
        _characterNode.ChooseDecisions();
        _characterNode.Position = _targetCharacterPosition;
        AddChild(_characterNode);
        StateDecisions();
    }

    private void StateDecisions()
    {
        RichTextLabel speech = GetNode<RichTextLabel>("Speech");
        speech.Text = "";
        foreach (Decision decision in _characterNode.Decisions)
        {
            //
            // if (decision.DecisionWeight > 0)
            // {
            //     speech.Text += "\u2705"; // green checkmark
            // }
            // else
            // {
            //     speech.Text += "\u274C"; // red cross
            // }
            speech.Text += "\u2022 " + decision.DecisionDescription + "\n";
        }
    }

    private void SetupButtons()
    {
        Button accept = GetNode<Button>("Accept/Button");
        Button deny = GetNode<Button>("Deny/Button");
        accept.Text = "\u2705";
        deny.Text = "\u274C";
        accept.Pressed += () =>
        {
            if (_stage is CharacterOutcomeStage.Alive)
            {
                JudgeCharacter(Outcome.Heaven);
            }
        };
        deny.Pressed += () =>
        {
            if (_stage is CharacterOutcomeStage.Alive)
            {
                JudgeCharacter(Outcome.Hell);
            }
        };
    }

    private void JudgeCharacter(Outcome outcome)
    {
        GameState.CharactersJudged++;
        switch (outcome)
        {
            case Outcome.Heaven:
                if (!_characterNode.IsGood) // wrong
                {
                    GameState.Score--;
                }
                else // correct
                {
                    GameState.Score++;
                    GameState.CorrectJudges++;
                }
                AnimatedSprite2D wings = _characterNode.GetNode<AnimatedSprite2D>("Wings");
                wings.Visible = true;
                wings.Play();
                _stage = CharacterOutcomeStage.Flying;
                break;
            case Outcome.Hell:
                if (_characterNode.IsGood) // wrong
                {
                    GameState.Score--;
                }
                else // correct
                {
                    GameState.Score++;
                    GameState.CorrectJudges++;
                }
                AnimatedSprite2D trapdoor = GetNode<AnimatedSprite2D>("bg");
                trapdoor.AnimationFinished += () => { _stage = CharacterOutcomeStage.Dropping; };
                trapdoor.Play("drop");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(outcome), outcome, "outcome is unspecified");
        }
    }

    private void ApplyCharacterGravity(double delta, bool falling)
    {
        _gravity += 20 * (float)delta;
        if (falling)
        {
            if (_characterNode.Position.Y < 960)
            {
                _characterNode.SetPosition(new Vector2(_characterNode.Position.X, _characterNode.Position.Y + _gravity));
            }
            else
            {
                _stage = CharacterOutcomeStage.Burning;
            }
        }
        else
        {
            // < 124
            if (_characterNode.Position.Y > 124 - _characterNode.Texture.GetHeight())
            {
                _characterNode.SetPosition(new Vector2(_characterNode.Position.X, _characterNode.Position.Y - _gravity));
            }
            else
            {
                _stage = CharacterOutcomeStage.Dead;
            }
        }
        
    }

    private void BurnCharacter()
    {
        _characterColour = _characterNode.Modulate;
        _characterColour.A -= 0.1f;
        _characterColour.R -= 0.1f;
        _characterColour.G -= 0.1f;
        _characterColour.B -= 0.1f;
        _characterNode.SetModulate(_characterColour);
        if (_characterColour.A <= 0)
        {
            _stage = CharacterOutcomeStage.Dead;
        }
    }
}

