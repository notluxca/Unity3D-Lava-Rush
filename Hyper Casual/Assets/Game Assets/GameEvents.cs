using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<int> OnGemCollected;
    public static event Action<int> OnGemCountChanged;
    public static event Action<int> OnHighScoreChanged;
    public static event Action<int> OnScoreChanged;

    public static void GemCollected(int currentGems)
    {
        OnGemCollected?.Invoke(currentGems);
    }

    public static void GemCountChanged(int currentGems)
    {
        OnGemCountChanged?.Invoke(currentGems);
    }

    public static void HighScoreChanged(int highScore){
        OnHighScoreChanged?.Invoke(highScore);
    }
    
    public static void ScoreChanged(int score){
        OnScoreChanged?.Invoke(score);
    }



}
