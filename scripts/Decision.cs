using System;

namespace JudgementDay;

public class Decision
{
    public string DecisionDescription;
    public int DecisionWeight;
    public int DecisionRarity;
    
    // i have no immutable fields :(
    #pragma warning disable CS0659
    public override bool Equals(object obj)
    {
        if (obj is not Decision decision)
        {
            throw new ArgumentException($"object is not an instance of {nameof(Decision)}");
        }
        return decision.DecisionDescription == DecisionDescription;
    }
}