using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    UIManager uIManager; // will know uiController
    public static System.Action PlayerFirstMove;

    [SerializeField] int highScore;
    [SerializeField] 

    
    private void OnEnable() {
        PlayerController.OnPlayerMove += OnPlayerFirstMove;
    }

    void Awake()
    {
        
        highScore = PlayerPrefs.GetInt("highscore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGameStart(){
        
        // start camera movement
        // turn off initial UI
        // enable player movement
    }

    public void OnPlayerFirstMove(){
        Debug.Log("invocando player first move");
        PlayerFirstMove?.Invoke();
        PlayerController.OnPlayerMove -= OnPlayerFirstMove;

    }
}
