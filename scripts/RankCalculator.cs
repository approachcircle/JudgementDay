using Godot;

namespace JudgementDay;

public static class RankCalculator
{
    public static Rank CalculateRank(GameState gameState)
    {
        int percentage = (int)(gameState.CorrectJudges / (float)gameState.CharactersJudged * 100);
        foreach (Rank rank in RankValue.RankValues.Keys)
        {
            int value = RankValue.RankValues[rank];
            if (rank == Rank.SS && percentage == value) return rank;
            if (rank == Rank.D && percentage < value) return rank;
            if (percentage >= value) return rank;
        }
        return Rank.F;
    }
}