using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<int> OnGemCollected;
    public static event Action<int> OnGemCountChanged;

    public static void GemCollected(int currentGems)
    {
        OnGemCollected?.Invoke(currentGems);
    }

    public static void GemCountChanged(int currentGems)
    {
        OnGemCountChanged?.Invoke(currentGems);
    }



}
