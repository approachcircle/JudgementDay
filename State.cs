using System.Collections.Generic;
using Godot;

namespace JudgementDay;

public static class State
{
    public static readonly List<int> UsedCharacters = [];
    public const int CharacterCount = 34;
    public static readonly Texture2D[] CharacterTextures = new Texture2D[CharacterCount];
    public const int DecisionsPerCharacter = 8;
}