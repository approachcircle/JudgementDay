using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JudgementDay;

public partial class Character : Node2D
{
    [Export]
    public bool IsPlayer { get; set; }

    public List<Decision> Decisions = [];
    public bool IsGood { get; private set; }

    private Texture2D _texture;
    public Texture2D Texture
    {
        get => _texture;
        private set
        {
            _texture = value;
            GetNode<Sprite2D>("Sprite").Texture = Texture;
        }
    }

    public override void _Ready()
    {
        Texture2D texture = IsPlayer
            ? GD.Load<Texture2D>("res://assets/chars/main_char_wingless.png")
            : GetRandomConstrainedTexture();
        if (texture == null)
        {
            return;
        }
        Texture = texture;
    }

    public override void _Process(double delta)
    {
        
    }
    
    private static Texture2D GetRandomConstrainedTexture()
    {
        if (Global.UsedCharacters.Count == Global.CharacterCount)
        {
            Main.GameState.GameEnding = true;
            return null;
        }
        int index;
        do
        {
            index = new Random().Next(0, Global.CharacterCount);
        }
        while (Global.UsedCharacters.Contains(index));
        
        Global.UsedCharacters.Add(index);
        return Global.CharacterTextures[index];
    }

    public void ChooseDecisions()
    {
        for (int i = 0; i < Global.DecisionsPerCharacter; i++)
        {
            Decision decision = DecisionManager.GetRandomDecision();
            if (!Decisions.Contains(decision))
            {
                Decisions.Add(decision);
                continue;
            }
            // in case we run into the same decision twice, loop again
            i--;
        }

        IsGood = Decisions.Sum(decision => decision.DecisionWeight) >= 0;
    }
}
