using Godot;

namespace JudgementDay;

public static class LessonState
{
    public static int CurrentLesson { get; set; } = 0;
    public static int LessonCount { get; private set; } = 0;

    public static void SetLessonCount()
    {
        CurrentLesson = 1;
        LessonCount = 0;
        while (ResourceLoader.Exists($"res://scenes/lessons/Lesson{LessonState.LessonCount + 1}.tscn"))
        {
            LessonCount++;
        }
    }
}