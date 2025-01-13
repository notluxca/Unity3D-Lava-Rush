using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static GameInfo Instance { get; private set; }

    [Header("Grid Settings")]
    [SerializeField] public float horizontalGridSize = 10f;
    [SerializeField] public float verticalGridSize = 10f;

    [SerializeField] public int currentGems;
    

    public int currentHighScore; // safe int
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
