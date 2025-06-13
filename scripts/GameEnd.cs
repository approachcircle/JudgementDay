using Godot;
using System;

namespace JudgementDay;

public partial class GameEnd : Control
{
    public static GameState GameState { get; set; }

    public override void _Ready()
    {
        GetNode<RichTextLabel>("IncorrectCount").Text = $"\u274C {GameState.CharactersJudged - GameState.CorrectJudges}";
        GetNode<RichTextLabel>("CorrectCount").Text = $"\u2705 {GameState.CorrectJudges}";
        GetNode<RichTextLabel>("Accuracy").Text = $"Accuracy: {RankCalculator.CalculateAccuracy(GameState)}%";
        GetNode<RichTextLabel>("Score").Text = $"Score: {GameState.Score}";
        RichTextLabel rank = GetNode<RichTextLabel>("Rank");
        if (RankCalculator.CalculateRank(GameState) is Rank.SS)
        {
            // increase rank label size to accomodate SS
            rank.Size = new Vector2(968, 1224);
            rank.Position = new Vector2(952, 248);
        }
        rank.Text = $"{RankCalculator.CalculateRank(GameState).ToString().ToUpper()}";
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("exit"))
        {
            GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
        }
    }
}
