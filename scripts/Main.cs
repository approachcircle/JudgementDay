using Godot;
using System;

namespace JudgementDay;

public partial class Main : Node2D
{
    // private readonly Vector2 _targetPlayerPosition = new Vector2(112, 968);
    private readonly Vector2 _targetCharacterPosition = new(1440, 616);
    
    public override void _Ready()
    {
        PackedScene character = GD.Load<PackedScene>("res://scenes/Character.tscn");
        Character characterNode = character.Instantiate<Character>();
        characterNode.ChooseDecisions();
        characterNode.Position = _targetCharacterPosition;
        AddChild(characterNode);
        RichTextLabel speech = GetNode<RichTextLabel>("Speech");
        foreach (Decision decision in characterNode.Decisions)
        {
            if (decision.DecisionWeight > 0)
            {
                speech.Text = speech.Text += "\u2705";
            }
            else
            {
                speech.Text = speech.Text += "\u274C";
            }
            speech.Text = speech.Text + decision.DecisionDescription + "\n";
        }
    }
}
