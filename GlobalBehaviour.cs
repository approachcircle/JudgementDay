using Godot;

namespace JudgementDay;

public partial class GlobalBehaviour : Node
{
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("exit")) GetTree().Quit();
    }
}