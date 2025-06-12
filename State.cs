using System.Collections.Generic;
using Godot;

namespace JudgementDay;

public static class State
{
    public static List<int> UsedCharacters = [];

    public const int CharacterCount = 34;
    
    public static Texture2D[] CharacterTextures = new Texture2D[CharacterCount];

}