using UnityEngine;

// [ExtensionOfNativeClass]
public class GameInfo : MonoBehaviour
{
    public static GameInfo Instance { get; private set; }

    public static float horizontalGridSize = 11.5f;
    public static float verticalGridSize = 13f;
    public int currentHighScore;
    public int currentGems;

    public int currentSessionGems;
    public int currentSessionScore;

}