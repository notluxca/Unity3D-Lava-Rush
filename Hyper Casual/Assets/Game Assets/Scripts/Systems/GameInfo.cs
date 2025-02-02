using System;
using UnityEngine;

public class GameInfo 
{
    public static GameInfo Instance { get; private set; }

    [Header("Grid Settings")]
    [SerializeField] public static float horizontalGridSize = 10f;
    [SerializeField] public static float verticalGridSize = 10f;

    

    [SerializeField] public int currentHighScore; // safe int
    [SerializeField] public int currentGems;
    
}
