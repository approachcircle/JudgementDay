using Godot;
using System;

namespace JudgementDay;

public partial class Character : Node2D
{
    [Export]
    public bool IsPlayer { get; set; }
    
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

    private Texture2D GetRandomConstrainedTexture()
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
}
