using Godot;
using System;

namespace JudgementDay;

public partial class LessonNav : Control
{
    public override void _Ready()
    {
        Button next = GetNode<Button>("Next");
        Button back = GetNode<Button>("Back");
        next.Pressed += () =>
        {
            if (next.Text == "End")
            {
                GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
            }
            else
            {
                LessonState.CurrentLesson++;
                TransferToNewLesson();
            }
        };
        back.Pressed += () =>
        {
            // GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
            LessonState.CurrentLesson--;
            TransferToNewLesson();
        };
    }

    public override void _Process(double delta)
    {
        Button next = GetNode<Button>("Next");
        Button back = GetNode<Button>("Back");
        back.Visible = !(LessonState.CurrentLesson <= 1);
        if (LessonState.CurrentLesson == LessonState.LessonCount)
        {
            next.Text = "End";
        }
        else
        {
            next.Text = "Next";
        }
        GetNode<Label>("Lesson").Text = $"Lesson {LessonState.CurrentLesson}/{LessonState.LessonCount}";
    }

    private void TransferToNewLesson()
    {
        Console.WriteLine($"changing to {LessonState.CurrentLesson}");
        PackedScene nextScene = GD.Load<PackedScene>($"res://scenes/lessons/Lesson{LessonState.CurrentLesson}.tscn");
        GetTree().ChangeSceneToPacked(nextScene);
    }
}
