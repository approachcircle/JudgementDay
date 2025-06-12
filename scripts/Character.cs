using Godot;
using System;
using System.Collections.Generic;

namespace JudgementDay;

public partial class Character : Node2D
{
    [Export]
    public bool IsPlayer { get; set; }

    private List<Decision> _decisions = [];
    
    private Texture2D _texture;
    public Texture2D Texture
    {
        get => _texture;
        set
        {
            _texture = value;
            GetNode<Sprite2D>("Sprite").Texture = Texture;
        }
    }

    public override void _Ready()
    {
        Texture = IsPlayer
            ? GD.Load<Texture2D>("res://assets/chars/main_char_wingless.png")
            : GetRandomConstrainedTexture();
    }

    public override void _Process(double delta)
    {
        
    }

    private static Texture2D GetRandomConstrainedTexture()
    {
        int index;
        do
        {
            index = new Random().Next(0, State.CharacterCount);
        }
        while (State.UsedCharacters.Contains(index));
        
        State.UsedCharacters.Add(index);
        return State.CharacterTextures[index];
    }

    public void ChooseDecisions()
    {
        for (int i = 0; i < State.DecisionsPerCharacter; i++)
        {
            Decision decision = DecisionManager.GetRandomDecision();
            if (!_decisions.Contains(decision))
            {
                _decisions.Add(decision);
                continue;
            }
            // in case we run into the same decision twice, loop again
            i--;
        }
        // _decisions.ForEach(decision => { GD.Print(decision.DecisionDescription); });
    }
}
