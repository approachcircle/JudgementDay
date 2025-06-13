using Godot;

namespace JudgementDay;

public class LessonState
{
    public static int CurrentLesson { get; set; } = 0;
    public static int LessonCount { get; set; } = 0;

    public static void SetLessonCount()
    {
        LessonState.CurrentLesson = 1;
        LessonState.LessonCount = 0;
        while (FileAccess.FileExists($"res://scenes/lessons/Lesson{LessonState.LessonCount + 1}.tscn"))
        {
            LessonState.LessonCount++;
        }
    }
}