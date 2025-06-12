using Godot;
using System;

namespace JudgementDay;

public partial class Main : Node2D
{
    // private readonly Vector2 _targetPlayerPosition = new Vector2(112, 968);
    private readonly Vector2 _targetCharacterPosition = new(1800, 968);
    
    public override void _Ready()
    {
        PackedScene character = GD.Load<PackedScene>("res://scenes/Character.tscn");
        Character characterNode = character.Instantiate<Character>();
        characterNode.ChooseDecisions();
        characterNode.Position = _targetCharacterPosition;
        AddChild(characterNode);
    }
}
