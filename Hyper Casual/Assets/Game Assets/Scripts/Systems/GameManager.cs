using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    UIManager uIManager; // will know uiController
    public static System.Action PlayerFirstMove;

    void Start()
    {
        
    }

  
    void OnEnable()
    {
        PlayerController.OnPlayerMove += OnPlayerFirstMove;
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
        PlayerFirstMove?.Invoke();
        PlayerController.OnPlayerMove -= OnPlayerFirstMove;

    }
}
