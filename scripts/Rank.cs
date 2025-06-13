using System.Collections.Generic;

namespace JudgementDay;

public enum Rank
{
    // ReSharper disable once InconsistentNaming
    SS,
    S,
    A,
    B,
    C,
    D
}

public class RankValue
{
    public static readonly Dictionary<Rank, int> RankValues = new()
    {
        { Rank.SS, 100 },
        { Rank.S, 96 },
        { Rank.A, 90 },
        { Rank.B, 80 },
        { Rank.C, 70 },
        { Rank.D, 50 },
    };
}