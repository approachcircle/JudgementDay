using Godot;
using System;

namespace JudgementDay;

public class GameState
{
    public int Score { get; set; } // difference between correct and incorrect decisions
    public Trajectory Trajectory { get; set; } = Trajectory.Neutral;
    public int CharactersJudged { get; set; } = 0;
    public int CorrectJudges { get; set; } = 0;
}
