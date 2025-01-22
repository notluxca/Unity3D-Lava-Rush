using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    UIManager uIManager; // will know uiController

    [SerializeField] TMP_Text gemsText; //! outside responsability
    int currentGems;

    private void Update() {
        
    }

    
    private void OnEnable() {
        Application.targetFrameRate = 60;
        GemCollectable.gemCollected += OnGemCollected;
    }

    private void OnGemCollected()
    {
        currentGems += 1;
        gemsText.text = currentGems.ToString();
    }

    void OnGameStart(){
        
        // start camera movement
        // turn off initial UI
        // enable player movement
    }
}
